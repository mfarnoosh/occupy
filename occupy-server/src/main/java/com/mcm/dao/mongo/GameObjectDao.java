package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.geo.*;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

import java.util.Collection;
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


    public boolean isArrived(BasePlayerObject playerObject, double[] destination, double maxRadiusInKilometer, Class clazz) {
        final LinkedHashSet<BaseGameObject> res = new LinkedHashSet<>();
        if (playerObject.getLocation() == null || playerObject.getLocation().length != 2) {
            return false;
        }
        Query query = new Query();
        query.addCriteria(Criteria.where("location")
                .within(new Circle(new Point(destination[0], destination[1])
                        , new Distance(maxRadiusInKilometer, Metrics.KILOMETERS))));
        final List<BaseGameObject> results = getMongoOperations().find(query, clazz);
        for (BaseGameObject gameObject: results) {
            res.add(gameObject);
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
        Query query = new Query();
        query.addCriteria(Criteria.where("location")
                .within(new Circle(new Point(tower.getLocation()[0], tower.getLocation()[1])
                        , new Distance(tower.getRange(), Metrics.KILOMETERS))));
        final List<Unit> results = getMongoOperations().find(query, Unit.class);
        for (Unit unit : results) {
            res.add(unit);
        }
        return res;
    }
    public LinkedHashSet<Tower> getAllTowersInUnitRange(Unit unit){
        final LinkedHashSet<Tower> res = new LinkedHashSet<>();
        if (unit.getLocation() == null || unit.getLocation().length != 2) {
            return res;
        }
        Query query = new Query();
        query.addCriteria(Criteria.where("location")
                .within(new Circle(new Point(unit.getLocation()[0], unit.getLocation()[1])
                        , new Distance(unit.getRange(), Metrics.KILOMETERS))));
        final List<Tower> results = getMongoOperations().find(query, Tower.class);
        for (Tower tower : results) {
            res.add(tower);
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

    @Override
    public void saveAllTowers(Collection<Tower> towers) {
        for (Tower tower: towers) {
            save(tower);
        }
    }
    @Override
    public void saveAllUnits(Collection<Unit> units) {
        for (Unit unit: units) {
            save(unit);
        }
    }

}
