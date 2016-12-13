package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.World;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.Player;

import java.util.Iterator;
import java.util.LinkedHashSet;

/**
 * Created by Mehrdad on 16/12/04.
 */
public abstract class BasePlayerObject extends BaseGameObject {
    protected double power = 0.0;
    protected double powerFactor = 0.0;
    protected boolean isInWar = false;
    protected boolean isRangeAttack = false;
    protected boolean needAttention = false;
    protected double range = 0;
    protected Player player;

    //Constructors

    //End Constructors

    //Abstract Methods

    //End Abstract Methods


    //Method Accessors
    public Double getPower() { return power; }

    public Double getPowerFactor() { return powerFactor; }

    public boolean isInWar() { return isInWar; }

    public boolean isNeedAttention() { return needAttention; }

    public boolean isRangeAttack() { return isRangeAttack; }

    public double getRange() { return range; }


    public Player getPlayer() { return player; }
    //End Method Accessors

    //Functions
    public void attack() {
        LinkedHashSet<BaseGameObject> others = World.gameObjectsNear(this);
        if (isRangeAttack) {
            for (BaseGameObject other : others) {
                attack(other);
            }
        } else {
            Iterator<BaseGameObject> iterator = others.iterator();
            if (iterator.hasNext()) {
                attack(iterator.next());
            }
        }
    }


    protected void attack(BaseGameObject otherGameObject) {
        double hitPower = power * powerFactor * (1 - otherGameObject.getDefenceFactor());
        otherGameObject.decreaseHealth(hitPower);
        increaseLevel(getExperience(hitPower));
    }
    //End Functions
}
