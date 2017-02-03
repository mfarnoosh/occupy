using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager :MonoBehaviour
{
	public GameObject SoldierPrefab;
	public GameObject MotorPrefab;
	public GameObject TankPrefab;
	public GameObject HelicopterPrefab;
	public GameObject AircraftPrefab;
	public GameObject TitanPrefab;

	private List<UnitConfigData> _unitsConfig = null;
	public static UnitManager Current;
	public UnitManager ()
	{
		Current = this;
	}

	private List<UnitConfigData> UnitsConfigList {
		get {
			if (_unitsConfig == null || _unitsConfig.Count <= 0) {
				LoadUnitsConfig ();
			}
			return _unitsConfig;
		}
	}
	public bool ShowUnitObject(Tower ownerTower,UnitData ud){
		if (ud.IsMoving || ud.IsAttacking)
			return true;
		return false;
	}
	public GameObject GetUnitGameObject (Tower ownerTower,UnitData ud)
	{
		GameObject go = GetUnitPrefabByType(ud.Type);
		if (go == null)
			return null;
		var unit = go.GetComponent<Unit> ();
		unit.unitData = ud;
		unit.parentTower = ownerTower;

		return go;
	}
	public UnitConfigData GetUnitConfig (int type, int level)
	{
		foreach (var unit in UnitsConfigList) {
			if (unit.Level == level && unit.Type == type) {
				return unit;
			}
		}
		return null;
	}
	public GameObject GetUnitPrefabByType(int unitType){
		switch (unitType) {
		case 1: //Soldier
			return GameObject.Instantiate (SoldierPrefab);
		case 2: //Motor
			return GameObject.Instantiate (MotorPrefab);
		case 3: //Tank
			return GameObject.Instantiate (TankPrefab);
		case 4: //Helicopter
			return GameObject.Instantiate (HelicopterPrefab);
		case 5: //Aircraft
			return GameObject.Instantiate (AircraftPrefab);
		case 6: //Titan
			return GameObject.Instantiate (TitanPrefab);
		}
			return null;
	}
	public GameObject GetUnitGhostPrefabByType(int unitType){
		switch (unitType) {
		case 1: //Soldier
			return GameObject.Instantiate (SoldierPrefab);
		case 2: //Motor
			return GameObject.Instantiate (MotorPrefab);
		case 3: //Tank
			return GameObject.Instantiate (TankPrefab);
		case 4: //Helicopter
			return GameObject.Instantiate (HelicopterPrefab);
		case 5: //Aircraft
			return GameObject.Instantiate (AircraftPrefab);
		case 6: //Titan
			return GameObject.Instantiate (TitanPrefab);
		}
		return null;
	}

	public string GetUnitNameByType(int unitType){
		switch (unitType) {
		case 1: //Soldier
			return "Soldier";
		case 2: //Motor
			return "Motor";
		case 3: //Tank
			return "Tank";
		case 4: //Helicopter
			return "Helicopter";
		case 5: //Aircraft
			return "Aircraft";
		case 6: //Titan
			return "Titan";
		}
		return "";
	}

	public void SaveUnitsConfig (List<UnitConfigData> units)
	{
		foreach (var unit in units) {
			string key = "unit." + unit.Type + "." + unit.Level;
			PlayerPrefs.SetFloat (key + ".build-time", (float)(unit.BuildTime));
			PlayerPrefs.SetFloat (key + ".value", (float)(unit.Value));
			PlayerPrefs.SetFloat (key + ".hit-point", (float)(unit.HitPoint));
			PlayerPrefs.SetFloat (key + ".attack-damage", (float)(unit.AttackDamage));
			PlayerPrefs.SetFloat (key + ".defence-damage", (float)(unit.DefenceDamage));
			PlayerPrefs.SetFloat (key + ".fire-rate", (float)(unit.FireRate));
			PlayerPrefs.SetFloat (key + ".range", (float)(unit.Range));
			PlayerPrefs.SetFloat (key + ".speed", (float)(unit.Speed));
			PlayerPrefs.SetFloat (key + ".upgrade-price", (float)(unit.UpgradePrice));
			PlayerPrefs.SetFloat (key + ".upgrade-time", (float)(unit.UpgradeTime));
			PlayerPrefs.SetInt (key + ".house-space", (unit.HouseSpace));

			if (unit.MaxLevel) {
				PlayerPrefs.SetInt ("unit." + unit.Type + ".max-level", unit.Level);
			}
		}

		this._unitsConfig = units;
	}

	private void LoadUnitsConfig ()
	{
		_unitsConfig = new List<UnitConfigData> ();
		for (int i = 1; i <= 5; i++) {
			int maxLevel = PlayerPrefs.GetInt ("unit." + i + ".max-level");
			for (int level = 1; level <= maxLevel; level++) {
				UnitConfigData unitConfig = new UnitConfigData ();

				unitConfig.Type = i;
				unitConfig.Level = level;

				string key = "unit." + i + "." + level;

				unitConfig.BuildTime = PlayerPrefs.GetFloat (key + ".build-time");
				unitConfig.Value = PlayerPrefs.GetFloat (key + ".value");
				unitConfig.HitPoint = PlayerPrefs.GetFloat (key + ".hit-point");
				unitConfig.AttackDamage = PlayerPrefs.GetFloat (key + ".attack-damage");
				unitConfig.DefenceDamage = PlayerPrefs.GetFloat (key + ".defence-damage");
				unitConfig.FireRate = PlayerPrefs.GetFloat (key + ".fire-rate");
				unitConfig.Range = PlayerPrefs.GetFloat (key + ".range");
				unitConfig.Speed = PlayerPrefs.GetFloat (key + ".speed");
				unitConfig.UpgradePrice = PlayerPrefs.GetFloat (key + ".upgrade-price");
				unitConfig.UpgradeTime = PlayerPrefs.GetFloat (key + ".upgrade-time");
				unitConfig.HouseSpace = PlayerPrefs.GetInt (key + ".house-space");

				if (level == maxLevel)
					unitConfig.MaxLevel = true;

				_unitsConfig.Add (unitConfig);
			}
		}
	}
}
