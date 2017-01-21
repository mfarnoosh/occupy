package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.enums.TowerType;
import com.mcm.enums.UnitType;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.messages.UnitData;
import org.apache.commons.lang3.StringUtils;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;


/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class CreateUnitMessageHandler extends BaseMessageHandler {
    @Autowired
    protected IPlayerDao playerDao;
    @Autowired
    protected IGameObjectDao gameObjectDao;
    private Logger logger = Logger.getLogger(CreateUnitMessageHandler.class);

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Create unit msg: " + message);

        Player player = null;
        if (!StringUtils.isEmpty(message.PlayerKey))
            player = playerDao.findById(message.PlayerKey);

        String towerId = message.Params.get(0);
        int unitType = Integer.valueOf(message.Params.get(1).toString());


        message.Params.clear();

        if (player == null) {
            message.ExceptionMessage = "Invalid Player key";
            return message;
        }
        if (TowerType.valueOf(unitType) == null) {
            message.ExceptionMessage = "Invalid Unit Type.";
            return message;
        }

        Tower tower = gameObjectDao.findTowerById(towerId);
        if(tower == null){
            message.ExceptionMessage = "Invalid Tower.";
            return message;
        }

        Unit newUnit = new Unit(UnitType.valueOf(unitType),player,tower);
        gameObjectDao.save(newUnit);

        message.Params.add(new Gson().toJson(new UnitData(newUnit)));

        return message;
    }
}
