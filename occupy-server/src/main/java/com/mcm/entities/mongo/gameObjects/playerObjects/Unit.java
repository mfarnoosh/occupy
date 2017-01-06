package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.World;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.enums.UnitType;

/**
 * Created by Mehrdad on 16/12/04.
 */
public class Unit extends BasePlayerObject {
    private UnitType type;
    private double velocity = 0;
    private boolean isMoving = false;

    public Unit(UnitType type, Player player) {
        setType(type);
        this.player = player;

        setMechanicValues(type);
    }
    public Unit(int typeValue, Player player) {
        this(UnitType.valueOf(typeValue),player);
    }

    private void setMechanicValues(UnitType type) {
        switch (type) {
            case SOLDIER:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case MACHINE:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case TANK:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case HELICOPTER:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case AIRCRAFT:
                power = 1;
                powerFactor = 1;
                isInWar = false;
                isRangeAttack = false;
                range = 10;

                health = 100.0;
                defenceFactor = 1.0;
                level = 1.0;
                break;
            case TITAN:
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

    //Constructors

    //End Constructors

    //Abstract Methods
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
    //End Abstract Methods

    //Method Accessors
    public double getVelocity() {
        return velocity;
    }

    public boolean isMoving() {
        return isMoving;
    }

    public void setVelocity(double velocity) {
        this.velocity = velocity;
    }

    public void setMoving(boolean moving) {
        isMoving = moving;
    }

    public UnitType getType() {
        return type;
    }

    public void setType(UnitType type) {
        this.type = type;
    }
    //End Method Accessors

    //Functions
    public double whenItWillGetTo(double lat, double lon) {
        return World.whenItWillGetTo(this, lat, lon);
    }

    public boolean canGetTo(double lat, double lon) {
        return World.canGetTo(this, lat, lon);
    }

    public double whenItWillGetTo(BaseGameObject otherGameObject) {
        if (otherGameObject.getLocation() == null || otherGameObject.getLocation().length != 2)
            return -1;
        return whenItWillGetTo(otherGameObject.getLocation()[0], otherGameObject.getLocation()[1]);
    }

    //End Functions
}
