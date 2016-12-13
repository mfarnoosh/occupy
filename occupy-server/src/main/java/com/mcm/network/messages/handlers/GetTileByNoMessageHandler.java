package com.mcm.network.messages.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import com.mcm.service.Tile;
import org.apache.axis.encoding.Base64;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.List;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class GetTileByNoMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(GetTileByNoMessageHandler.class);

    @Autowired
    private IGameObjectDao gameObjectDao;

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Get Tile by no msg: " + message);
        int tileX = Integer.parseInt(message.Params.get(0));
        int tileY = Integer.parseInt(message.Params.get(1));

        Tile tile = new Tile(tileX,tileY);

        message.Params.clear();
        message.Params.add(Base64.encode(tile.getImage()));

        message.Params.add(String.valueOf(tile.getCenter().getX()));
        message.Params.add(String.valueOf(tile.getCenter().getY()));

        message.Params.add(String.valueOf(tile.getBoundingBox().north));
        message.Params.add(String.valueOf(tile.getBoundingBox().east));
        message.Params.add(String.valueOf(tile.getBoundingBox().south));
        message.Params.add(String.valueOf(tile.getBoundingBox().west));

        List<Tower> towers = gameObjectDao.getAllTowersInBox(
                new double[]{tile.getBoundingBox().south,tile.getBoundingBox().west} ,
                new double[]{tile.getBoundingBox().north,tile.getBoundingBox().east});
        message.Params.add(String.valueOf(towers.size())); //number of towers
        for(Tower t : towers){
            message.Params.add(String.valueOf(t.getType().getValue())); //tower type
            message.Params.add(String.valueOf(t.getId())); //tower id
            message.Params.add(String.valueOf(t.getLocation()[0])); //lat
            message.Params.add(String.valueOf(t.getLocation()[1])); //lon
        }
        return message;
    }
}
