package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.GameObject;
import org.springframework.data.geo.GeoResult;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.NearQuery;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashSet;
import java.util.List;

/**
 * Created by alirezaghias on 10/19/2016 AD.
 */
@Repository
public interface IGameObjectDao extends IBaseMongoDao<GameObject> {
    LinkedHashSet<GameObject> gameObjectsNear(GameObject gameObject);
}
