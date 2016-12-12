package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.GameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.geo.GeoResult;
import org.springframework.data.mongodb.core.MongoOperations;
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
    public Class<GameObject> getClazz() {
        return GameObject.class;
    }

    @Override
    public MongoOperations getMongoOperations() {
        return mongoTemplate;
    }

    @Override
    public void deleteAll() {
        mongoTemplate.findAllAndRemove(new Query(), GameObject.class);
    }

    public LinkedHashSet<GameObject> gameObjectsNear(BasePlayerObject playerObject) {
        final LinkedHashSet<GameObject> res = new LinkedHashSet<>();
        if (playerObject.getLocation() == null || playerObject.getLocation().length != 2) {
            return res;
        }
        final List<GeoResult<GameObject>> results = getMongoOperations().geoNear(NearQuery.near(playerObject.getLocation()[0], playerObject.getLocation()[1])
                .inKilometers().maxDistance(playerObject.getRange()), GameObject.class).getContent();
        for (GeoResult<GameObject> geoResult : results) {
            res.add(geoResult.getContent());
        }
        return res;
    }
}
