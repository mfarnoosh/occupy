package com.mcm.network.messages;

import com.mcm.enums.TowerPropertyType;
import com.mcm.enums.TowerType;
import com.mcm.enums.UnitPropertyType;
import com.mcm.enums.UnitType;
import com.mcm.util.GameConfig;

import java.util.LinkedList;
import java.util.List;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class UnitData {
    public String PlayerKey;
    public String Id;

    public int Type = -1;

    public double Range = 0.0;
    public double Lat = 0.0;
    public double Lon = 0.0;
    public double Level = 0.0;
    public double Health = 100.0;

    public static List<UnitData> getUnitConfig() {
        List<UnitData> result = new LinkedList<>();
        for (UnitType type : UnitType.values()) {
            int maxLevel = GameConfig.getUnitMaxLevel(type);
            for (int level = 1; level <= maxLevel; level++) {
                UnitData td = new UnitData();

                td.Type = type.getValue();
                td.Level = level;

                td.Range = Double.parseDouble(GameConfig.getUnitProperty(type,level, UnitPropertyType.RANGE));
                td.Health = Double.parseDouble(GameConfig.getUnitProperty(type,level, UnitPropertyType.HEALTH));

                result.add(td);
            }
        }
        return result;
    }
}
