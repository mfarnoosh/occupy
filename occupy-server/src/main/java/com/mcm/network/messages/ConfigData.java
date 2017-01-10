package com.mcm.network.messages;

import java.util.LinkedList;
import java.util.List;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class ConfigData {
    public String version;
    public MapConfigData mapConfig = new MapConfigData();
    public List<TowerConfigData> towers = new LinkedList<>();
    public List<UnitConfigData> units = new LinkedList<>();
}
