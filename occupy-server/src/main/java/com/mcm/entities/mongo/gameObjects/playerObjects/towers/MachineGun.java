package com.mcm.entities.mongo.gameObjects.playerObjects.towers;

import com.mcm.entities.mongo.Player;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class MachineGun extends BaseTower {
    public MachineGun(Player player, double[] location){
        power = 1;
        powerFactor = 1;
        isInWar = false;
        isRangeAttack = false;
        range = 10;

        health = 100.0;
        defenceFactor = 1.0;
        level = 1.0;

        setLocation(location);
        this.player = player;
    }
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
