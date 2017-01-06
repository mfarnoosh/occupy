package com.mcm.dao.mongo;

import com.mcm.dao.mongo.interfaces.IAttackEventDao;
import com.mcm.dao.mongo.interfaces.IMoveEventDao;
import com.mcm.entities.mongo.events.AttackEvent;
import com.mcm.entities.mongo.events.MoveEvent;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

/**
 * Created by alirezaghias on 1/6/2017 AD.
 */
@Repository
public class AttackEventDao implements IAttackEventDao {
    @Autowired
    MongoOperations mongoTemplate;
    @Override
    public Class<AttackEvent> getClazz() {
        return AttackEvent.class;
    }

    @Override
    public MongoOperations getMongoOperations() {
        return mongoTemplate;
    }


}
