package com.mcm.network.messages;

import com.mcm.entities.mongo.gameObjects.playerObjects.Unit;

/**
 * Created by Mehrdad on 16/12/17.
 */
public class UnitData {
    public String PlayerKey;
    public String Id;
    public int Type = -1;
    public int Level = 0;
    public String towerId = "";

    public double CurrentHitPoint = 0.0;

    public double Lat = 0.0;
    public double Lon = 0.0;


    public boolean IsMoving = false;
    public boolean IsAttacking = false;
    public boolean IsUpgrading = false;

    public UnitData(Unit unit){
        PlayerKey = unit.getPlayerId();
        Id = unit.getId();
        Type = unit.getType().getValue();
        Lat = unit.getLocation()[0];
        Lon = unit.getLocation()[1];
        Level = unit.getLevel();
        CurrentHitPoint = unit.getCurrentHitPoint();
        IsAttacking = unit.isAttacking();
        IsUpgrading = unit.isUpgrading();
        IsMoving = unit.isMoving();
        towerId = unit.getKeepingTowerId();

    }
}
