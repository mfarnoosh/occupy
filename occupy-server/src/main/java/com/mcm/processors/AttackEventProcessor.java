package com.mcm.processors;

import com.mcm.dao.mongo.interfaces.IAttackEventDao;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.events.AttackEvent;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.util.Spring;
import org.apache.log4j.Logger;

import java.util.*;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class AttackEventProcessor extends EventProcessor<AttackEvent> {
    private IGameObjectDao gameObjectDao = Spring.context.getBean(IGameObjectDao.class);
    private IAttackEventDao attackEventDao = Spring.context.getBean(IAttackEventDao.class);
    public AttackEventProcessor(List<AttackEvent> batch) {
        super(batch);
    }
    private static Logger logger = Logger.getLogger(AttackEventProcessor.class);
    @Override
    void doJob(List<AttackEvent> batch) {
        final Set<String> attackedSet =  new LinkedHashSet<>(batch.size());
        for (AttackEvent attackEvent: batch) {
            Unit unit = (Unit) gameObjectDao.findUnitById(attackEvent.getGameObjectId());
            if (unit != null) {
                Tower tower = gameObjectDao.findTowerById(attackEvent.getUnderAttackTowerId());
                if (tower != null) {
                    final String attackId = unit.getId() + tower.getId();
                    if (!attackedSet.contains(attackId)) {
                        attackedSet.add(attackId);
                        if (gameObjectDao.isInRangeEachOther(tower, unit)) {
                            gameObjectDao.saveAllTowers(unit.attack());
                            Collection<Unit> underAttackUnits = tower.attack();
                            gameObjectDao.saveAllUnits(underAttackUnits);


                            boolean finishAttack = false;
                            Iterator<Unit> iterator = underAttackUnits.iterator();
                            int oldSize = underAttackUnits.size();
                            while (iterator.hasNext()) {
                                Unit u = iterator.next();
                                if (u.getCurrentHitPoint() <= 0) {
                                    gameObjectDao.delete(u);
                                    iterator.remove();
                                }
                            }
                            int newSize = underAttackUnits.size();
                            if (oldSize != 0 && newSize == 0) {
                                finishAttack = true;
                            }
                            if (tower.getCurrentHitPoint() <= 0) {
                                finishAttack = true;
                            }
                            if (finishAttack) {
                                unit.setAttacking(false);
                                gameObjectDao.save(unit);
                                attackEventDao.delete(attackEvent);
                            }
                        }
                    } else {
                        logger.error("multiple attack event in a batch from a unit to a tower is not allowed");
                        attackEventDao.delete(attackEvent);
                    }
                } else {
                    logger.error("tower = " + attackEvent.getUnderAttackTowerId() + " is null deleting event");
                    attackEventDao.delete(attackEvent);
                }
            } else {
                logger.error("unit = " + attackEvent.getGameObjectId() + " is null deleting event");
                attackEventDao.delete(attackEvent);
            }

        }
    }
}
