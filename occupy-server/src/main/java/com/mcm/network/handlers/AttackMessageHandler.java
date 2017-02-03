package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class AttackMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(AttackMessageHandler.class);
    @Autowired
    IGameObjectDao gameObjectDao;

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("attack msg: " + message);
        String unitId = message.Params.get(0);
        String towerId = message.Params.get(1);

        message.Params.clear();

        Unit unit = gameObjectDao.findUnitById(unitId);
        Tower targetTower = gameObjectDao.findTowerById(towerId);
        if(targetTower == null) {
            message.ExceptionMessage = "Invalid tower id.";
            return message;
        }



        return message;
    }
}
