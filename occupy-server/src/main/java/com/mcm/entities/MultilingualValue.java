package com.mcm.entities;

import com.mcm.util.StringUtil;

public class MultilingualValue {
    private String persian;
    private String english;

    public MultilingualValue(String persian, String english) {
        this.persian = persian;
        this.english = english;
    }

    public String getPersian() {
        return persian;
    }

    public void setPersian(String persian) {
        this.persian = StringUtil.persianFix(persian);
    }

    public String getEnglish() {
        return english;
    }

    public void setEnglish(String english) {
        this.english = english;
    }

    public String ToString() {
        return "MultilingualValue{" +
                "persian='" + persian + '\'' +
                ", english='" + english + '\'' +
                '}';
    }
}