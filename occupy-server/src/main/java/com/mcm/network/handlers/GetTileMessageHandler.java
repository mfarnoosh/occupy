package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.TowerData;
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
public class GetTileMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(GetTileMessageHandler.class);

    @Autowired
    private IGameObjectDao gameObjectDao;

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

        List<Tower> towers = gameObjectDao.getAllTowersInBox(
                new double[]{tile.getBoundingBox().south,tile.getBoundingBox().west} ,
                new double[]{tile.getBoundingBox().north,tile.getBoundingBox().east});

        double tile_x = tile.getBoundingBox().east - tile.getBoundingBox().west;
        double tile_y = tile.getBoundingBox().north - tile.getBoundingBox().south;

        message.Params.add(String.valueOf(towers.size())); //number of towers
        for(Tower t : towers){
            message.Params.add(new Gson().toJson(new TowerData(t)));
        }

        return message;
    }
}
