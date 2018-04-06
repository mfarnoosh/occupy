using System;
using System.Collections;

[Serializable]
public class UnitConfigData : BaseObjectData{
	public int Type = -1;
	public int Level = 0;
	public double BuildTime = 0.0;
	public double Value = 0.0;
	public double HitPoint = 0.0;
	public double AttackDamage = 0.0;
	public double DefenceDamage = 0.0;
	public double FireRate = 0.0;
	public double Range = 0.0;
	public double Speed = 0.0;
	public double UpgradePrice = 0.0;
	public double UpgradeTime = 0.0;
	public int HouseSpace = 0;

	public bool MaxLevel = false;
}
