package com.mcm.util;

import org.springframework.data.geo.Point;

import javax.persistence.Tuple;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class GeoUtil {
    /**
     *
     * @param lat1
     * @param lon1
     * @param lat2
     * @param lon2
     * @param unit  K -> Kilometer , N -> Natural mile, default -> Mile
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
        int xtile = (int)Math.floor( (lon + 180) / 360 * (1<<zoom) ) ;
        int ytile = (int)Math.floor( (1 - Math.log(Math.tan(Math.toRadians(lat)) + 1 / Math.cos(Math.toRadians(lat))) / Math.PI) / 2 * (1<<zoom) ) ;
        if (xtile < 0)
            xtile=0;
        if (xtile >= (1<<zoom))
            xtile=((1<<zoom)-1);
        if (ytile < 0)
            ytile=0;
        if (ytile >= (1<<zoom))
            ytile=((1<<zoom)-1);
        List<Integer> rez = new ArrayList<>();
        rez.add(xtile);
        rez.add(ytile);
        return rez;
    }
    public static String getTileNumberUrl(int xTile,int yTile,int zoom){
        return("" + zoom + "/" + xTile + "/" + yTile);
    }
    public static Point getTileCenter(int xTile,int yTile , int zoom){
        double lon = xTile / Math.pow(2.0, zoom) * 360.0 - 180;
        double n = Math.PI - (2.0 * Math.PI * yTile) / Math.pow(2.0, zoom);
        double lat = Math.toDegrees(Math.atan(Math.sinh(n)));

        return new Point(lat,lon);
    }
}
