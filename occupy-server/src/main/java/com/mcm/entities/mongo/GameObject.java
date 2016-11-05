package com.mcm.entities.mongo;

import com.mcm.entities.World;
import com.mcm.util.GeoUtil;
import org.springframework.data.mongodb.core.index.GeoSpatialIndexed;

import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.Set;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public abstract class GameObject extends BaseDocument{
    enum Type {
        Building, Tower, Unit
    }
    private Type type;
    private double health = 100.0;
    private double power = 0.0;
    private double powerFactor = 0.0;
    private double defenceFactor = 0.0;
    private double level = 0.0;
    @GeoSpatialIndexed
    private double[] location;
    /**
     * in K/H
     */
    private double velocity = 0;
    private boolean isMovable = false;
    private boolean isMoving = false;
    private boolean isInWar = false;
    private boolean isRangeAttack = false;
    private boolean needAttention = false;
    private double range = 0;

    public double getRange() {
        return range;
    }

    public boolean isRangeAttack() {
        return isRangeAttack;
    }

    public boolean isMovable() {
        return isMovable;
    }

    public void setLocation(double[] location) {
        this.location = location;
    }

    public double[] getLocation() {
        return location;
    }

    public double getVelocity() {
        return velocity;
    }

    public boolean isMoving() {
        return isMoving;
    }

    public boolean isInWar() {
        return isInWar;
    }

    public boolean isNeedAttention() {
        return needAttention;
    }

    protected GameObject() {

    }
    public GameObject(Type type) {
        this.type = type;
    }

    public Type getType() {
        return type;
    }



    public Double getHealth() {
        return health;
    }



    public Double getPower() {
        return power;
    }



    public Double getPowerFactor() {
        return powerFactor;
    }



    public Double getDefenceFactor() {
        return defenceFactor;
    }



    public Double getLevel() {
        return level;
    }

    public void attack() {
        LinkedHashSet<GameObject> others = World.gameObjectsNear(this);
        if (isRangeAttack) {
            for (GameObject other : others) {
                attack(other);
            }
        } else {
            Iterator<GameObject> iterator = others.iterator();
            if (iterator.hasNext()) {
                attack(iterator.next());
            }
        }
    }

    protected void attack(GameObject otherGameObject) {
        double hitPower = power * powerFactor * (1 - otherGameObject.defenceFactor);
        otherGameObject.decreaseHealth(hitPower);
        increaseLevel(getExperience(hitPower));
    }
    abstract double getExperience(double hitPower);
    abstract void levelIncreasedBy(double diff);
    abstract void healthIncreasedBy(double diff);
    abstract void levelDecreasedBy(double diff);
    abstract void healthDecreasedBy(double diff);
    public void increaseLevel(double level) {
        if (level > 0) {
            double pre = this.level;
            this.level += level;
            levelIncreasedBy(this.level - pre);
        }
    }
    public void decreaseLevel(double level) {
        if (level > 0) {
            double pre = this.level;
            this.level -= level;
            levelDecreasedBy(pre - this.level);
        }
    }
    public void increaseHealth(double health) {
        if (health > 0) {
            double pre = this.health;
            this.health += health;
            healthIncreasedBy(this.health - pre);
        }
    }
    public void decreaseHealth(double health) {
        if (health > 0) {
            double pre = this.health;
            this.health -= health;
            healthDecreasedBy(pre - this.health);
        }
    }
    public double distanceTo(double lat, double lon) {
        if (location == null || location.length != 2)
            return -1;
        return GeoUtil.distance(location[0], location[1], lat, lon, "K");
    }
    public double distanceTo(GameObject otherGameObject) {
        if (otherGameObject.getLocation() == null || otherGameObject.getLocation().length != 2)
            return -1;
        return distanceTo(otherGameObject.location[0], otherGameObject.location[1]);
    }
    public boolean canGetTo(double lat, double lon) {
        return World.canGetTo(this, lat, lon);
    }
    public double whenItWillGetTo(double lat, double lon) {
        return World.whenItWillGetTo(this, lat, lon);
    }
    public double whenItWillGetTo(GameObject otherGameObject) {
        if (otherGameObject.getLocation() == null || otherGameObject.getLocation().length != 2)
            return -1;
        return whenItWillGetTo(otherGameObject.location[0], otherGameObject.location[1]);
    }


}
