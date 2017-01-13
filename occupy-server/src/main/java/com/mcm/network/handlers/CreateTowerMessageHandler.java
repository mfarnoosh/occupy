package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IGameObjectDao;
import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.mongo.Player;
import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;
import com.mcm.enums.TowerType;
import com.mcm.enums.UnitType;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.SocketMessage;
import com.mcm.network.messages.TowerData;
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
    @Autowired
    protected IPlayerDao playerDao;
    @Autowired
    protected IGameObjectDao gameObjectDao;
    private Logger logger = Logger.getLogger(CreateTowerMessageHandler.class);

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Save Tower msg: " + message);

        Player player = null;
        if (!StringUtils.isEmpty(message.PlayerKey))
            player = playerDao.findById(message.PlayerKey);

        double lat = Double.parseDouble(message.Params.get(0));
        double lon = Double.parseDouble(message.Params.get(1));

        int towerType = Integer.parseInt(message.Params.get(2));

        message.Params.clear();

        if (player == null) {
            message.ExceptionMessage = "Invalid Player key";
            return message;
        }
        if (TowerType.valueOf(towerType) == null) {
            message.ExceptionMessage = "Invalid Tower Type.";
            return message;
        }

        //find this tower is the first tower of user or not?
        int count = gameObjectDao.getPlayerTowersCount(player.getId());
        Tower tower = null;

        tower = new Tower(TowerType.valueOf(towerType), player, new double[]{lat, lon});
        gameObjectDao.save(tower);

        //this was first tower of player
        if(count <= 0){
            Unit unit1 = new Unit(UnitType.SOLDIER,player,tower);
            gameObjectDao.save(unit1);
            Unit unit2 = new Unit(UnitType.MOTOR,player,tower);
            gameObjectDao.save(unit2);
            Unit unit3 = new Unit(UnitType.TANK,player,tower);
            gameObjectDao.save(unit3);
            Unit unit4 = new Unit(UnitType.HELICOPTER,player,tower);
            gameObjectDao.save(unit4);
            Unit unit5 = new Unit(UnitType.AIRCRAFT,player,tower);
            gameObjectDao.save(unit5);
        }


        message.Params.add(new Gson().toJson(new TowerData(tower)));

        return message;
    }
}
