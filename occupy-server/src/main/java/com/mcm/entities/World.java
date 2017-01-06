package com.mcm.entities;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.util.GeoUtil;
import com.mcm.util.Spring;

import java.util.LinkedHashSet;

/**
 * Created by alirezaghias on 10/18/2016 AD.
 */
public class World {
    public static boolean canGetTo(double lat1, double lon1, double lat2, double lon2) {
        return false;
    }
    public static boolean canGetTo(BaseGameObject object, double lat, double lon) {
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
    public static double whenItWillGetTo(Unit unit, double lat, double lon) {
            if (unit.getVelocity() == 0)
                return -1;

        Path path = nearestPath(unit.getLocation()[0], unit.getLocation()[1], lat, lon);
        double t = 0;
        for (Line line: path.lines) {
            if (line.getStart() != null && line.getStart().length == 2 && line.getEnd() != null && line.getEnd().length == 2) {
                t += GeoUtil.distance(line.getStart()[0], line.getStart()[1], line.getEnd()[0], line.getEnd()[1], "K")/unit.getVelocity();
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

    public static LinkedHashSet<BaseGameObject> gameObjectsNear(BasePlayerObject playerObject) {
        IGameObjectDao gameObjectDao = (IGameObjectDao) Spring.context.getBean("IGameObjectDao");
        return gameObjectDao.gameObjectsNear(playerObject);
    }
}
