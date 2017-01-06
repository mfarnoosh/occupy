package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;

import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.geo.Box;
import org.springframework.data.geo.GeoResult;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.CriteriaDefinition;
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



    public LinkedHashSet<BaseGameObject> gameObjectsNear(BasePlayerObject playerObject) {
        final LinkedHashSet<BaseGameObject> res = new LinkedHashSet<>();
        if (playerObject.getLocation() == null || playerObject.getLocation().length != 2) {
            return res;
        }
        final List<GeoResult<BaseGameObject>> results = getMongoOperations().geoNear(NearQuery.near(playerObject.getLocation()[0], playerObject.getLocation()[1])
                .inKilometers().maxDistance(playerObject.getRange()), BaseGameObject.class).getContent();
        for (GeoResult<BaseGameObject> geoResult : results) {
            res.add(geoResult.getContent());
        }
        return res;
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

    public Tower findTowerById(String id){
        Query query = new Query();
        query.addCriteria(Criteria.where("id").is(id));
        final Tower result = getMongoOperations().findOne(query,Tower.class);
        return result;
    }

    @Override
    public boolean isInRangeEachOther(BasePlayerObject object1, BasePlayerObject object2) {
        for (BaseGameObject object: gameObjectsNear(object1)) {
            if (object.equals(object2)) {
                return true;
            }
        }
        return false;
    }
}
