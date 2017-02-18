package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.enums.TowerPropertyType;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import com.mcm.util.GameConfig;
import com.mcm.util.Spring;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class RepairMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(RepairMessageHandler.class);
    @Autowired
    IGameObjectDao gameObjectDao;
    @Override
    public SocketMessage handle(SocketMessage message) {
        String towerId = message.Params.get(0);
        logger.info("Repair tower request message: " + towerId);
        Tower tower = gameObjectDao.findTowerById(towerId);
        if (tower != null) {
            tower.setCurrentHitPoint(Double.parseDouble(GameConfig.getTowerProperty(tower.getType(), tower.getLevel(), TowerPropertyType.HIT_POINT)));
            gameObjectDao.save(tower);
        }

        return message;
    }
}
