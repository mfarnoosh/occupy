package com.mcm.util;

import org.apache.axis.utils.StringUtils;

import java.util.UUID;

/**
 * Created by Cross on 2/17/2015.
 * helper class for String functions.
 */
public class StringUtil {
    /**
     *
     * @param word will get a word and capitalize its first character.
     * @return a capitalized word
     */
    public static String capitalize(String word)
    {
        return Character.toUpperCase(word.charAt(0)) + word.substring(1);
    }
    public static String randomString(int characterLength){
        return UUID.randomUUID().toString().substring(0,characterLength);
    }

    public static String addSeparator(String s) {
        if (s == null) return "";
        StringBuilder f = new StringBuilder();
        String temp = s.replaceAll(",", "");
        for (int i = 0; i < temp.length(); ++i) {
            f.append(temp.charAt(i));
            if (((temp.length() - 1) - i) % 3 == 0 && ((temp.length() - 1) - i) != 0) {
                f.append(",");
            }
        }
        return f.toString();
    }
    public static String persianFix(String text) {
        if (StringUtils.isEmpty(text)) {
            return "";
        } else {
            return text.replaceAll("ك", "ک").replaceAll("ي", "ی");
        }
    }
}
