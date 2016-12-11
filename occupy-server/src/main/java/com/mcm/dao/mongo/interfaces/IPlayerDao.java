package com.mcm.dao.mongo.interfaces;

import com.mcm.entities.mongo.GameObject;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashSet;

/**
 * Created by mehrdad on 11/12/2016 AD.
 */
public interface IPlayerDao extends IBaseMongoDao<Player> {
    Player findByEmail(String email);
}
