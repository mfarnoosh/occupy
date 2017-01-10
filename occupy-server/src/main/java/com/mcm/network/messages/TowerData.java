package com.mcm.network.messages;

import com.mcm.entities.mongo.gameObjects.playerObjects.Tower;

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

    public TowerData(Tower tower){
        PlayerKey = tower.getPlayer().getId();
        Id = tower.getId();
        Type = tower.getType().getValue();
        Lat = tower.getLocation()[0];
        Lon = tower.getLocation()[1];
        Level = tower.getLevel();
        CurrentHitPoint = tower.getCurrentHitPoint();
        IsAttacking = tower.isAttacking();
        IsUpgrading = tower.isUpgrading();
    }
}
