package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.BaseDocument;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.stereotype.Repository;

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


    default T findById(String id) {
        return getMongoOperations().findById(id, getClazz());
    }

    void deleteAll();
}
