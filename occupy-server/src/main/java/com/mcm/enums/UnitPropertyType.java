package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum UnitPropertyType {
    BUILD_TIME("build-time"),
    VALUE("value"),
    HIT_POINT("hit-point"),
    ATTACK_DAMAGE("attack-damage"),
    DEFENCE_DAMAGE("defence-damage"),
    FIRE_RATE("fire-rate"),
    RANGE("range"),
    SPEED("speed"),
    UPGRADE_PRICE("upgrade-price"),
    UPGRADE_TIME("upgrade-time"),
    HOUSE_SPACE("house-space");

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
