package com.mcm.network.messages.handlers;

import com.mcm.dao.mongo.PlayerDao;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.mongo.GameObject;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.playerObjects.BasePlayerObject;
import com.mcm.entities.mongo.gameObjects.playerObjects.towers.*;
import com.mcm.network.SocketMessage;
import com.mcm.network.messages.BaseMessageHandler;
import org.apache.commons.lang3.StringUtils;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.List;

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

        GameObject object = null;
        switch (type){
            case 1: //Sentry
                object = new Sentry(player,new double[]{lat,lon});
                break;
            case 2: //MachineGun
                object = new MachineGun(player,new double[]{lat,lon});
                break;
            case 3: //RocketLauncher
                object = new RocketLauncher(player,new double[]{lat,lon});
                break;
            case 4: //AntiAircraft
                object = new AntiAircraft(player,new double[]{lat,lon});
                break;
            case 5: //Stealth
                object = new Stealth(player,new double[]{lat,lon});
                break;
        }

        gameObjectDao.save(object);

/*
 message.Params.add(String.valueOf(35.70283f));
        message.Params.add(String.valueOf(51.40641f));


        message.Params.add(String.valueOf(35.70200f));
        message.Params.add(String.valueOf(51.40987f));


        message.Params.add(String.valueOf(35.70536f));
        message.Params.add(String.valueOf(51.40976f));


//        message.Params.add(String.valueOf(35.70374f));
//        message.Params.add(String.valueOf(51.40529f));

        message.Params.add(String.valueOf(35.702305));
        message.Params.add(String.valueOf(51.400487));

 */
        message.Params.clear();
        return message;
    }
}
