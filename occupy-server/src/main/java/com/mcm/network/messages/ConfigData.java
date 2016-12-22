package com.mcm.network.messages;

import com.mcm.enums.MapConfig;
import com.mcm.enums.UnitPropertyType;
import com.mcm.enums.UnitType;
import com.mcm.util.GameConfig;

import java.util.LinkedList;
import java.util.List;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class ConfigData {
    public String version;
    public MapConfigData mapConfig = new MapConfigData();
    public List<TowerData> towers = new LinkedList<>();
    public List<UnitData> units = new LinkedList<>();
}
