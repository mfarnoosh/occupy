package com.mcm.entities;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.GameObject;
import com.mcm.entities.mongo.gameObjects.BaseBuilding;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.towers.BaseTower;
import com.mcm.entities.mongo.gameObjects.playerObjects.units.BaseUnit;
import com.mcm.util.GeoUtil;
import com.mcm.util.Spring;

import java.util.LinkedHashSet;
import java.util.Set;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class World {
    public static boolean canGetTo(double lat1, double lon1, double lat2, double lon2) {
        return false;
    }
    public static boolean canGetTo(GameObject object, double lat, double lon) {
        if (object.getLocation() == null || object.getLocation().length != 2)
            return false;
        return canGetTo(object.getLocation()[0], object.getLocation()[1], lat, lon);
    }

    /**
     *
     * @param unit
     * @param lat
     * @param lon
     * @return time in ms if t = -1 it means ther is no route to specified lat and lon
     */
    public static double whenItWillGetTo(BaseUnit unit, double lat, double lon) {
            if (unit.getVelocity() == 0)
                return -1;

        Path path = nearestPath(unit.getLocation()[0], unit.getLocation()[1], lat, lon);
        double t = 0;
        for (Line line: path.lines) {
            if (line.start != null && line.start.length == 2 && line.end != null && line.end.length == 2) {
                t += GeoUtil.distance(line.start[0], line.start[1], line.end[0], line.end[1], "K")/unit.getVelocity();
            } else {
                t = -1;
                break;
            }
        }
        if (t != -1)
            t *= 3.6e+6;    // convert hour to ms
        return t;
    }

    public static Path nearestPath(double lat1, double lon1, double lat2, double lon2) {
        return new Path();
    }

    public static LinkedHashSet<GameObject> gameObjectsNear(BasePlayerObject playerObject) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean("IGameObjectDao");
        return gameObjectDao.gameObjectsNear(playerObject);
    }
}
