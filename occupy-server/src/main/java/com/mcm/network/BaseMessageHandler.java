package com.mcm.network;

import com.mcm.network.messages.SocketMessage;

/**
 * Created by Mehrdad on 16/12/11.
 */

public abstract class BaseMessageHandler {
    public abstract SocketMessage handle(SocketMessage message);
}
