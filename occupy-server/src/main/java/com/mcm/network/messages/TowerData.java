package com.mcm.network.messages;

import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;
import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;

import java.util.LinkedList;
import java.util.List;
import java.util.stream.Collectors;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class TowerData {
    public String PlayerKey;
    public String Id;
    public int Type = -1;
    public int Level = 0;

    public double CurrentHitPoint = 0.0;

    public double Lat = 0.0;
    public double Lon = 0.0;

    public boolean IsAttacking = false;
    public boolean IsUpgrading = false;

    public int OccupiedSpace = 0;

    public List<UnitData> Units = new LinkedList<>();

    public TowerData(Tower tower, List<Unit> units){
        PlayerKey = tower.getPlayerId();
        Id = tower.getId();
        Type = tower.getType().getValue();
        Lat = tower.getLocation()[0];
        Lon = tower.getLocation()[1];
        Level = tower.getLevel();
        CurrentHitPoint = tower.getCurrentHitPoint();
        IsAttacking = tower.isAttacking();
        IsUpgrading = tower.isUpgrading();
        OccupiedSpace = tower.getOccupiedHouseSpace();
        if(units != null)
            Units.addAll(units.stream().map(UnitData::new).collect(Collectors.toList()));
    }
}
