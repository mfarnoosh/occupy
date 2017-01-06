package com.mcm.entities.mongo.events;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class AttackEvent extends BaseEvent {
    private String underAttackObjectId;

    public String getUnderAttackObjectId() {
        return underAttackObjectId;
    }

    public void setUnderAttackObjectId(String underAttackObjectId) {
        this.underAttackObjectId = underAttackObjectId;
    }
}
