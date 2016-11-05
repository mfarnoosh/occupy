package com.mcm.util;

/**
 * Created by alirezaghias on 7/16/16.
 */
public class PhoneUtil {
    public static String toPlain(String phone) {
        if (phone != null)
            return phone.replace("+98", "0").replace("+", "00").replaceAll("[^0-9]+", "");
        return "";
    }



}
