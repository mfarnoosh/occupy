package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.BaseDocument;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;

/**
 * Created by Ashki on 12/20/2015.
 */
public interface IBaseMongoDao<T extends BaseDocument> {
    Class<T> getClazz();

    MongoOperations getMongoOperations();

    default void save(T t) {
        if(t.getId() == null || t.getId().isEmpty())
            getMongoOperations().insert(t);
        else
            getMongoOperations().save(t);
    }
    default void delete(T t) {
        getMongoOperations().remove(t);
    }
    default void deleteAll() {
        getMongoOperations().findAllAndRemove(new Query(), getClazz());
    }
    default List<T> findAll() {
        return getMongoOperations().findAll(getClazz());
    }

    default T findById(String id) {
        return getMongoOperations().findById(id, getClazz());
    }


}
