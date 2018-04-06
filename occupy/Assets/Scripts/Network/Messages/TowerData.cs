using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TowerData : BaseObjectData{
	public String PlayerKey;
	public String Id;
	public int Type = -1;
	public int Level = 0;

	public double CurrentHitPoint = 0.0;

	public double Lat = 0.0;
	public double Lon = 0.0;

	public bool IsAttacking = false;
	public bool IsUpgrading = false;

	public int OccupiedSpace = 0;

	public List<UnitData> Units = new List<UnitData>();
}
