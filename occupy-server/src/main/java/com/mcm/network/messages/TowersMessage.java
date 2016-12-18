package com.mcm.network.messages;

import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;

import java.util.LinkedList;
import java.util.UUID;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class TowersMessage {
    public TowersMessage(){

    }



    public String Id = UUID.randomUUID().toString();
    public String Cmd = "";
    public LinkedList<String> Params = new LinkedList<>();
    public String PlayerKey = "";
    public String ExceptionMessage = "";

    @Override
    public String toString() {
        return "SocketMessage{" +
                "Id='" + Id + '\'' +
                ", Cmd='" + Cmd + '\'' +
                ", PlayerKey='" + PlayerKey + '\'' +
                ", ExceptionMessage='" + ExceptionMessage + '\'' +
                ", Params=" + Params +
                '}';
    }
}
