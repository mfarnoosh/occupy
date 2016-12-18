package com.mcm.network;

import com.mcm.network.handlers.*;
import com.mcm.network.messages.SocketMessage;
import com.mcm.util.Spring;

/**
 * Created by Mehrdad on 16/12/11.
 */
public abstract class MessageFactory {

    public static SocketMessage handleMessage(SocketMessage socketMessage){
        BaseMessageHandler handler = getHandler(socketMessage.Cmd);
        if(handler != null){
            return handler.handle(socketMessage);
        }else{
            //TODO: Farnoosh - invalid message command
            return null;
        }
    }

    private static BaseMessageHandler getHandler(String command){
        switch (command) {
            case "echo":
                return (EchoMessageHandler)Spring.context.getBean("echoMessageHandler");
            case "updateLoc":
                return (UpdateLocMessageHandler)Spring.context.getBean("updateLocMessageHandler");
            case "getTile":
                return (GetTileMessageHandler)Spring.context.getBean("getTileMessageHandler");
            case "getTileByNumber":
                return (GetTileByNoMessageHandler)Spring.context.getBean("getTileByNoMessageHandler");
            case "createTower":
                return (CreateTowerMessageHandler)Spring.context.getBean("createTowerMessageHandler");
            case "moveTower":
                return (MoveTowerMessageHandler)Spring.context.getBean("moveTowerMessageHandler");
            case "sendUnit":
                return (SendUnitMessageHandler)Spring.context.getBean("sendUnitMessageHandler");
            case "login":
                return (LoginMessageHandler)Spring.context.getBean("loginMessageHandler");
            case "signup":
                return (SignupMessageHandler)Spring.context.getBean("signupMessageHandler");
        }
        return null;
    }
}
