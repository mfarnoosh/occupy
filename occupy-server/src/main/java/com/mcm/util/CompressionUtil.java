package com.mcm.util;

import org.apache.log4j.Logger;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

/**
 * Created by Ashki on 12/16/2015.
 */
public class CompressionUtil {
    private static Logger logger = Logger.getLogger(CompressionUtil.class);

    public static byte[] decompress(byte[] bytes) {
        try {
            GZIPInputStream gis = new GZIPInputStream(new ByteArrayInputStream(bytes));
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            byte[] tmp = new byte[1024];
            for (int len; (len = gis.read(tmp)) > 0; ) {
                bos.write(tmp, 0, len);
            }
            bos.flush();
            bos.close();
            return bos.toByteArray();
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return bytes;
    }

    public static byte[] compress(byte[] array) {
        try {
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            GZIPOutputStream gzip = new GZIPOutputStream(bos);
            gzip.write(array);
            gzip.flush();
            gzip.close();
            final byte[] tmp = bos.toByteArray();
            logger.info("compressed " + array.length + " to " + tmp.length + " compressed= " + (array.length - tmp.length));
            return tmp;
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return null;
    }
}
