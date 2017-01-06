package com.mcm.network;

import org.apache.log4j.Logger;
import org.springframework.beans.factory.config.BeanDefinition;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by alirezaghias on 10/19/2016 AD.
 */
@Component
@Scope(scopeName = BeanDefinition.SCOPE_SINGLETON)
public class NetworkManager {
    private final static Logger logger = Logger.getLogger(NetworkManager.class);
    private ExecutorService executorService = Executors.newFixedThreadPool(100);
    public void initializeServer(int port) throws IOException {
        ServerSocket serverSocket = new ServerSocket(port);
        logger.info("Sever started on port = " + port);
        while (!Thread.interrupted()) {
            Socket client = serverSocket.accept();
            executorService.execute(new ClientService(client));
        }
    }
}
