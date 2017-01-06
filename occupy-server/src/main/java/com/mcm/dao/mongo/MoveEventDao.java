package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.mongo.events.MoveEvent;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
@Repository
public class MoveEventDao implements IMoveEventDao {
    @Autowired
    MongoOperations mongoTemplate;
    @Override
    public Class<MoveEvent> getClazz() {
        return MoveEvent.class;
    }

    @Override
    public MongoOperations getMongoOperations() {
        return mongoTemplate;
    }


}
