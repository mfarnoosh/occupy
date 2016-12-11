package com.mcm.network.messages.handlers;

import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import org.apache.log4j.Logger;

/**
 * Created by Mehrdad on 16/12/11.
 */
public class EchoMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(EchoMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("echo message: " + message);
        return message;
    }
}
