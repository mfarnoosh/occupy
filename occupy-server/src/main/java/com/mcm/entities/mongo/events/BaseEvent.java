package com.mcm.entities.mongo.events;

import com.mcm.entities.mongo.BaseDocument;

import java.util.Date;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
abstract public class BaseEvent extends BaseDocument {
    private String gameObjectId;

    public String getGameObjectId() {
        return gameObjectId;
    }

    public void setGameObjectId(String gameObjectId) {
        this.gameObjectId = gameObjectId;
    }
}
