package com.mcm.processors;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.mongo.events.MoveEvent;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.exceptions.NotValidPathException;
import com.mcm.util.GeoUtil;
import com.mcm.util.Spring;

import java.util.Date;
import java.util.List;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class MoveEventProcessor extends EventProcessor<MoveEvent> {
    private IGameObjectDao gameObjectDao = Spring.context.getBean(IGameObjectDao.class);
    private IMoveEventDao moveEventDao = Spring.context.getBean(IMoveEventDao.class);

    public MoveEventProcessor(List<MoveEvent> batch) {
        super(batch);
    }

    @Override
    void doJob(List<MoveEvent> batch) {
        for (MoveEvent moveEvent : batch) {
            Unit unit = (Unit) gameObjectDao.findUnitById(moveEvent.getGameObjectId());
            try {
                long t = (new Date().getTime() - moveEvent.getCreated().getTime()) / 1000; //time in seccond
                double v = unit.getSpeed();
                double x = v * t; // distance in meter if velocity is m/s
                double xInKilometer = x / 1000;
                try {
                    double[] newLoc = GeoUtil.latLonOf(xInKilometer, moveEvent.getPath());
                    unit.setLocation(newLoc);
                    unit.setMoving(true);
                    if (gameObjectDao.isArrived(unit, moveEvent.getTargetTowerLocation(), 0.05, Unit.class)) {
                        unit.setMoving(false);
                        unit.setKeepingTowerId(moveEvent.getTargetTowerId());
                        moveEventDao.delete(moveEvent);
                    }
                    gameObjectDao.save(unit);
                } catch (NotValidPathException e) {
                    e.printStackTrace();
                    //TODO: maybe we should delete this moveEvent
                }
            } catch (Exception ex) {
                ex.printStackTrace();
                unit.setMoving(false);
                gameObjectDao.save(unit);
                moveEventDao.delete(moveEvent);
            }
        }

    }
}
