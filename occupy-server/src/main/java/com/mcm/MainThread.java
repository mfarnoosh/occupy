package com.mcm;

import com.mcm.dao.mongo.GameObjectDao;
import com.mcm.dao.mongo.interfaces.IAttackEventDao;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.mongo.events.AttackEvent;
import com.mcm.entities.mongo.events.MoveEvent;
import com.mcm.processors.AttackEventProcessor;
import com.mcm.processors.MoveEventProcessor;
import org.apache.commons.collections4.ListUtils;
import org.apache.commons.lang.SystemUtils;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.Arrays;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
@Component
public class MainThread extends Thread {
    final ExecutorService executorService = Executors.newCachedThreadPool();
    @Autowired
    protected IGameObjectDao gameObjectDao;
    @Autowired
    protected IMoveEventDao moveEventDao;
    @Autowired
    protected IAttackEventDao attackEventDao;
    final private int batchSize = 20;
    private final static Logger logger = Logger.getLogger(MainThread.class);

    @Override
    public void run() {
        long cycle = 0;
        while (!interrupted()) {
            long time  = System.currentTimeMillis();
            cycle ++;
            logger.info("--- cycle = " + cycle + " time -> " + time + "---");
            try {
                // processing move events
                final List<MoveEvent> moveEvents = moveEventDao.findAll();
                executorService.invokeAll(ListUtils.partition(moveEvents, batchSize)
                        .stream().map(MoveEventProcessor::new).collect(Collectors.toList()));
                long moveEventTime = System.currentTimeMillis();
                logger.info("--- move events (" + moveEvents.size() + ") processed in -> " + (moveEventTime - time) + "ms ---");
                // processing attack events
                final List<AttackEvent> attackEvents = attackEventDao.findAll();
                executorService.invokeAll(ListUtils.partition(attackEvents, batchSize)
                        .stream().map(AttackEventProcessor::new).collect(Collectors.toList()));
                long attackEventTime = System.currentTimeMillis();
                logger.info("--- attack events (" + attackEvents.size() + ") processed in -> " + (attackEventTime - moveEventTime) + "ms ---");
                // sleep to have a 1 second cycle
                long elapsed = attackEventTime - time;
                long waitTime = 1000 - elapsed;
                if (waitTime > 0) {
                    Thread.sleep(waitTime);
                }
                logger.info("--- elapsed " + elapsed + "ms ---");
                logger.info("--- waiting " + waitTime + "ms ---");
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }


    }
}
