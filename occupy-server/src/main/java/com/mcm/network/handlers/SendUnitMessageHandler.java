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

        MoveEvent me = new MoveEvent();
        me.setGameObjectId(unit.getId());
        me.setTargetTowerId(targetTower.getId());
        me.setTargetTowerLocation(targetTower.getLocation());
        Path path = new Path();
        Line line = new Line(keepingTower.getLocation(), targetTower.getLocation());
        path.lines.add(line);
        me.setPath(path);
        moveEventDao.save(me);
        unit.setMoving(true);
        gameObjectDao.save(unit);


        return message;
    }
}
