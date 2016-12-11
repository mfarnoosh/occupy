package com.mcm.network.messages.handlers;

import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import com.mcm.service.Tile;
import org.apache.axis.encoding.Base64;
import org.apache.log4j.Logger;

/**
 * Created by Mehrdad on 16/12/11.
 */
public class GetTileMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(GetTileMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Get tile msg: " + message);
        float lat = Float.parseFloat(message.Params.get(0));
        float lon = Float.parseFloat(message.Params.get(1));
        Tile tile = new Tile(lat,lon);

        message.Params.clear();
        message.Params.add(Base64.encode(tile.getImage()));

        message.Params.add(String.valueOf(tile.getCenter().getX()));
        message.Params.add(String.valueOf(tile.getCenter().getY()));

        message.Params.add(String.valueOf(tile.getBoundingBox().north));
        message.Params.add(String.valueOf(tile.getBoundingBox().east));
        message.Params.add(String.valueOf(tile.getBoundingBox().south));
        message.Params.add(String.valueOf(tile.getBoundingBox().west));


        message.Params.add(String.valueOf(tile.getTileX()));
        message.Params.add(String.valueOf(tile.getTileY()));

//        message.Params.add(String.valueOf(35.70283f));
//        message.Params.add(String.valueOf(51.40641f));
//        message.Params.add(String.valueOf(35.72403f));
//        message.Params.add(String.valueOf(51.44572f));
//        message.Params.add(String.valueOf(35.72215f));
//        message.Params.add(String.valueOf(51.44447f));
//        message.Params.add(String.valueOf(35.72046f));
//        message.Params.add(String.valueOf(51.44354f));
//        message.Params.add(String.valueOf(35.71995f));
//        message.Params.add(String.valueOf(51.44778f));

        return message;
    }
}
