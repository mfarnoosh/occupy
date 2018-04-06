package com.mcm.entities.mongo.gameObjects.playerObjects;

import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;

/**
 * Created by Mehrdad on 16/12/04.
 */
public abstract class BasePlayerObject extends BaseGameObject {
    protected double currentHitPoint = 0;
    protected boolean isAttacking = false;
    protected boolean isUpgrading = false;
    protected String playerId;

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

    public String getPlayerId() { return playerId; }

    public void setPlayer(Player player) { this.playerId = player.getId(); }
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
