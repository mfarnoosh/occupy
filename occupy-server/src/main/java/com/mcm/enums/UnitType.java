package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum UnitType {
    SOLDIER(1),
    MOTOR(2),
    TANK(3),
    HELICOPTER(4),
    AIRCRAFT(5),
    TITAN(6);

    private final int value;
    UnitType(int value){this.value = value;}
    public int getValue(){return value;}
    public static UnitType valueOf(int value){
        for(UnitType type : values()){
            if(type.getValue() == value)
                return type;
        }
        return null;
    }
}
