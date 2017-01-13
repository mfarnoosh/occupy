package com.mcm.entities.mongo.events;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class AttackEvent extends BaseEvent {
    private String underAttackTowerId;

    public String getUnderAttackTowerId() {
        return underAttackTowerId;
    }

    public void setUnderAttackTowerId(String underAttackTowerId) {
        this.underAttackTowerId = underAttackTowerId;
    }
}
