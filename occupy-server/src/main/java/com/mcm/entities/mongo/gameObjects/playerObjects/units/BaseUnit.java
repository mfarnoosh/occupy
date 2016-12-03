package com.mcm.entities.mongo.gameObjects.playerObjects.units;

import com.mcm.entities.World;
import com.mcm.entities.mongo.GameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;

/**
 * Created by Mehrdad on 16/12/04.
 */
public abstract class BaseUnit extends BasePlayerObject {
    private double velocity = 0;
    private boolean isMoving = false;

    //Constructors

    //End Constructors

    //Abstract Methods

    //End Abstract Methods

    //Method Accessors
    public double getVelocity() {
        return velocity;
    }

    public boolean isMoving() {
        return isMoving;
    }
    //End Method Accessors

    //Functions
    public double whenItWillGetTo(double lat, double lon) {
        return World.whenItWillGetTo(this, lat, lon);
    }
    public boolean canGetTo(double lat, double lon) {
        return World.canGetTo(this, lat, lon);
    }

    public double whenItWillGetTo(GameObject otherGameObject) {
        if (otherGameObject.getLocation() == null || otherGameObject.getLocation().length != 2)
            return -1;
        return whenItWillGetTo(otherGameObject.getLocation()[0], otherGameObject.getLocation()[1]);
    }
    //End Functions
}
