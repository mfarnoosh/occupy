package com.mcm.network;

import com.google.gson.Gson;
import com.mcm.service.Tile;
import com.mcm.util.GeoUtil;
import com.mcm.util.SharedPreference;
import com.sun.xml.internal.messaging.saaj.util.ByteOutputStream;
import org.apache.axis.encoding.Base64;
import org.apache.commons.io.FileUtils;
import org.apache.log4j.Logger;
import org.springframework.data.geo.Point;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.File;
import java.io.IOException;
import java.net.Socket;
import java.net.URL;
import java.net.URLConnection;
import java.util.Date;
import java.util.List;

/**
 * Created by alirezaghias on 11/3/2016 AD.
 */
public class ClientService implements Runnable {
    private Logger logger = Logger.getLogger(ClientService.class);
    private Socket client;
    private String clientData = "";
    private String clientOutData = "";

    public ClientService(Socket client) throws IOException {
        this.client = client;
        this.client.setSoTimeout(45000);
    }

    private void readClientData() throws IOException {
        DataInputStream dis = new DataInputStream(client.getInputStream());
        byte[] data = new byte[1024];
        ByteOutputStream bos = new ByteOutputStream();
        int count = 0;
        int length = 0;
        while ((count = dis.read(data)) != -1) {
            bos.write(data, 0, count);
            length += count;
            byte[] bytes = bos.getBytes();
            if (length >= 7) {
                byte[] dest = new byte[7];
                System.arraycopy(bytes, length - 7, dest, 0, 7);
                String finished = new String(dest, "UTF-8");
                if ("__FIN__".equals(finished))
                    break;
            }
        }
        if (length > 0) {
            clientData = new String(bos.getBytes(), "UTF-8").trim();
            clientData = clientData.substring(0, clientData.length() - 7);
        }
    }

    @Override
    public void run() {
        try {
            readClientData();
            SocketMessage socketMessage = handleCommand();
            if (socketMessage == null)
                clientOutData = "";
            else {
                clientOutData = new Gson().toJson(socketMessage);
            }
            byte[] bytes = clientOutData.getBytes("UTF-8");
            client.getOutputStream().write(bytes);
            client.getOutputStream().flush();
            client.getOutputStream().close();
            logger.info("sent -> " + bytes.length + " byte");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private SocketMessage handleCommand() {
        SocketMessage socketMessage = new Gson().fromJson(clientData, SocketMessage.class);
        switch (socketMessage.Cmd) {
            case "echo":
                return socketMessage;
            case "updateLoc":
                return handleUpdateLoc(socketMessage);
            case "getTile":
                return handleTile(socketMessage);
            case "getTileByNumber":
                return handleTileByTileNo(socketMessage);
            case "saveBuilding":
                return handleSaveBuilding(socketMessage);
            default:
                return null;
        }


    }

    private SocketMessage handleTile(SocketMessage socketMessage) {
        logger.info(socketMessage);
        float lat = Float.parseFloat(socketMessage.Params.get(0));
        float lon = Float.parseFloat(socketMessage.Params.get(1));
        Tile tile = new Tile(lat,lon);

        socketMessage.Params.clear();
        socketMessage.Params.add(Base64.encode(tile.getImage()));

        socketMessage.Params.add(String.valueOf(tile.getCenter().getX()));
        socketMessage.Params.add(String.valueOf(tile.getCenter().getY()));

        socketMessage.Params.add(String.valueOf(tile.getBoundingBox().north));
        socketMessage.Params.add(String.valueOf(tile.getBoundingBox().east));
        socketMessage.Params.add(String.valueOf(tile.getBoundingBox().south));
        socketMessage.Params.add(String.valueOf(tile.getBoundingBox().west));


        socketMessage.Params.add(String.valueOf(tile.getTileX()));
        socketMessage.Params.add(String.valueOf(tile.getTileY()));

//        socketMessage.Params.add(String.valueOf(35.70283f));
//        socketMessage.Params.add(String.valueOf(51.40641f));
//        socketMessage.Params.add(String.valueOf(35.72403f));
//        socketMessage.Params.add(String.valueOf(51.44572f));
//        socketMessage.Params.add(String.valueOf(35.72215f));
//        socketMessage.Params.add(String.valueOf(51.44447f));
//        socketMessage.Params.add(String.valueOf(35.72046f));
//        socketMessage.Params.add(String.valueOf(51.44354f));
//        socketMessage.Params.add(String.valueOf(35.71995f));
//        socketMessage.Params.add(String.valueOf(51.44778f));

        return socketMessage;
    }

    private SocketMessage handleTileByTileNo(SocketMessage socketMessage) {
        logger.info(socketMessage);
        int tileX = Integer.parseInt(socketMessage.Params.get(0));
        int tileY = Integer.parseInt(socketMessage.Params.get(1));

        Tile tile = new Tile(tileX,tileY);

        socketMessage.Params.clear();
        socketMessage.Params.add(Base64.encode(tile.getImage()));

        socketMessage.Params.add(String.valueOf(tile.getCenter().getX()));
        socketMessage.Params.add(String.valueOf(tile.getCenter().getY()));

        return socketMessage;
    }
    private SocketMessage handleSaveBuilding(SocketMessage socketMessage){
        logger.info(socketMessage);
        double lat = Double.parseDouble(socketMessage.Params.get(0));
        double lon = Double.parseDouble(socketMessage.Params.get(1));

        socketMessage.Params.clear();


        socketMessage.Params.add(String.valueOf(35.70283f));
        socketMessage.Params.add(String.valueOf(51.40641f));


        socketMessage.Params.add(String.valueOf(35.70200f));
        socketMessage.Params.add(String.valueOf(51.40987f));


        socketMessage.Params.add(String.valueOf(35.70536f));
        socketMessage.Params.add(String.valueOf(51.40976f));


//        socketMessage.Params.add(String.valueOf(35.70374f));
//        socketMessage.Params.add(String.valueOf(51.40529f));

        socketMessage.Params.add(String.valueOf(35.702305));
        socketMessage.Params.add(String.valueOf(51.400487));


        return socketMessage;
    }
    private SocketMessage handleUpdateLoc(SocketMessage socketMessage) {
        logger.info(socketMessage);
        float lat = Float.parseFloat(socketMessage.Params.get(0));
        float lon = Float.parseFloat(socketMessage.Params.get(1));
        float alt = Float.parseFloat(socketMessage.Params.get(2));
        float accuracy = Float.parseFloat(socketMessage.Params.get(3));
        double timeStamp = Double.parseDouble(socketMessage.Params.get(4));
        Date date = new Date((long) timeStamp * 1000);
        return null;
    }
}
