package com.mcm.network;

import com.google.gson.Gson;
import com.sun.xml.internal.messaging.saaj.util.ByteOutputStream;
import org.apache.log4j.Logger;

import java.io.DataInputStream;
import java.io.IOException;
import java.net.Socket;
import java.util.Date;

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
            clientOutData = handleCommand();
            client.getOutputStream().write(clientOutData.getBytes("UTF-8"));
            client.getOutputStream().flush();
            client.getOutputStream().close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private String handleCommand() {
        SocketMessage socketMessage = new Gson().fromJson(clientData, SocketMessage.class);
        switch (socketMessage.Cmd) {
            case "echo":
                return new Gson().toJson(socketMessage);
            case "updateLoc":
                logger.info(socketMessage);
                float lat = Float.parseFloat(socketMessage.Params.get(0));
                float lon = Float.parseFloat(socketMessage.Params.get(1));
                float alt = Float.parseFloat(socketMessage.Params.get(2));
                float accuracy = Float.parseFloat(socketMessage.Params.get(3));
                double timeStamp = Double.parseDouble(socketMessage.Params.get(4));
                Date date = new Date((long)timeStamp * 1000);

                return "";
            default:
                return "";
        }


    }
}
