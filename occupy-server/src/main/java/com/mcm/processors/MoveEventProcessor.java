package com.mcm.processors;

import com.mcm.dao.mongo.interfaces.IAttackEventDao;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.mongo.events.AttackEvent;
import com.mcm.entities.mongo.events.MoveEvent;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.exceptions.NotValidPathException;
import com.mcm.util.GeoUtil;
import com.mcm.util.Spring;
import org.apache.log4j.Logger;

import java.util.Date;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class MoveEventProcessor extends EventProcessor<MoveEvent> {
    private IGameObjectDao gameObjectDao = Spring.context.getBean(IGameObjectDao.class);
    private IMoveEventDao moveEventDao = Spring.context.getBean(IMoveEventDao.class);
    private IAttackEventDao attackEventDao = Spring.context.getBean(IAttackEventDao.class);
    public MoveEventProcessor(List<MoveEvent> batch) {
        super(batch);
    }
    private static Logger logger = Logger.getLogger(MoveEventProcessor.class);
    @Override
    void doJob(List<MoveEvent> batch) {
        final Set<String> movedSet =  new LinkedHashSet<>(batch.size());
        for (MoveEvent moveEvent : batch) {
            Unit unit = (Unit) gameObjectDao.findUnitById(moveEvent.getGameObjectId());
            if (unit != null) {
                final String moveId = unit.getId() + moveEvent.getTargetTowerId();
                if (!movedSet.contains(moveId)) {
                    movedSet.add(moveId);
                    long t = (new Date().getTime() - moveEvent.getCreated().getTime()) / 1000; //time in seccond
                    double v = unit.getSpeed();
                    double x = v * t; // distance in meter if velocity is m/s
                    double xInKilometer = x / 1000;
                    try {
                        double[] newLoc = GeoUtil.latLonOf(xInKilometer, moveEvent.getPath());
                        unit.setLocation(newLoc);
                        unit.setMoving(true);
                        final double distance = moveEvent.isAttackMode() ? unit.getRange() : 0.05;
                        if (gameObjectDao.isArrived(unit, moveEvent.getTargetTowerLocation(), distance, Unit.class)) {
                            unit.setMoving(false);
                            if (!moveEvent.isAttackMode()) {
                                unit.setKeepingTowerId(moveEvent.getTargetTowerId());
                            } else {
                                unit.setAttacking(true);
                                AttackEvent ae = new AttackEvent();
                                ae.setGameObjectId(unit.getId());
                                ae.setUnderAttackTowerId(moveEvent.getTargetTowerId());
                                attackEventDao.save(ae);
                            }
                            moveEventDao.delete(moveEvent);
                        }
                        gameObjectDao.save(unit);
                    } catch (NotValidPathException e) {
                        logger.error("not a valid path deleting event");
                        unit.setMoving(false);
                        gameObjectDao.save(unit);
                        moveEventDao.delete(moveEvent);
                    }
                } else {
                    logger.error("multiple move event in a batch from a unit to a tower is not allowed");
                    moveEventDao.delete(moveEvent);
                }
            } else {
                logger.error("unit = " + moveEvent.getGameObjectId() + " is null deleting event");
                moveEventDao.delete(moveEvent);
            }
        }

    }
}
