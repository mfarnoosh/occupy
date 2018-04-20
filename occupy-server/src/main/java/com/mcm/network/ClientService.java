package com.mcm.network;

import com.google.gson.Gson;
import com.mcm.network.messages.SocketMessage;
import org.apache.commons.lang3.StringUtils;
import org.apache.log4j.Logger;

import java.io.*;
import java.net.Socket;

/**
 * Created by alirezaghias on 11/3/2016 AD.
 */
public class ClientService implements Runnable {
    private Logger logger = Logger.getLogger(ClientService.class);
    private Socket client;

    public ClientService(Socket client) throws IOException {
        this.client = client;
        this.client.setSoTimeout(45000);
    }


    @Override
    public void run() {
        try {
            String clientData = readClientData(client.getInputStream());
            String clientOutData = handleCommand(clientData);
            sendDataToClient(client.getOutputStream(),clientOutData);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private String readClientData(InputStream inputStream) throws IOException {
        DataInputStream dis = new DataInputStream(inputStream);
        byte[] data = new byte[1024];
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        int count;
        int length = 0;
        while ((count = dis.read(data)) != -1) {
            bos.write(data, 0, count);
            length += count;
            byte[] bytes = bos.toByteArray();
            if (length >= 7) {
                byte[] dest = new byte[7];
                System.arraycopy(bytes, length - 7, dest, 0, 7);
                String finished = new String(dest, "UTF-8");
                if ("__FIN__".equals(finished))
                    break;
            }
        }
        String readData = "";
        if (length > 0) {
            readData = new String(bos.toByteArray(), "UTF-8").trim();
            readData = readData.substring(0, readData.length() - 7);
        }
        return readData;
    }

    private void sendDataToClient(OutputStream outputStream,String clientOutData) throws IOException {
        if(StringUtils.isEmpty(clientOutData))
            throw new IOException("Sending data is empty");

        byte[] bytes = clientOutData.getBytes("UTF-8");
        outputStream.write(bytes);
        outputStream.flush();
        outputStream.close();
    }
    private String handleCommand(String clientData) {
        SocketMessage receivedMessage = new Gson().fromJson(clientData, SocketMessage.class);
        SocketMessage sendMessage = MessageFactory.handleMessage(receivedMessage);
        if (sendMessage != null)
            return new Gson().toJson(sendMessage);
        return "";
    }
}
