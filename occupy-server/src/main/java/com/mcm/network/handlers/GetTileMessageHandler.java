package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.messages.TileData;
import com.mcm.network.messages.TowerData;
import com.mcm.service.Tile;
import org.apache.axis.encoding.Base64;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.LinkedList;
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
        Tile tile = new Tile(lat, lon);

        TileData tileData = new TileData();
        tileData.ImageBytes = Base64.encode(tile.getImage());

        tileData.CenterLat = (float) tile.getCenter().getX();
        tileData.CenterLon = (float) tile.getCenter().getY();

        tileData.North = (float) tile.getBoundingBox().north;
        tileData.East = (float) tile.getBoundingBox().east;
        tileData.South = (float) tile.getBoundingBox().south;
        tileData.West = (float) tile.getBoundingBox().west;

        tileData.PositionX = tile.getTileX();
        tileData.PositionY = tile.getTileY();

        List<Tower> towers = gameObjectDao.getAllTowersInBox(
                new double[]{tile.getBoundingBox().south, tile.getBoundingBox().west},
                new double[]{tile.getBoundingBox().north, tile.getBoundingBox().east});

        List<TowerData> towersData = new LinkedList<>();
        for(Tower tower : towers) {
            towersData.add(new TowerData(tower,gameObjectDao.findUnitByOwnerTower(tower)));
        }

        tileData.towers = towersData;

        /* TODO: Farnoosh - see the todo description in GetTileByNoMessageHandler class
        List<Unit> units = gameObjectDao.getAllUnitsInBox(
                new double[]{tile.getBoundingBox().south, tile.getBoundingBox().west},
                new double[]{tile.getBoundingBox().north, tile.getBoundingBox().east});

        List<UnitData> unitsData = new LinkedList<>();
        unitsData.addAll(units.stream().map(UnitData::new).collect(Collectors.toList()));
        tileData.units = unitsData;*/

        message.Params.clear();
        message.Params.add(new Gson().toJson(tileData));

        return message;
    }
}
