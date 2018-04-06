package com.mcm.network.messages;

import com.mcm.enums.MapConfig;
import com.mcm.util.GameConfig;
/**
 * Created by Mehrdad on 16/12/17.
 */
public class MapConfigData {
    public double tileSizeX;
    public double tileSizeY;
    public int tileGridWidth;
    public double moveSpeed;

    public static MapConfigData getMapConfig() {
        MapConfigData result = new MapConfigData();

        result.tileSizeX = Double.parseDouble(GameConfig.getMapProperty(MapConfig.TILE_SIZE_X));
        result.tileSizeY = Double.parseDouble(GameConfig.getMapProperty(MapConfig.TILE_SIZE_Y));
        result.tileGridWidth = Integer.parseInt(GameConfig.getMapProperty(MapConfig.TILE_GRID_WIDTH));
        result.moveSpeed = Double.parseDouble(GameConfig.getMapProperty(MapConfig.MOVE_SPEED));

        return result;
    }
}
