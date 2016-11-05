package com.mcm.util;

import javafx.util.Pair;
import org.apache.log4j.Logger;
import org.springframework.stereotype.Component;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Created by Cross on 3/3/2015.
 * in db date format is yyyy/mm/dd and it always use java.util.date to write back in db not string.
 * so to write in database we should use toDate function with a persian date string as parameter and to
 * get from database we should call toPersian function with a java.util.date as parameter.
 */
@Component
public class DateUtil {
    private static Logger logger=Logger.getLogger(DateUtil.class);
    private static SimpleDateFormat sdfTime =  new SimpleDateFormat("HH:mm", Locale.getDefault());
    private final static String[] weeks = new String[]{
            "یکشنبه", "دوشنبه", "سه شنبه",
            "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"
    };
    private final static String[] months = new String[]{
            "فروردین", "اردیبهشت", "خرداد",
            "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر",
            "دی", "بهمن", "اسفند"
    };
    private final static Map<String, Integer> monthMap = new HashMap<String, Integer>();

    static {
        for (int i = 0; i < months.length; i++) {
            monthMap.put(months[i], i + 1);
        }
    }

    /**
     * it will convert a persian date string to java.util.date
     *
     * @return java.util.date of persian date string
     */
    public static Date toDate(int persianYear, int persianMonth, int persianDay) {
        JalaliCalendar.YearMonthDate ymd = JalaliCalendar.jalaliToGregorian(new JalaliCalendar.YearMonthDate(persianYear, persianMonth, persianDay));
        Calendar calendar = Calendar.getInstance();
        calendar.set(ymd.getYear(), ymd.getMonth(), ymd.getDate(), 0, 0, 0);
        return calendar.getTime();
    }

    public static String relativeDiffFromNow(Date date) {
        try {
            if (date == null)
                return "";
            final Date now = new Date();
            Long diff = now.getTime() - date.getTime();

            long diffSeconds = diff / 1000;
            long diffMinutes = diff / (60 * 1000);
            long diffHours = diff / (60 * 60 * 1000);
            long diffDays = diff / (24 * 60 * 60 * 1000);
            String relativeTime = "";
            if (diffDays > 90) {
                relativeTime = toPersianOnlyDate(date);
            } else if (diffDays > 6) {
                relativeTime = toPersianMonthAndDate(date);
            } else if (diffDays > 1) {
                relativeTime = toPersianDayOfWeek(date);
            } else if (diffDays > 0) {
                relativeTime = " دیروز ";
            } else {
                Calendar cal = Calendar.getInstance();
                int day = cal.get(Calendar.DATE);
                cal.setTime(date);
                int dateDay = cal.get(Calendar.DATE);
                if(day != dateDay)
                    relativeTime = " دیروز ";
                else
                    relativeTime = toTimeOnly(date);
            }
            return relativeTime;
        } catch (Exception ex) {
            return "";
        }
    }

    private static String toPersianDayOfWeek(Date date) {
        if (date == null) return "";
        final Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        return weeks[calendar.get(Calendar.DAY_OF_WEEK)];
    }

    /**
     * convert a new java.util.Date() to a persian date String
     *
     * @return converted new Date() as a persian date String
     */
    public static String toPersian() {
        return toPersian(new Date());
    }

    /**
     * convert a java.util.date to a persian date String
     *
     * @param date a java.util.date which we want to convert as a persian date string
     * @return converted date as a persian date String
     */
    public static String toPersian(Date date) {
        if (date == null) return "";
        final Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        final JalaliCalendar.YearMonthDate pc = JalaliCalendar.gregorianToJalali(new JalaliCalendar.YearMonthDate(calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DATE)));

        return pc.getDate() + " " + months[(pc.getMonth())] + " " + pc.getYear() + "\u200f" + " " + sdfTime.format(date);
    }

    public static String toPersianOnlyDate(Date date) {
        if (date == null) return "";
        final Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        final JalaliCalendar.YearMonthDate pc = JalaliCalendar.gregorianToJalali(new JalaliCalendar.YearMonthDate(calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DATE)));
        return pc.getYear() + "/" + (pc.getMonth() + 1) + "/" + pc.getDate();
    }


    public static String toPersianMonthAndDate(Date date) {
        if (date == null) return "";
        final Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        final JalaliCalendar.YearMonthDate pc = JalaliCalendar.gregorianToJalali(new JalaliCalendar.YearMonthDate(calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DATE)));
        return pc.getDate() + " " + months[(pc.getMonth())];
    }

    public static String toTimeOnly(Date date) {
        if (date == null) return "";
        return sdfTime.format(date);
    }

    public static JalaliCalendar.YearMonthDate toPersianYearMonthDate(Long value) {
        final Calendar calendar = Calendar.getInstance();
        if (value != null)
            calendar.setTime(new Date(value));
        final JalaliCalendar.YearMonthDate pc = JalaliCalendar.gregorianToJalali(new JalaliCalendar.YearMonthDate(calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DATE)));
        return pc;
    }

    public static String militaryTimeConversion(int time) {
        int hour = time / 100;
        int minute = time % 100;
        return String.valueOf(hour) + ":" + (minute > 9? String.valueOf(minute): "0" + String.valueOf(minute));
    }

    public static Pair<Integer, Integer> militaryTimeConversion(String time) throws NumberFormatException {
        if (time.contains(":")) {
            String data[] = time.split(":");
            return new Pair<>(Integer.parseInt(data[0]), Integer.parseInt(data[1]));
        }else throw new NumberFormatException();
    }

    public static String relativeDiffFromNowInDetail(Date date) {
        try {
            if (date == null)
                return "";
            final Date now = new Date();
            Long diff = now.getTime() - date.getTime();
            long diffSeconds = diff / 1000;
            long diffMinutes = diff / (60 * 1000);
            long diffHours = diff / (60 * 60 * 1000);
            long diffDays = diff / (24 * 60 * 60 * 1000);
            String relativeTime = "";
            if (diffDays > 90) {
                relativeTime = "\u200f" + toPersianOnlyDate(date) + " " + toTimeOnly(date) + "\u200f";
            } else if (diffDays > 6) {
                relativeTime = "\u200f" + toPersianMonthAndDate(date) + " " + toTimeOnly(date) + "\u200f";
            } else if (diffDays > 1) {
                relativeTime = "\u200f" + toPersianDayOfWeek(date) + " " + toTimeOnly(date) + "\u200f";
            } else if (diffDays > 0) {
                relativeTime = "\u200f" + " دیروز " + toTimeOnly(date) + "\u200f";
            } else if (diffHours > 4){
                relativeTime = toTimeOnly(date);
            } else if (diffMinutes > 59){
                relativeTime = "\u200f" + diffHours + " ساعت قبل " + "\u200f";
            } else if(diffMinutes > 4){
                relativeTime = "\u200f" + diffMinutes + " دقیقه قبل " + "\u200f";
            } else{
                relativeTime = " چند لحظه پیش ";
            }
            return relativeTime;
        } catch (Exception ex) {
            return "";
        }
    }
}
