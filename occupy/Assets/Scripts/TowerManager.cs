using UnityEngine;
using System.Collections;

public class TowerManager : MonoBehaviour
{

	public GameObject SentryPrefab;
	public GameObject MachineGunPrefab;
	public GameObject RocketLauncherPrefab;
	public GameObject AntiAircraftPrefab;
	public GameObject StealthPrefab;

	public static TowerManager Current;

	public TowerManager ()
	{
		Current = this;
	}

	public GameObject CreateTower (TowerData td, Tile tile)
	{
		//var go = GameObject.Instantiate(SentryPrefab);
		//		go.transform.localScale = new Vector3(
		//			go.transform.localScale.x * PlayerController.Current.ObjectScaleMultiplier.x,
		//			go.transform.localScale.y * PlayerController.Current.ObjectScaleMultiplier.y,
		//			go.transform.localScale.z * PlayerController.Current.ObjectScaleMultiplier.z);
		//
		//		go.GetComponent<Renderer> ().material.color = col;
		//		//TODO: Convert Latitude,Longitude to X,Y
		//		var pos = GeoUtils.LocationToXYZ(tile,loc);
		//
		//		go.transform.position = pos;
		//		go.transform.parent = tile.transform;
		TowerData data = GetTowerData(td);
		GameObject go = null;
		switch (data.Type) {
		case 1: //Sentry
			go = GameObject.Instantiate (SentryPrefab);
			break;
		case 2: //MachineGun
			go = GameObject.Instantiate (MachineGunPrefab);
			break;
		case 3: //RocketLauncher
			go = GameObject.Instantiate (RocketLauncherPrefab);
			break;
		case 4: //AntiAircraft
			go = GameObject.Instantiate (AntiAircraftPrefab);
			break;
		case 5: //Stealth
			go = GameObject. Instantiate (StealthPrefab);
			break;
		}
		if (go == null) {
			Debug.LogError ("not found type of tower.");
			return null;
		}
		go.transform.localScale = new Vector3 (
			go.transform.localScale.x * PlayerController.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerController.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerController.Current.ObjectScaleMultiplier.z);

		var pos = GeoUtils.LocationToXYZ (tile, new Location((float)(data.Lat),(float)(data.Lon)));

		go.transform.position = pos;
		go.transform.parent = tile.transform;

		var tower = go.GetComponent<GameObjects.Tower> ();
		if (tower != null) {
			tower.FromObjectData (data);
		}

		return go;
	}

	public TowerData GetTowerData(TowerData td){
		string key = "tower." + td.Type + "." + td.Level;
		td.Range = PlayerPrefs.GetFloat (key + ".range");
		td.Health = PlayerPrefs.GetFloat (key + ".health");

		return td;
	}
	public TowerData GetTowerData(int towerType,int level){
		TowerData td = new TowerData ();
		td.Type = towerType;
		td.Level = level;

		return GetTowerData (td);
	}
}
