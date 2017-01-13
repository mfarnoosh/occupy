package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.BaseMessageHandler;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class MoveTowerMessageHandler extends BaseMessageHandler {
    @Autowired
    IGameObjectDao gameObjectDao;
    private Logger logger = Logger.getLogger(MoveTowerMessageHandler.class);

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Move Tower msg: " + message);

        String id = String.valueOf(message.Params.get(0));
        double lat = Double.parseDouble(message.Params.get(1));
        double lon = Double.parseDouble(message.Params.get(2));

        message.Params.clear();

        Tower t = gameObjectDao.findTowerById(id);
        if (t != null && t.getPlayerId().equals(message.PlayerKey)) {
            t.move(new double[]{lat,lon});
            gameObjectDao.save(t);
        }else{
            message.ExceptionMessage = "Invalid tower.";
        }


        return message;
    }
}
