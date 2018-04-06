package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum TowerType {
    SENTRY(1),
    MACHINE_GUN(2),
    ROCKET_LAUNCHER(3),
    ANTI_AIRCRAFT(4),
    STEALTH(5);

    private final int value;
    TowerType(int value){this.value = value;}
    public int getValue(){return value;}
    public static TowerType valueOf(int value){
        for(TowerType type : values()){
            if(type.getValue() == value)
                return type;
        }
        return null;
    }

}
