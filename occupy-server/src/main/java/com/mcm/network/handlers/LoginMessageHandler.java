package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.mongo.Player;
import com.mcm.network.BaseMessageHandler;
import com.mcm.network.messages.*;
import com.mcm.util.SharedPreference;
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
public class LoginMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(LoginMessageHandler.class);

    @Autowired
    protected IPlayerDao playerDao;

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Login msg: " + message);
        String playerKey = message.PlayerKey;
        String email = message.Params.get(0);
        int configVersion = Integer.parseInt(message.Params.get(1));

        message.Params.clear();
        message.PlayerKey = "";

        if(StringUtils.isEmpty(playerKey) || StringUtils.isEmpty(email)) {
            message.ExceptionMessage = "empty player key or email";
            message.Params.add(String.valueOf(false)); //unsuccessful login
            return message;
        }
        Player playerByPlayerKey = playerDao.findById(playerKey);
        Player playerByEmail = playerDao.findByEmail(email);

        if(playerByPlayerKey == null || playerByEmail == null) {
            message.ExceptionMessage = "Invalid player key or email";
            message.Params.add(String.valueOf(false)); //unsuccessful login
            return message;
        }

        if(!playerByPlayerKey.getId().equals(playerByEmail.getId())){
            message.ExceptionMessage = "requested player key not equal to requested email";
            message.Params.add(String.valueOf(false)); //unsuccessful login
            return message;
        }

        message.PlayerKey = playerKey;
        message.Params.add(String.valueOf(true));//successful login

        if(configVersion != Integer.parseInt(SharedPreference.get("game_config_version"))){
            message.Params.add(String.valueOf(false)); //config not ok

            ConfigData config = new ConfigData();
            config.version = SharedPreference.get("game_config_version");
            config.mapConfig = MapConfigData.getMapConfig();
            config.towers = TowerConfigData.getAllTowerConfigs();
            config.units = UnitConfigData.getAllUnitConfigs();

            message.Params.add(new Gson().toJson(config));
        }else{
            message.Params.add(String.valueOf(true)); //config ok
        }

        return message;
    }
}
