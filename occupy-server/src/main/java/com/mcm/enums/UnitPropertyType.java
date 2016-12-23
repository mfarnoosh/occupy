package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum UnitPropertyType {
    POWER("power"),
    POWER_FACTOR("power-factor"),
    DEFENCE_FACTOR("defence-factor"),
    RANGE("range"),
    HEALTH("health");

    private final String value;
    UnitPropertyType(String value){this.value = value;}

    public static UnitPropertyType getEnum(String value){
        for(UnitPropertyType type : values()){
            if(type.toString().equalsIgnoreCase(value))
                return type;
        }
        return null;
    }

    @Override
    public String toString() {
        return value;
    }
}
