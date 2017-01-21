using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{

	public GameObject SentryPrefab;
	public GameObject MachineGunPrefab;
	public GameObject RocketLauncherPrefab;
	public GameObject AntiAircraftPrefab;
	public GameObject StealthPrefab;

	public List<TowerConfigData> _towersConfig = null;

	private Tower _selectedTower = null;
	public Tower SelectedTower { 
		get{ return _selectedTower; } 
		set {
			this._selectedTower = value;
			if (_selectedTower != null)
				HUDManager.Current.TowerSelect (_selectedTower);
			else
				HUDManager.Current.TowerDeselect ();
		}
	}
	public static TowerManager Current;

	public TowerManager ()
	{
		Current = this;
	}

	private List<TowerConfigData> TowersConfigList {
		get {
			if (_towersConfig == null || _towersConfig.Count <= 0) {
				LoadTowersConfig ();
			}
			return _towersConfig;
		}
	}

	public GameObject CreateTower (TowerData td, Tile tile)
	{
		GameObject go = GetTowerPrefabByType(td.Type);
		if (go == null)
			return null;
			
		SetTowerInfo (go, td, tile);
		return go;
	}

	public GameObject GetTowerPrefabByType(int towerType){
		switch (towerType) {
		case 1: //Sentry
			return GameObject.Instantiate (SentryPrefab);
		case 2: //MachineGun
			return GameObject.Instantiate (MachineGunPrefab);
		case 3: //RocketLauncher
			return GameObject.Instantiate (RocketLauncherPrefab);
		case 4: //AntiAircraft
			return GameObject.Instantiate (AntiAircraftPrefab);
		case 5: //Stealth
			return GameObject.Instantiate (StealthPrefab);
		}
			return null;
	}

	public GameObject GetTowerGhostPrefabByType(int towerType){
		switch (towerType) {
		case 1: //Sentry
			return GameObject.Instantiate (SentryPrefab);
		case 2: //MachineGun
			return GameObject.Instantiate (MachineGunPrefab);
		case 3: //RocketLauncher
			return GameObject.Instantiate (RocketLauncherPrefab);
		case 4: //AntiAircraft
			return GameObject.Instantiate (AntiAircraftPrefab);
		case 5: //Stealth
			return GameObject.Instantiate (StealthPrefab);
		}
		return null;
	}

	public void SetTowerInfo (GameObject go, TowerData td, Tile parentTile)
	{
		var tower = go.GetComponent<Tower> ();
		if (tower == null)
			return;
		
		Location towerLocation = new Location ((float)(td.Lat), (float)(td.Lon));
		var pos = GeoUtils.LocationToXYZ (parentTile, towerLocation);

		go.transform.position = pos;
		go.transform.parent = parentTile.transform;

		parentTile.AddTower (go);

		tower.playerKey = td.PlayerKey;
		tower.id = td.Id;
		tower.type = td.Type;
		tower.level = td.Level;
		tower.currentHitPoint = (float)td.CurrentHitPoint;
		tower.location = towerLocation;
		tower.isAttacking = td.IsAttacking;
		tower.isUpgrading = td.IsUpgrading;
		tower.occupiedSpace = td.OccupiedSpace;
		if (td.Units != null && td.Units.Count > 0) {
			tower.AddUnit (td.Units);
		}
		tower.parentTile = parentTile;
	}

	public TowerConfigData GetTowerConfig (int type, int level)
	{
		foreach (var tower in TowersConfigList) {
			if (tower.Level == level && tower.Type == type) {
				return tower;
			}
		}
		return null;
	}

	public void SaveTowersConfig (List<TowerConfigData> towers)
	{
		foreach (var tower in towers) {
			string key = "tower." + tower.Type + "." + tower.Level;
			PlayerPrefs.SetFloat (key + ".build-time", (float)(tower.BuildTime));
			PlayerPrefs.SetFloat (key + ".value", (float)(tower.Value));
			PlayerPrefs.SetFloat (key + ".hit-point", (float)(tower.HitPoint));
			PlayerPrefs.SetFloat (key + ".air-damage", (float)(tower.AirDamage));
			PlayerPrefs.SetFloat (key + ".land-damage", (float)(tower.LandDamage));
			PlayerPrefs.SetFloat (key + ".fire-rate", (float)(tower.FireRate));
			PlayerPrefs.SetFloat (key + ".range", (float)(tower.Range));
			PlayerPrefs.SetFloat (key + ".max-capacity", (float)(tower.MaxCapacity));
			PlayerPrefs.SetFloat (key + ".upgrade-price", (float)(tower.UpgradePrice));
			PlayerPrefs.SetFloat (key + ".upgrade-time", (float)(tower.UpgradeTime));
			PlayerPrefs.SetInt (key + ".max-house-space", (tower.MaxHouseSpace));
			if (tower.MaxLevel) {
				PlayerPrefs.SetInt ("tower." + tower.Type + ".max-level", tower.Level);
			}
		}

		this._towersConfig = towers;
	}

	private void LoadTowersConfig ()
	{
		_towersConfig = new List<TowerConfigData> ();
		for (int i = 1; i <= 5; i++) {
			int maxLevel = PlayerPrefs.GetInt ("tower." + i + ".max-level");
			for (int level = 1; level <= maxLevel; level++) {
				TowerConfigData towerConfig = new TowerConfigData ();

				towerConfig.Type = i;
				towerConfig.Level = level;

				string key = "tower." + i + "." + level;

				towerConfig.BuildTime = PlayerPrefs.GetFloat (key + ".build-time");
				towerConfig.Value = PlayerPrefs.GetFloat (key + ".value");
				towerConfig.HitPoint = PlayerPrefs.GetFloat (key + ".hit-point");
				towerConfig.AirDamage = PlayerPrefs.GetFloat (key + ".air-damage");
				towerConfig.LandDamage = PlayerPrefs.GetFloat (key + ".land-damage");
				towerConfig.FireRate = PlayerPrefs.GetFloat (key + ".fire-rate");
				towerConfig.Range = PlayerPrefs.GetFloat (key + ".range");
				towerConfig.MaxCapacity = PlayerPrefs.GetFloat (key + ".max-capacity");
				towerConfig.UpgradePrice = PlayerPrefs.GetFloat (key + ".upgrade-price");
				towerConfig.UpgradeTime = PlayerPrefs.GetFloat (key + ".upgrade-time");
				towerConfig.MaxHouseSpace = PlayerPrefs.GetInt (key + ".max-house-space");

				if (level == maxLevel)
					towerConfig.MaxLevel = true;

				_towersConfig.Add (towerConfig);
			}
		}
	}
}
