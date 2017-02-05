package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.Line;
import com.mcm.entities.Path;
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
public class AttackMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(AttackMessageHandler.class);
    @Autowired
    IGameObjectDao gameObjectDao;
    @Autowired
    IMoveEventDao moveEventDao;
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("attack msg: " + message);
        String unitId = message.Params.get(0);
        String towerId = message.Params.get(1);

        message.Params.clear();

        Unit unit = gameObjectDao.findUnitById(unitId);
        Tower keepingTower = gameObjectDao.findTowerById(unit.getKeepingTowerId());
        Tower targetTower = gameObjectDao.findTowerById(towerId);
        if(targetTower == null) {
            message.ExceptionMessage = "Invalid tower id.";
            logger.error(message.ExceptionMessage);
            return message;
        }
        if (Objects.equals(targetTower.getPlayerId(), unit.getPlayerId())) {
            message.ExceptionMessage = "unit cant attack own tower";
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
        Path path = new Path();
        Line line = new Line(keepingTower.getLocation(), targetTower.getLocation());
        path.lines.add(line);
        me.setPath(path);
        me.setAttackMode(true);
        moveEventDao.save(me);
        unit.setMoving(true);
        gameObjectDao.save(unit);



        return message;
    }
}
