package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.Line;
import com.mcm.entities.Path;
import com.mcm.entities.World;
import com.mcm.entities.mongo.events.MoveEvent;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.Objects;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class SendUnitMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(SendUnitMessageHandler.class);
    @Autowired
    IGameObjectDao gameObjectDao;
    @Autowired
    IMoveEventDao moveEventDao;
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Send unit msg: " + message);
        String unitId = message.Params.get(0);
        String towerId = message.Params.get(1);

        message.Params.clear();

        Unit unit = gameObjectDao.findUnitById(unitId);
        Tower targetTower = gameObjectDao.findTowerById(towerId);
        Tower keepingTower = gameObjectDao.findTowerById(unit.getKeepingTowerId());
        if(targetTower == null){
            message.ExceptionMessage = "Invalid tower id.";
            return message;
        }
        if (!Objects.equals(targetTower.getPlayerId(), unit.getPlayerId())) {
            message.ExceptionMessage = "unit cant move to another player tower";
            logger.error(message.ExceptionMessage);
            return message;
        }
        if (unit.isAttacking() || unit.isMoving()) {
            message.ExceptionMessage = "unit should not be on attacking or moving";
            logger.error(message.ExceptionMessage);
            return message;
        }
        moveEventDao.delete(unit.getId(), targetTower.getId());
        MoveEvent me = new MoveEvent();
        me.setGameObjectId(unit.getId());
        me.setTargetTowerId(targetTower.getId());
        me.setTargetTowerLocation(targetTower.getLocation());
        Path path = World.findPath(keepingTower.getLocation(), targetTower.getLocation());
        me.setPath(path);
        moveEventDao.save(me);
        unit.setMoving(true);
        gameObjectDao.save(unit);
        for (Line line: path.lines) {
            message.Params.add(String.valueOf(line.getEnd()[0]));
            message.Params.add(String.valueOf(line.getEnd()[1]));
        }
        return message;
    }
}
