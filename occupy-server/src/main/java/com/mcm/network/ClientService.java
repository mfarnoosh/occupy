package com.mcm.network;

import com.sun.xml.internal.messaging.saaj.util.ByteInputStream;
import com.sun.xml.internal.messaging.saaj.util.ByteOutputStream;

import java.io.BufferedInputStream;
import java.io.DataInputStream;
import java.io.IOException;
import java.net.Socket;

/**
 * Created by alirezaghias on 11/3/2016 AD.
 */
public class ClientService implements Runnable {
    private Socket client;
    private String clientData = "";
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
            if (count >= 7) {
                byte[] dest = new byte[7];
                System.arraycopy(data, count - 7, dest, 0, 7);
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
            client.getOutputStream().write(clientData.getBytes("UTF-8"));
            client.getOutputStream().flush();
            client.getOutputStream().close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
