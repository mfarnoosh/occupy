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

	public GameObject CreateUnit (UnitData td, Tile tile)
	{
		//Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
		GameObject go = null;
		switch (td.Type) {
		case 1: //Soldier
			go = GameObject.Instantiate (SoldierPrefab);
			break;
		case 2: //Motor
			go = GameObject.Instantiate (MotorPrefab);
			break;
		case 3: //Tank
			go = GameObject.Instantiate (TankPrefab);
			break;
		case 4: //Helicopter
			go = GameObject.Instantiate (HelicopterPrefab);
			break;
		case 5: //Aircraft
			go = GameObject.Instantiate (AircraftPrefab);
			break;
		case 6: //Titan
			go = GameObject.Instantiate (TitanPrefab);
			break;
		}
		if (go == null) {
			Debug.LogError ("not found type of unit.");
			return null;
		}

		SetUnitInfo (go, td, tile);
		return go;
	}

	public void SetUnitInfo (GameObject go, UnitData td, Tile parentTile)
	{
		var unit = go.GetComponent<Unit> ();
		if (unit == null)
			return;

		Location unitLocation = new Location ((float)(td.Lat), (float)(td.Lon));
		var pos = GeoUtils.LocationToXYZ (parentTile, unitLocation);

		go.transform.position = pos;
		go.transform.parent = parentTile.transform;

		parentTile.AddUnit (go);

		unit.playerKey = td.PlayerKey;
		unit.id = td.Id;
		unit.type = td.Type;
		unit.level = td.Level;
		unit.currentHitPoint = (float)td.CurrentHitPoint;
		unit.location = unitLocation;
		unit.isAttacking = td.IsAttacking;
		unit.isUpgrading = td.IsUpgrading;
		unit.isMoving = td.IsMoving;
		unit.parentTile = parentTile;
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

	public void SaveUnitsConfig (List<UnitConfigData> units)
	{
		foreach (var unit in units) {
			string key = "unit." + unit.Type + "." + unit.Level;
			PlayerPrefs.SetFloat (key + ".build-time", (float)(unit.BuildTime));
			PlayerPrefs.SetFloat (key + ".value", (float)(unit.Value));
			PlayerPrefs.SetFloat (key + ".hit-point", (float)(unit.HitPoint));
			PlayerPrefs.SetFloat (key + ".damage", (float)(unit.Damage));
			PlayerPrefs.SetFloat (key + ".fire-rate", (float)(unit.FireRate));
			PlayerPrefs.SetFloat (key + ".range", (float)(unit.Range));
			PlayerPrefs.SetFloat (key + ".speed", (float)(unit.Speed));
			PlayerPrefs.SetFloat (key + ".upgrade-price", (float)(unit.UpgradePrice));
			PlayerPrefs.SetFloat (key + ".upgrade-time", (float)(unit.UpgradeTime));
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
				unitConfig.Damage = PlayerPrefs.GetFloat (key + ".damage");
				unitConfig.FireRate = PlayerPrefs.GetFloat (key + ".fire-rate");
				unitConfig.Range = PlayerPrefs.GetFloat (key + ".range");
				unitConfig.Speed = PlayerPrefs.GetFloat (key + ".speed");
				unitConfig.UpgradePrice = PlayerPrefs.GetFloat (key + ".upgrade-price");
				unitConfig.UpgradeTime = PlayerPrefs.GetFloat (key + ".upgrade-time");

				if (level == maxLevel)
					unitConfig.MaxLevel = true;

				_unitsConfig.Add (unitConfig);
			}
		}
	}
}
