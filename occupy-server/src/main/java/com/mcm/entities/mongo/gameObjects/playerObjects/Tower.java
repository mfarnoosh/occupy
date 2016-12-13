package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.mongo.Player;
import com.mcm.enums.TowerType;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Tower extends BasePlayerObject {
    private TowerType type;

    public Tower(){}
    public Tower(TowerType type, Player player, double[] location) {
        setType(type);
        setLocation(location);
        this.player = player;

        setMechanicValues(type);
    }
    public Tower(int typeValue, Player player, double[] location) {
        this(TowerType.valueOf(typeValue),player,location);
    }

    private void setMechanicValues(TowerType type) {
        switch (type) {
            case SENTRY:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case MACHINE_GUN:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case ROCKET_LAUNCHER:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case ANTI_AIRCRAFT:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case STEALTH:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
        }
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

    public TowerType getType() {
        return type;
    }

    public void setType(TowerType type) {
        this.type = type;
    }
}
