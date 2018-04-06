package com.mcm.enums;

/**
 * Created by Mehrdad on 16/12/13.
 */
public enum MapConfig {
    ZOOM_LEVEL("zoom-level"),
    TILE_SIZE_X("tile-size-x"),
    TILE_SIZE_Y("tile-size-y"),
    TILE_GRID_WIDTH("tile-grid-width"),
    MOVE_SPEED("move-speed");

    private final String value;
    MapConfig(String value){this.value = value;}

    public static MapConfig getEnum(String value){
        for(MapConfig type : values()){
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
