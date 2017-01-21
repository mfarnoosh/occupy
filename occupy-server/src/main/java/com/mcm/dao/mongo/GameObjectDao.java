package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.geo.Box;
import org.springframework.data.geo.GeoResult;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.NearQuery;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashSet;
import java.util.List;

/**
 * Created by alirezaghias on 10/19/2016 AD.
 */
@Repository
public class GameObjectDao implements IGameObjectDao {
    @Autowired
    MongoOperations mongoTemplate;
    @Override
    public Class<BaseGameObject> getClazz() {
        return BaseGameObject.class;
    }

    @Override
    public MongoOperations getMongoOperations() {
        return mongoTemplate;
    }


    public boolean isArrived(BasePlayerObject playerObject, double[] destination, double maxRadiusInKilometer) {
        final LinkedHashSet<BaseGameObject> res = new LinkedHashSet<>();
        if (playerObject.getLocation() == null || playerObject.getLocation().length != 2) {
            return false;
        }
        final List<GeoResult<BaseGameObject>> results = getMongoOperations().geoNear(NearQuery.near(destination[0], destination[1])
                .inKilometers().maxDistance(maxRadiusInKilometer), BaseGameObject.class).getContent();
        for (GeoResult<BaseGameObject> geoResult : results) {
            res.add(geoResult.getContent());
        }
        return res.contains(playerObject);
    }

    public List<Tower> getAllTowersInBox(double[] lowerLeft,double[] upperRight){
        Query query = new Query();
        query.addCriteria(Criteria.where("location").within(new Box(lowerLeft,upperRight)));
        final List<Tower> results = getMongoOperations().find(query,Tower.class);
        return results;
    }
    public List<Unit> getAllUnitsInBox(double[] lowerLeft, double[] upperRight){
        Query query = new Query();
        query.addCriteria(Criteria.where("location").within(new Box(lowerLeft,upperRight)));
        final List<Unit> results = getMongoOperations().find(query,Unit.class);
        return results;
    }

    public Tower findTowerById(String id){
        Query query = new Query();
        query.addCriteria(Criteria.where("id").is(id));
        final Tower result = getMongoOperations().findOne(query,Tower.class);
        return result;
    }

    public Unit findUnitById(String unitId){
        Query query = new Query();
        query.addCriteria(Criteria.where("id").is(unitId));
        final Unit result = getMongoOperations().findOne(query,Unit.class);
        return result;
    }

    @Override
    public boolean isInRangeEachOther(BasePlayerObject object1, BasePlayerObject object2) {
        //TODO: Farnoosh
        return true;
    }

    public LinkedHashSet<Unit> getAllUnitsInTowerRange(Tower tower){
        final LinkedHashSet<Unit> res = new LinkedHashSet<>();
        if (tower.getLocation() == null || tower.getLocation().length != 2) {
            return res;
        }
        final List<GeoResult<Unit>> results =
                getMongoOperations().geoNear(NearQuery.near(tower.getLocation()[0], tower.getLocation()[1])
                .inKilometers().maxDistance(tower.getRange()), Unit.class).getContent();
        for (GeoResult<Unit> geoResult : results) {
            res.add(geoResult.getContent());
        }
        return res;
    }
    public LinkedHashSet<Tower> getAllTowersInUnitRange(Unit unit){
        final LinkedHashSet<Tower> res = new LinkedHashSet<>();
        if (unit.getLocation() == null || unit.getLocation().length != 2) {
            return res;
        }
        final List<GeoResult<Tower>> results =
                getMongoOperations().geoNear(NearQuery.near(unit.getLocation()[0], unit.getLocation()[1])
                        .inKilometers().maxDistance(unit.getRange()), Tower.class).getContent();
        for (GeoResult<Tower> geoResult : results) {
            res.add(geoResult.getContent());
        }
        return res;
    }

    public Tower getNearestTowerInUnitRange(Unit unit){
        return getAllTowersInUnitRange(unit).iterator().next();
    }

    public List<Tower> getPlayerTowers(String playerId){
        Query query = new Query();
        query.addCriteria(Criteria.where("playerId").is(playerId));
        final List<Tower> result = getMongoOperations().find(query,Tower.class);
        return result;
    }

    public int getPlayerTowersCount(String playerId){
        Query query = new Query();
        query.addCriteria(Criteria.where("playerId").is(playerId));
        long towerCount = getMongoOperations().count(query,Tower.class);
        return (int) towerCount;
    }
    public List<Unit> findUnitByOwnerTower(Tower tower){
        Query query = new Query();
        query.addCriteria(Criteria.where("keepingTowerId").is(tower.getId()));
        final List<Unit> result = getMongoOperations().find(query,Unit.class);
        return result;
    }

}
