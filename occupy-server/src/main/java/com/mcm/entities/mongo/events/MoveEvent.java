package com.mcm.entities.mongo.events;

import com.mcm.entities.Path;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class MoveEvent extends BaseEvent {
    private String targetTowerId;
    private double[] targetTowerLocation; //move destination
    private Path path;

    public Path getPath() {
        return path;
    }

    public void setPath(Path path) {
        this.path = path;
    }

    public String getTargetTowerId() {
        return targetTowerId;
    }

    public void setTargetTowerId(String targetTowerId) {
        this.targetTowerId = targetTowerId;
    }

    public double[] getTargetTowerLocation() {
        return targetTowerLocation;
    }

    public void setTargetTowerLocation(double[] targetTowerLocation) {
        this.targetTowerLocation = targetTowerLocation;
    }
}
