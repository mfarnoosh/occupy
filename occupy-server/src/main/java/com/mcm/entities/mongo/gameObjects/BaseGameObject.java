package com.mcm.entities.mongo.gameObjects;

import com.mcm.entities.mongo.BaseDocument;
import com.mcm.util.GeoUtil;
import org.springframework.data.mongodb.core.index.GeoSpatialIndexed;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public abstract class BaseGameObject extends BaseDocument {
    protected int level = 1;
    @GeoSpatialIndexed
    private double[] location;

    //Constructors
    protected BaseGameObject() { }
    //End Constructors

    public double[] getLocation() { return location; }

    //Method Accessors
    public void setLocation(double[] location) { this.location = location; }

    public Integer getLevel() { return level; }

    public void setLevel(int level) { this.level = level;}

    //End Method Accessors

    //Abstracts
    //End Abstracts

    //Functions
    //End Functions

    public double distanceTo(double lat, double lon) {
        if (location == null || location.length != 2)
            return -1;
        return GeoUtil.distance(location[0], location[1], lat, lon, "K");
    }

    public double distanceTo(BaseGameObject otherGameObject) {
        if (otherGameObject.getLocation() == null || otherGameObject.getLocation().length != 2)
            return -1;
        return distanceTo(otherGameObject.location[0], otherGameObject.location[1]);
    }


}
