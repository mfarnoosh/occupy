package com.mcm.network.messages;

import com.mcm.network.SocketMessage;
import com.mcm.network.messages.handlers.*;

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
                return new EchoMessageHandler();
            case "updateLoc":
                return new UpdateLocMessageHandler();
            case "getTile":
                return new GetTileMessageHandler();
            case "getTileByNumber":
                return new GetTileByNoMessageHandler();
            case "createTower":
                return new CreateTowerMessageHandler();
            case "moveTower":
                return new MoveTowerMessageHandler();
            case "sendUnit":
                return new SendUnitMessageHandler();
            case "login":
                return new LoginMessageHandler();
            case "signup":
                return new SignupMessageHandler();
        }
        return null;
    }
}
