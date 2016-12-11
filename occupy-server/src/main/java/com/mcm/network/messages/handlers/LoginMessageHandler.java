package com.mcm.network.messages.handlers;

import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import org.apache.log4j.Logger;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class LoginMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(LoginMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Login msg: " + message);
        logger.info("Login: key = " + message.PlayerKey + " configVersion = " + message.Params.get(0));
        return null;
    }
}
