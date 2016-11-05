package com.mcm.service;

import com.google.gson.Gson;
import org.apache.log4j.Logger;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.ProtocolException;
import java.nio.charset.StandardCharsets;
import java.util.Map;

/**
 * Created by Ashki on 11/30/2015.
 */
public class ConnectionService {

    private static Logger log = Logger.getLogger(ConnectionService.class);



    public static void addPostParameter(HttpURLConnection con, Map<String, Object> map) {
        try {
            con.setRequestMethod("POST");
            byte[] postData = new Gson().toJson(map).getBytes(StandardCharsets.UTF_8);
            int postDataLength = postData.length;
            con.setRequestProperty("Content-Length", Integer.toString(postDataLength));
            con.setDoOutput(true);
            try (DataOutputStream wr = new DataOutputStream(con.getOutputStream())) {
                wr.write(postData);
                wr.flush();
                wr.close();
            } catch (IOException e) {
                log.error("GCM err "+e.getMessage());
            }

        } catch (ProtocolException e) {
            log.error("GCM err "+e.getMessage());
        }

    }

    public static String getResponse(HttpURLConnection con) {
        try {
            con.connect();
            return new BufferedReader(new InputStreamReader(con.getInputStream())).readLine();
        } catch (Exception e) {
            log.error("GCM err "+e.getMessage());
        }
//        log.info("GCM ERR!!");
        return "";
    }


}
