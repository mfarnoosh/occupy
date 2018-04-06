package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum BuildingType {
    BANK(1),
    MOSQUE(2),
    PARK(3);

    private final int value;
    BuildingType(int value){this.value = value;}
    public int getValue(){return value;}
    public static BuildingType valueOf(int value){
        for(BuildingType type : values()){
            if(type.getValue() == value)
                return type;
        }
        return null;
    }
}
