package com.mcm.processors;

import com.mcm.entities.mongo.events.BaseEvent;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.concurrent.Callable;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */

abstract class EventProcessor<T extends BaseEvent> implements Callable<Void> {
    private List<T> batch;
    EventProcessor(List<T> batch) {
        this.batch = batch;
    }
    abstract void doJob(List<T> batch);

    @Override
    public Void call() throws Exception {
        doJob(batch);
        return null;
    }

}
