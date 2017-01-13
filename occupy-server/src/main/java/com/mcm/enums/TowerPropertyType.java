package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum TowerPropertyType {
    BUILD_TIME("build-time"),
    VALUE("value"),
    HIT_POINT("hit-point"),
    AIR_DAMAGE("air-damage"),
    LAND_DAMAGE("land-damage"),
    FIRE_RATE("fire-rate"),
    RANGE("range"),
    MAX_CAPACITY("max-capacity"),
    UPGRADE_PRICE("upgrade-price"),
    UPGRADE_TIME("upgrade-time"),
    MAX_HOUSE_SPACE("max-space");

    private final String value;
    TowerPropertyType(String value){this.value = value;}


    public static TowerPropertyType getEnum(String value){
        for(TowerPropertyType type : values()){
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
