package com.mcm.network.handlers;

import com.mcm.dao.mongo.interfaces.IPlayerDao;
import com.mcm.entities.MultilingualValue;
import com.mcm.entities.mongo.Player;
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
public class SignupMessageHandler extends BaseMessageHandler {
    private Logger logger = Logger.getLogger(SignupMessageHandler.class);

    @Autowired
    protected IPlayerDao playerDao;

    @Override
    public SocketMessage handle(SocketMessage message) {
        logger.info("Signup msg: " + message);

        String email = message.Params.get(0);
        String name = message.Params.get(1);
        String lastName = message.Params.get(2);

        Player player = playerDao.findByEmail(email);

        if(player == null){
            player = new Player();
            player.setEmail(email);
            player.setName(new MultilingualValue(name,name));
            player.setLastName(new MultilingualValue(lastName,lastName));
            player.setUsername(email);

            playerDao.save(player);
            logger.info("new player save.");
        }else{
            logger.info("player found.");
        }

        message.Params.clear();
        message.PlayerKey = player.getId();

        return message;
    }
}
