package com.mcm.network.messages;

import java.util.List;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class TileData {
    public String ImageBytes;

    public float CenterLat;
    public float CenterLon;

    public float North;
    public float East;
    public float South;
    public float West;

    public int PositionX;
    public int PositionY;

    public List<TowerData> towers;
    //public List<UnitData> units;
}
