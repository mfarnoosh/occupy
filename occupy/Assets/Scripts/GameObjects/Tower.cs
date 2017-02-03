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

	private Dictionary<string,GameObject> UnitGOs = new Dictionary<string,GameObject>();

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
			if(data.value != null && data.value.Params.Count > 0){
				string towersStr = data.value.Params[0];
				var towerData = JsonUtility.FromJson<TowerData>(towersStr);

				TowerManager.Current.SetTowerInfo(this.gameObject,towerData,parentTile);
			}
		});
	}

	private void UpdateUnitObjects(){
		List<string> removedKey = new List<string> ();
		foreach(var unitGo in UnitGOs){
			bool shouldRemove = true;
			foreach(var unitData in towerUnits){
				if (unitGo.Key.Equals (unitData.Id)) {
					shouldRemove = false;
					break;
				}
			}
			if (shouldRemove) {
				Destroy (unitGo.Value);
				removedKey.Add (unitGo.Key);
			}
		}
		foreach (var key in removedKey)
			this.UnitGOs.Remove (key);

		foreach (var unit in towerUnits) {
			bool showUnitObject = UnitManager.Current.ShowUnitObject (this, unit);
			if (showUnitObject) {
				GameObject go = null;
				if (this.UnitGOs.ContainsKey (unit.Id)) {
					//game object created before
					go = this.UnitGOs [unit.Id];
				} else {
					//game object not created before
					go = UnitManager.Current.GetUnitGameObject (this, unit);
					go.transform.SetParent (gameObject.transform, true);
					this.UnitGOs.Add (unit.Id, go);
				}

				if (go != null && parentTile != null) {
					Location unitLocation = new Location ((float)(unit.Lat), (float)(unit.Lon));
					var pos = GeoUtils.LocationToXYZ (parentTile, unitLocation);
					go.transform.position = pos;
				}
			} else {
				if (this.UnitGOs.ContainsKey (unit.Id)) {
					Destroy (UnitGOs [unit.Id]);
					UnitGOs.Remove (unit.Id);
				}
			}
		}
	}
}
