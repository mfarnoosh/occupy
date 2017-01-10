package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;

import java.util.LinkedHashSet;
import java.util.List;

/**
 * Created by alirezaghias on 10/19/2016 AD.
 */

public interface IGameObjectDao extends IBaseMongoDao<BaseGameObject> {
    List<Tower> getAllTowersInBox(double[] lowerLeft, double[] upperRight);
    Tower findTowerById(String id);

    boolean isInRangeEachOther(BasePlayerObject object1, BasePlayerObject object2);

    boolean isArrived(BasePlayerObject playerObject, double[] destination, double maxRadiusInKilometer);

    LinkedHashSet<Unit> getAllUnitsInTowerRange(Tower tower);

    LinkedHashSet<Tower> getAllTowersInUnitRange(Unit unit);

    Tower getNearestTowerInUnitRange(Unit unit);
}
