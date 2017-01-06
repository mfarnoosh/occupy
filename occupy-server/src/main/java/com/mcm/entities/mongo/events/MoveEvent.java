package com.mcm.entities.mongo.events;

import com.mcm.entities.Path;
import com.mcm.entities.mongo.BaseDocument;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
public class MoveEvent extends BaseEvent {
    private double[] destination;
    private Path path;

    public Path getPath() {
        return path;
    }

    public void setPath(Path path) {
        this.path = path;
    }

    public double[] getDestination() {
        return destination;
    }

    public void setDestination(double[] destination) {
        this.destination = destination;
    }
}
