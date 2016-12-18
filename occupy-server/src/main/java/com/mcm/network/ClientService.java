package com.mcm.network;

import com.google.gson.Gson;
import com.mcm.network.messages.SocketMessage;
import com.sun.xml.internal.messaging.saaj.util.ByteOutputStream;
import org.apache.log4j.Logger;

import java.io.DataInputStream;
import java.io.IOException;
import java.net.Socket;

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
        int count;
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
            //logger.info("sent -> " + bytes.length + " byte");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private SocketMessage handleCommand() {
        SocketMessage socketMessage = new Gson().fromJson(clientData, SocketMessage.class);

        return MessageFactory.handleMessage(socketMessage);
    }
}
