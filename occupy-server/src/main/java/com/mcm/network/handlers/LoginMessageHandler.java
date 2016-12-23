package com.mcm.network.handlers;

import com.google.gson.Gson;
import com.mcm.network.messages.*;
import com.mcm.network.BaseMessageHandler;
import com.mcm.util.SharedPreference;
import org.apache.log4j.Logger;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.List;

/**
 * Created by Mehrdad on 16/12/11.
 */
@Component
@Scope("singleton")
public class LoginMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(LoginMessageHandler.class);
    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Login msg: " + message);
        int configVersion = Integer.parseInt(message.Params.get(0));

        message.Params.clear();

        if(configVersion < Integer.parseInt(SharedPreference.get("game_config_version"))){
            message.Params.add(String.valueOf(false));

            ConfigData config = new ConfigData();
            config.version = SharedPreference.get("game_config_version");
            config.mapConfig = MapConfigData.getMapConfig();
            config.towers = TowerData.getTowerConfig();
            config.units = UnitData.getUnitConfig();


            message.Params.add(new Gson().toJson(config));
        }else{
            message.Params.add(String.valueOf(true));
        }

        return message;
    }
}
