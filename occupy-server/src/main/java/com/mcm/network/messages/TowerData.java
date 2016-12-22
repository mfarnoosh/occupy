package com.mcm.network.messages;

import com.mcm.enums.TowerPropertyType;
import com.mcm.enums.TowerType;
import com.mcm.util.GameConfig;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class TowerData {
    public String PlayerKey;
    public String Id;

    public int Type = -1;

    public double Range = 0.0;
    public double Lat = 0.0;
    public double Lon = 0.0;
    public double Level = 0.0;
    public double Health = 100.0;

    public static List<TowerData> getTowerConfig() {
        List<TowerData> result = new LinkedList<>();
        for (TowerType type : TowerType.values()) {
            int maxLevel = GameConfig.getTowerMaxLevel(type);
            for (int level = 1; level <= maxLevel; level++) {
                TowerData td = new TowerData();

                td.Type = type.getValue();
                td.Level = level;

                td.Range = Double.parseDouble(GameConfig.getTowerProperty(type,level, TowerPropertyType.RANGE));
                td.Health = Double.parseDouble(GameConfig.getTowerProperty(type,level, TowerPropertyType.HEALTH));

                result.add(td);
            }
        }
        return result;
    }
}
