using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{
	/// <summary>
	/// Sentry(1),MachineGun(2),RocketLauncher(3),AntiAircraft(4),Stealth(5)
	/// </summary>

	public int type = -1;
	public String playerKey;
	public String id;
	public int level = 0;
	public float currentHitPoint = 0.0f;

	public Location location;

	public bool isAttacking = false;
	public bool isUpgrading = false;

	public int occupiedSpace = 0;

	public Tile parentTile;
	public List<UnitData> towerUnits = new List<UnitData> ();

	public void AddUnit(List<UnitData> units){
		this.towerUnits.Clear();
		this.towerUnits.AddRange (units);
		UpdateUnitObjects ();
	}
	public TowerConfigData Config {
		get{ return TowerManager.Current.GetTowerConfig (type, level); }
	}
	void Start(){
		InvokeRepeating ("GetDataFromServer", 3.0f, 1.0f);
	}
	private void GetDataFromServer(){
		SocketMessage message = new SocketMessage ();
		message.Cmd = "getTowerData";
		message.Params.Add (id);
		NetworkManager.Current.SendToServer (message).OnSuccess ((data) => {
			string towersStr = data.value.Params[0];
			var towerData = JsonUtility.FromJson<TowerData>(towersStr);

			TowerManager.Current.SetTowerInfo(this.gameObject,towerData,parentTile);
		});
	}

	private void UpdateUnitObjects(){
		Debug.Log ("units in towers: " + towerUnits.Count);
		return;
		foreach (var unit in towerUnits) {
			bool showUnitObject = UnitManager.Current.ShowUnitObject (this, unit);
			if (showUnitObject) {
				var go = UnitManager.Current.GetUnitGameObject (this,unit);
				if (go != null) {
					Location unitLocation = new Location ((float)(unit.Lat), (float)(unit.Lon));
					var pos = GeoUtils.LocationToXYZ (parentTile, unitLocation);

					go.transform.position = pos;

					go.transform.SetParent (gameObject.transform,true);
				}
			}
		}
	}
}
