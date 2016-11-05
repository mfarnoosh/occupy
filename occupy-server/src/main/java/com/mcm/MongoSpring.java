package com.mcm;

import com.mcm.util.SharedPreference;
import com.mongodb.Mongo;
import com.mongodb.MongoClient;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.mongodb.config.AbstractMongoConfiguration;
import org.springframework.data.mongodb.config.EnableMongoAuditing;
import org.springframework.data.mongodb.repository.config.EnableMongoRepositories;

/**
 * Created by Ashki on 12/20/2015.
 */
@Configuration
@EnableMongoAuditing
@EnableMongoRepositories(basePackages = "com.mcm.dao.mongo")
public class MongoSpring extends AbstractMongoConfiguration {
    @Override
    protected String getDatabaseName() {
        return "mcm";
    }

    @Override
    public Mongo mongo() throws Exception {
        return new MongoClient(SharedPreference.get("mongo.address"));
    }

    protected String getMappingBasePackage() {
        return "com.mcm.entities.mango";
    }


}
