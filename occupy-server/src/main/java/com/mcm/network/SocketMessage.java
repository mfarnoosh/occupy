package com.mcm.network;

import java.util.LinkedList;
import java.util.UUID;

/**
 * Created by alirezaghias on 11/5/2016 AD.
 */
public class SocketMessage {
    public String Id = UUID.randomUUID().toString();
    public String Cmd = "";
    public LinkedList<String> Params = new LinkedList<>();

    @Override
    public String toString() {
        return "SocketMessage{" +
                "Id='" + Id + '\'' +
                ", Cmd='" + Cmd + '\'' +
                ", Params=" + Params +
                '}';
    }
}
