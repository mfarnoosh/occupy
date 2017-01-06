package com.mcm.entities;

import com.mcm.util.GeoUtil;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class Line {
    private double[] start;
    private double[] end;
    private double distance;

    public Line() {
    }

    public Line(double[] start, double[] end) {
        this.start = start;
        this.end = end;
        this.distance = GeoUtil.distance(start[0], start[1], end[0], end[1], "K");
    }

    public void setStart(double[] start) {
        this.start = start;
    }

    public void setEnd(double[] end) {
        this.end = end;
    }

    public void setDistance(double distance) {
        this.distance = distance;
    }

    public double[] getStart() {
        return start;
    }

    public double[] getEnd() {
        return end;
    }

    public double getDistance() {
        return distance;
    }
}
