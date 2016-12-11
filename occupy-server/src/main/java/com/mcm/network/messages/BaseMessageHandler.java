package com.mcm.network.messages;

import com.mcm.network.SocketMessage;

/**
 * Created by Mehrdad on 16/12/11.
 */
public abstract class BaseMessageHandler {
    public abstract SocketMessage handle(SocketMessage message);
}
