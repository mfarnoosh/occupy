package com.mcm.network;

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
    private ExecutorService executorService = Executors.newFixedThreadPool(100);
    public void initializeServer(int port) throws IOException {
        ServerSocket serverSocket = new ServerSocket(port);
        while (!Thread.interrupted()) {
            Socket client = serverSocket.accept();
            executorService.execute(new ClientService(client));
        }
    }
}
