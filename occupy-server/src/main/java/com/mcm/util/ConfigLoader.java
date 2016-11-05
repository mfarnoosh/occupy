package com.mcm.util;

import org.springframework.stereotype.Component;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

/**
 * Created by Cross on 7/7/2015.
 */
@Component
public class ConfigLoader {
    private final Map<String,Properties> cacheConfig=new HashMap<>();
    public Properties load(String resource){
        if(cacheConfig.containsKey(resource)) return cacheConfig.get(resource);
        final Properties tmp=new Properties();
        final InputStream inputStream=getClass().getClassLoader().getResourceAsStream(resource);
        try {
            tmp.load(inputStream);
            cacheConfig.put(resource,tmp);
            return tmp;
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }
}
