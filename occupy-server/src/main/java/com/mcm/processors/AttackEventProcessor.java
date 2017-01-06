package com.mcm.processors;

import com.mcm.dao.mongo.AttackEventDao;
import com.mcm.dao.mongo.GameObjectDao;
import com.mcm.dao.mongo.interfaces.IAttackEventDao;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.events.AttackEvent;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.util.Spring;
import org.springframework.beans.factory.annotation.Autowired;

import java.util.List;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class AttackEventProcessor extends EventProcessor<AttackEvent> {
    private IGameObjectDao gameObjectDao = Spring.context.getBean(IGameObjectDao.class);
    private IAttackEventDao attackEventDao = Spring.context.getBean(IAttackEventDao.class);
    public AttackEventProcessor(List<AttackEvent> batch) {
        super(batch);
    }

    @Override
    void doJob(List<AttackEvent> batch) {
        for (AttackEvent attackEvent: batch) {
            Unit unit = (Unit) gameObjectDao.findById(attackEvent.getGameObjectId());
            Tower tower = gameObjectDao.findTowerById(attackEvent.getUnderAttackObjectId());
            if (gameObjectDao.isInRangeEachOther(tower, unit)) {
                unit.attack();
                tower.attack();
                gameObjectDao.save(unit);
                gameObjectDao.save(tower);
                if (unit.getHealth() <= 0) {
                    gameObjectDao.delete(unit);
                    attackEventDao.delete(attackEvent);
                }
                if (tower.getHealth() <= 0) {
                    gameObjectDao.delete(tower);
                    attackEventDao.delete(attackEvent);
                }
            }

        }
    }
}
