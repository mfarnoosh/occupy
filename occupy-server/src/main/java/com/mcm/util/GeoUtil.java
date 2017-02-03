package com.mcm.util;

import com.mcm.entities.Line;
import com.mcm.entities.Path;
import com.mcm.exceptions.NotValidPathException;
import org.springframework.data.geo.Point;

import javax.persistence.Tuple;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class GeoUtil {
    public static double[] latLonOf(double dist, Path path) throws NotValidPathException {
        double[] source = null;
        double[] dest = null;
        double tmp = dist;
        for (Line line: path.lines) {
            if (source == null)
                source = line.getStart();
            if (dest == null)
                dest = line.getEnd();
            if (tmp > line.getDistance())
                return line.getEnd();
            if (line.getDistance() >= tmp) {
                source = line.getStart();
                dest = line.getEnd();
                break;
            } else {
                tmp -= line.getDistance();
            }
        }
        if (source == null || dest == null) {
            throw new NotValidPathException();
        }

        double lat1 = deg2rad(source[0]);
        double lon1 = deg2rad(source[1]);

        double lat2 = deg2rad(dest[0]);
        double lon2 = deg2rad(dest[1]);
        double tc = Math.atan2(Math.sin(lon2-lon1) * Math.cos(lat2),
                Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon2 - lon1)) % (2 * Math.PI);

        return latLonOf(source, dist, tc);
    }
    public static double[] latLonOf(double[] latLon, double dist, double tc) {
        // see http://mathforum.org/library/drmath/view/51816.html for the math
        double lat = deg2rad(latLon[0]);
        double lon = deg2rad(latLon[1]);
        double r = 6371; // radius of earth in km
        double d = dist / r; // convert dist to arc radians


        double resultLat, resultLon;
        resultLat = Math.asin(Math.sin(lat) * Math.cos(d) +
                Math.cos(lat) * Math.sin(d) * Math.cos(tc));
        double dlon = Math.atan2(Math.sin(tc) * Math.sin(d) *
                Math.cos(lat), Math.cos(d) - Math.sin(lat) * Math.sin(lat));
        resultLon = ((lon + dlon + Math.PI) % (2 * Math.PI)) - Math.PI;

        resultLat = (resultLat * 180) / Math.PI; // back to degrees
        resultLon = (resultLon * 180) / Math.PI;
        return new double[]{resultLat, resultLon};
    }

    /**
     * @param lat1
     * @param lon1
     * @param lat2
     * @param lon2
     * @param unit K -> Kilometer , N -> Natural mile, default -> Mile
     * @return
     */
    public static double distance(double lat1, double lon1, double lat2, double lon2, String unit) {
        double theta = lon1 - lon2;
        double dist = Math.sin(deg2rad(lat1)) * Math.sin(deg2rad(lat2)) + Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) * Math.cos(deg2rad(theta));
        dist = Math.acos(dist);
        dist = rad2deg(dist);
        dist = dist * 60 * 1.1515;
        if (unit.equals("K")) {
            dist = dist * 1.609344;
        } else if (unit.equals("N")) {
            dist = dist * 0.8684;
        }

        return (dist);
    }

    /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
    /*::	This function converts decimal degrees to radians						 :*/
	/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
    public static double deg2rad(double deg) {
        return (deg * Math.PI / 180.0);
    }

    /*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
	/*::	This function converts radians to decimal degrees						 :*/
	/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/
    public static double rad2deg(double rad) {
        return (rad * 180 / Math.PI);
    }

    public static List<Integer> getTileNumber(final double lat, final double lon, final int zoom) {
        int xtile = (int) Math.floor((lon + 180) / 360 * (1 << zoom));
        int ytile = (int) Math.floor((1 - Math.log(Math.tan(Math.toRadians(lat)) + 1 / Math.cos(Math.toRadians(lat))) / Math.PI) / 2 * (1 << zoom));
        if (xtile < 0)
            xtile = 0;
        if (xtile >= (1 << zoom))
            xtile = ((1 << zoom) - 1);
        if (ytile < 0)
            ytile = 0;
        if (ytile >= (1 << zoom))
            ytile = ((1 << zoom) - 1);
        List<Integer> rez = new ArrayList<>();
        rez.add(xtile);
        rez.add(ytile);
        return rez;
    }

    public static String getTileNumberUrl(int xTile, int yTile, int zoom) {
        return ("" + zoom + "/" + xTile + "/" + yTile);
    }

    public static Point getTileCenter(int xTile, int yTile, int zoom) {
        BoundingBox box = tile2boundingBox(xTile, yTile, zoom);
        return new Point(box.north - Math.abs(box.north - box.south) / 2, box.west + Math.abs(box.east - box.west) / 2);
    }

    public static BoundingBox tile2boundingBox(final int x, final int y, final int zoom) {
        BoundingBox bb = new BoundingBox();
        bb.north = tile2lat(y, zoom);
        bb.south = tile2lat(y + 1, zoom);
        bb.west = tile2lon(x, zoom);
        bb.east = tile2lon(x + 1, zoom);
        return bb;
    }

    static double tile2lon(int x, int z) {
        return x / Math.pow(2.0, z) * 360.0 - 180;
    }

    static double tile2lat(int y, int z) {
        double n = Math.PI - (2.0 * Math.PI * y) / Math.pow(2.0, z);
        return Math.toDegrees(Math.atan(Math.sinh(n)));
    }

    public static class BoundingBox {
        public double north;
        public double south;
        public double east;
        public double west;
    }
}
