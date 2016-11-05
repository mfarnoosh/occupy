package com.mcm.util;

import org.springframework.stereotype.Component;

import java.util.Locale;

/**
 * Created by Cross on 2/18/2015.
 * Messages are in resources/messages.
 * default locale is fa.
 */
@Component
public class MessageBundle {
    private static Locale defaultLocale=new Locale("fa");

    /**
     *
     * @param msg the word
     * @param args arguments will be passed to messages: {0}
     * @param locale default locale is fa u can use another locale like new Locale("en")
     * @return
     */
    public static String get(String msg,Object[] args,Locale locale){
        try {
            return Spring.context.getMessage(msg, args, locale);
        }catch (Exception ex){
            return msg;
        }

    }

    /**
     *
     * @param msg the word
     * @param args arguments will be passed to messages: {0}
     * @return
     */
    public static String get(String msg,Object[] args){
        try {
            return Spring.context.getMessage(msg, args, defaultLocale);
        }catch (Exception ex){
            return msg;
        }

    }

    /**
     *
     * @param msg the word
     * @param locale default locale is fa u can use another locale like new Locale("en")
     * @return
     */
    public static String get(String msg,Locale locale){
        try {
            return Spring.context.getMessage(msg, null, locale);
        }catch (Exception ex){
            return msg;
        }

    }

    /**
     *
     * @param msg the word
     * @return
     */
    public static String get(String msg){
        try {
            return Spring.context.getMessage(msg, null, defaultLocale);
        }catch (Exception ex){
            return msg;
        }

    }

}
