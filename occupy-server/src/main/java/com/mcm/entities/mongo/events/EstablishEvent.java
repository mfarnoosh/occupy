package com.mcm.entities.mongo.events;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class EstablishEvent extends BaseEvent {
    private String targetTowerId;

    public String getTargetTowerId() {
        return targetTowerId;
    }

    public void setTargetTowerId(String targetTowerId) {
        this.targetTowerId = targetTowerId;
    }
}
