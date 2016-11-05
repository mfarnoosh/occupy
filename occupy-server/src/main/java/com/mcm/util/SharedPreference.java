package com.mcm.util;

import java.io.IOException;
import java.util.Properties;

/**
 * Created by Ashki on 9/22/2015.
 */
public class SharedPreference {
    private static Properties config = new Properties();

    static {
        try {
            config.load(SharedPreference.class.getClassLoader().getResourceAsStream("preferences.properties"));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static void reload() {
        try {
            config.load(SharedPreference.class.getClassLoader().getResourceAsStream("preferences.properties"));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static String get(String key) {
        try {
            return (String) config.get(key);
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }
    }

    public static void set(String key, String value) {
        try {
            config.setProperty(key, value);
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
}
