package com.mcm.util;

import org.apache.commons.codec.digest.DigestUtils;

/**
 * Created by FPDev06 on 5/25/2015.
 */
public class CommonUtil {
    public static String hashPass(String pass){
        return DigestUtils.sha1Hex(pass);
    }


}
