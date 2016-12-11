package com.mcm.network.messages.handlers;

import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import org.apache.log4j.Logger;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.Date;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class UpdateLocMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(UpdateLocMessageHandler.class);

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("UpdateLoc msg: " + message);
        float lat = Float.parseFloat(message.Params.get(0));
        float lon = Float.parseFloat(message.Params.get(1));
        float alt = Float.parseFloat(message.Params.get(2));
        float accuracy = Float.parseFloat(message.Params.get(3));
        double timeStamp = Double.parseDouble(message.Params.get(4));
        Date date = new Date((long) timeStamp * 1000);
        return null;
    }
}
