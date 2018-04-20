package com.mcm;


import com.mcm.util.Spring;

import java.io.IOException;
import java.net.URISyntaxException;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;

/**
 * Created by Cross on 7/5/2015.
 */
public class Main {
    public static void main(String[] args) throws IOException, NoSuchAlgorithmException, KeyManagementException, URISyntaxException {
//        final Logger log=Logger.getLogger(Main.class);
//        final Factory factory = new IniSecurityManagerFactory("classpath:shiro.ini");
//        final SecurityManager securityManager = (SecurityManager) Spring.context.getBean("securityManager");
//        SecurityUtils.setSecurityManager(securityManager);
//        MediaListener.TMP_MEDIA = SharedPreference.get("uploadDir");
//        log.info("upload dir set to " + MediaListener.TMP_MEDIA);
//        log.info("login service is running.");
//

        Spring.context.getBean(MainThread.class).run();

    }

    //simple function to echo data to terminal
    public static void echo(String msg)
    {
        System.out.println(msg);
    }
}
