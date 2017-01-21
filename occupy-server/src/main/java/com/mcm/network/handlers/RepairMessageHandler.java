package com.mcm.network.handlers;

import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import org.apache.log4j.Logger;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class RepairMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(RepairMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Repair tower request message: " + message.Params.get(0));
        message.Params.clear();
        return message;
    }
}
