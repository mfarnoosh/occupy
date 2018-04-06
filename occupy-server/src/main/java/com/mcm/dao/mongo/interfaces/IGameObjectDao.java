package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;

/**
 * Created by alirezaghias on 10/19/2016 AD.
 */

public interface IGameObjectDao extends IBaseMongoDao<BaseGameObject> {
    List<Tower> getAllTowersInBox(double[] lowerLeft, double[] upperRight);
    List<Unit> getAllUnitsInBox(double[] lowerLeft, double[] upperRight);

    Tower findTowerById(String id);
    Unit findUnitById(String unitId);

    boolean isInRangeEachOther(Tower tower, Unit unit);
    boolean isArrived(BasePlayerObject playerObject, double[] destination, double maxRadiusInKilometer, Class clazz);

    LinkedHashSet<Unit> getAllUnitsInTowerRange(Tower tower);
    LinkedHashSet<Tower> getAllTowersInUnitRange(Unit unit);

    Tower getNearestTowerInUnitRange(Unit unit);

    List<Tower> getPlayerTowers(String playerId);
    int getPlayerTowersCount(String playerId);


    List<Unit> findUnitByOwnerTower(Tower tower);

    void saveAllTowers(Collection<Tower> objects);
    void saveAllUnits(Collection<Unit> objects);

    Tower getNearestTowerInUnitRange(Unit unit, String exceptPlayerId);

    LinkedHashSet<Unit> getAllUnitsInTowerRange(Tower tower, String playerId);

    void saveAll(Collection<BasePlayerObject> objects);
}
