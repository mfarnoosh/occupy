package com.mcm.network.handlers;

import com.mcm.network.messages.SocketMessage;
import com.mcm.network.BaseMessageHandler;
import org.apache.log4j.Logger;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class SendUnitMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(SendUnitMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Send unit msg: " + message);
        int buildingType = Integer.parseInt(message.Params.get(0));

        return null;
    }
}
