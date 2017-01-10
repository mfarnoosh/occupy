using System;
using System.Collections;

[Serializable]
public class TowerConfigData : BaseObjectData {
	public int Type = -1;
	public int Level = 0;
	public float BuildTime = 0.0f;
	public float Value = 0.0f;
	public float HitPoint = 0.0f;
	public float Damage = 0.0f;
	public float FireRate = 0.0f;
	public float Range = 0.0f;
	public float MaxCapacity = 0.0f;
	public float UpgradePrice = 0.0f;
	public float UpgradeTime = 0.0f;

	public bool MaxLevel = false;
}
