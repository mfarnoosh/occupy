package com.mcm.entities.mongo.gameObjects.buildings;

import com.mcm.entities.mongo.gameObjects.BaseBuilding;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Park extends BaseBuilding {
    @Override
    protected double getExperience(double hitPower) {
        return 0;
    }

    @Override
    protected void levelIncreasedBy(double diff) {

    }

    @Override
    protected void healthIncreasedBy(double diff) {

    }

    @Override
    protected void levelDecreasedBy(double diff) {

    }

    @Override
    protected void healthDecreasedBy(double diff) {

    }
}
