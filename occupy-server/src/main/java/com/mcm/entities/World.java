package com.mcm.entities;

import com.graphhopper.GHRequest;
import com.graphhopper.GHResponse;
import com.graphhopper.GraphHopper;
import com.graphhopper.PathWrapper;
import com.graphhopper.api.GraphHopperWeb;
import com.graphhopper.util.PointList;
import com.graphhopper.util.shapes.GHPoint;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.util.GeoUtil;
import com.mcm.util.Spring;

import java.util.LinkedHashSet;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class World {
    public static boolean canGetTo(double lat1, double lon1, double lat2, double lon2) {
        return false;
    }
    private static GraphHopperWeb graphHopper = new GraphHopperWeb();
    static {
        graphHopper.setKey("64de687c-8f2e-4f54-b613-532e4b73316e");
    }
    public static boolean canGetTo(BaseGameObject object, double lat, double lon) {
        if (object.getLocation() == null || object.getLocation().length != 2)
            return false;
        return canGetTo(object.getLocation()[0], object.getLocation()[1], lat, lon);
    }

    /**
     * @param unit
     * @param lat
     * @param lon
     * @return time in ms if t = -1 it means ther is no route to specified lat and lon
     */
    public static double getArriveTime(Unit unit, double lat, double lon) {
        if (unit.getSpeed() == 0)
            return -1;

        Path path = nearestPath(unit.getLocation()[0], unit.getLocation()[1], lat, lon);
        double t = 0;
        for (Line line : path.lines) {
            if (line.getStart() != null && line.getStart().length == 2 && line.getEnd() != null && line.getEnd().length == 2) {
                t += GeoUtil.distance(line.getStart()[0], line.getStart()[1], line.getEnd()[0], line.getEnd()[1], "K") / unit.getSpeed();
            } else {
                t = -1;
                break;
            }
        }
        if (t != -1)
            t *= 3.6e+6;    // convert hour to ms
        return t;
    }
    public static Path findPath(double lat1, double lon1, double lat2, double lon2) {
        GHRequest request = new GHRequest().addPoint(new GHPoint(lat1, lon1))
                .addPoint(new GHPoint(lat2, lon2)).setVehicle("car");
        GHResponse response = graphHopper.route(request);
        if (response.hasErrors()) {
            return null;
        }
        PathWrapper res = response.getBest();
// get path geometry information (latitude, longitude and optionally elevation)
        PointList pl = res.getPoints();
// distance of the full path, in meter
        double distance = res.getDistance();
// time of the full path, in milliseconds
        long millis = res.getTime();
        LinkedList<Line> lines = new LinkedList<>();
        List<Double[]> points = res.getPoints().toGeoJson();
        double[] pre = new double[]{lat1, lon1};
        for (Double[] point: points) {
            double[] newP = new double[]{point[1], point[0]};
            lines.add(new Line(pre, newP));
            pre = newP;
        }
        double[] post = new double[]{lat2, lon2};
        lines.add(new Line(pre, post));
        Path path = new Path();
        path.lines = lines;
        return path;
    }
    public static Path nearestPath(double lat1, double lon1, double lat2, double lon2) {
        return new Path();
    }

    public static LinkedHashSet<Unit> getAllUnitsNearTower(Tower tower) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean(IGameObjectDao.class);
        return gameObjectDao.getAllUnitsInTowerRange(tower);
    }

    public static Tower getNearestTowerInUnitRange(Unit unit) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean(IGameObjectDao.class);
        return gameObjectDao.getNearestTowerInUnitRange(unit);
    }

    public static List<Unit> findUnitByOwnerTower(Tower tower) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean(IGameObjectDao.class);
        return gameObjectDao.findUnitByOwnerTower(tower);
    }

    public static Tower getNearestTowerInUnitRange(Unit unit, String exceptPlayerId) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean(IGameObjectDao.class);
        return gameObjectDao.getNearestTowerInUnitRange(unit, exceptPlayerId);
    }

    public static LinkedHashSet<Unit> getAllUnitsNearTower(Tower tower, String playerId) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean(IGameObjectDao.class);
        return gameObjectDao.getAllUnitsInTowerRange(tower, playerId);
    }

    public static Path findPath(double[] location, double[] location1) {
        return findPath(location[0], location[1], location1[0], location1[1]);
    }
}
