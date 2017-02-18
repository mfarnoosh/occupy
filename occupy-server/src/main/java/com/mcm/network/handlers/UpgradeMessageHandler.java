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
public class UpgradeMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(UpgradeMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Upgrade tower request message: " + message.Params.get(0));
        message.Params.clear();
        return message;
    }
}
