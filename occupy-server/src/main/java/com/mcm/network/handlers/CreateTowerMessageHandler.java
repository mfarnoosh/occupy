package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.BaseGameObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.enums.TowerType;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.BaseMessageHandler;
import org.apache.commons.lang3.StringUtils;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;


/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class CreateTowerMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(CreateTowerMessageHandler.class);

    @Autowired
    protected IPlayerDao playerDao;

    @Autowired
    protected IGameObjectDao gameObjectDao;

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Save Tower msg: " + message);

        Player player = null;
        if(!StringUtils.isEmpty(message.PlayerKey))
            player = playerDao.findById(message.PlayerKey);

        if(player == null)
        {
            message.Params.clear();
            message.ExceptionMessage = "Invalid Player key";
            return message;
        }

        double lat = Double.parseDouble(message.Params.get(0));
        double lon = Double.parseDouble(message.Params.get(1));

        int type = Integer.parseInt(message.Params.get(2));

        Tower tower = null;

        if(TowerType.valueOf(type) != null) {
            tower = new Tower(type, player, new double[]{lat, lon});
            gameObjectDao.save(tower);
        }

        message.Params.clear();

        message.Params.add(new Gson().toJson(tower.getNewTowerData()));

        return message;
    }
}
