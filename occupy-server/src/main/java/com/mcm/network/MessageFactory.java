package com.mcm.network;

import com.mcm.network.handlers.*;
import com.mcm.network.messages.SocketMessage;
import com.mcm.util.Spring;
import org.apache.log4j.Logger;

/**
 * Created by Mehrdad on 16/12/11.
 */
public abstract class MessageFactory {
    private static Logger logger = Logger.getLogger(MessageFactory.class);
    public static SocketMessage handleMessage(SocketMessage socketMessage){
        BaseMessageHandler handler = getHandler(socketMessage.Cmd);
        if(handler != null){
            return handler.handle(socketMessage);
        }else{
            //TODO: Farnoosh - invalid message command
            logger.error(socketMessage);
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
            case "getTowerData":
                return (GetTowerDataMessageHandler)Spring.context.getBean("getTowerDataMessageHandler");
            case "createUnit":
                return (CreateUnitMessageHandler)Spring.context.getBean("createUnitMessageHandler");
            case "log":
                return (LogMessageHandler)Spring.context.getBean("logMessageHandler");
            case "repairTower":
                return (RepairMessageHandler)Spring.context.getBean("repairMessageHandler");
        }
        return null;
    }
}
