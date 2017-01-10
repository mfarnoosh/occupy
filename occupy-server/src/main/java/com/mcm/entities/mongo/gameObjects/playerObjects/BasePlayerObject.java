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
    protected double currentHitPoint = 0;
    protected boolean isAttacking = false;
    protected boolean isUpgrading = false;
    protected Player player;

    //Constructors

    //End Constructors

    //Abstract Methods

    /**
     * upgrade this object to higher level(if available)
     * decrease the upgrade price which saved in game-config file from user
     * and put the object in upgrade list until upgrade will finished
     */
    public abstract void upgrade();
    //End Abstract Methods


    //region Method Accessors
    public double getCurrentHitPoint() {return currentHitPoint;}

    public void setCurrentHitPoint(double currentHitPoint) {this.currentHitPoint = currentHitPoint;}

    public boolean isAttacking() { return isAttacking; }

    public void setAttacking(boolean attacking) { isAttacking = attacking; }

    public boolean isUpgrading() { return isUpgrading; }

    public void setUpgrading(boolean upgrading) { isUpgrading = upgrading; }
    public Player getPlayer() { return player; }

    public void setPlayer(Player player) { this.player = player; }
    //end region

    //Functions

    /**
     * move object to new location
     * @param newLocation
     */
    public void move(double[] newLocation) {
        setLocation(newLocation);
    }
    //End Functions
}
