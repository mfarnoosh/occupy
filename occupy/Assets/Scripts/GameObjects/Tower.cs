using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{
	/// <summary>
	/// Sentry(1),MachineGun(2),RocketLauncher(3),AntiAircraft(4),Stealth(5)
	/// </summary>
	public GameObject flag;
	public int type = -1;
	public String playerKey;
	public String id;
	public int level = 0;
	public float currentHitPoint = 0.0f;
	private bool _mine = false;
	public bool mine {
		get {
			return _mine;	
		}
		set {
			_mine = value;
			if (flag != null)
				flag.SetActive (_mine);
		}
	}
	public Location location;

	public bool isAttacking = false;
	public bool isUpgrading = false;

	public int occupiedSpace = 0;

	private Tile _parentTile;

	public Tile ParentTile { 
		get { 
			if (_parentTile == null)
				_parentTile = GetComponentInParent<Tile> ();
			return _parentTile;
		}
	}

	public Dictionary<string,Unit> towerUnits = new Dictionary<string,Unit> ();

	public TowerConfigData Config {
		get{ return TowerManager.Current.GetTowerConfig (type, level); }
	}

	void Start ()
	{
		InvokeRepeating ("GetDataFromServer", 3.0f, 1.0f);
	}

	private void GetDataFromServer ()
	{
		SocketMessage message = new SocketMessage ();
		message.Cmd = "getTowerData";
		message.Params.Add (id);
		NetworkManager.Current.SendToServer (message).OnSuccess ((data) => {
			if (data.value != null && data.value.Params.Count > 0) {
				string towersStr = data.value.Params [0];
				var towerData = JsonUtility.FromJson<TowerData> (towersStr);

				TowerManager.Current.SetTowerInfo (this.gameObject, towerData, ParentTile);
			}
		});
	}

	public void AddOrUpdateUnits (List<UnitData> units)
	{
		foreach (var unit in units) {
			AddUnit (unit);
		}
		//trying to remove removed units from this tower
		List<string> shouldRemoveList = new List<string> ();
		foreach (var savedUnit in towerUnits) {
			bool shouldRemove = true;
			foreach (var unit in units) {
				if (savedUnit.Key.Equals (unit.Id)) {
					shouldRemove = false;
					break;
				}
			}
			if (shouldRemove)
				shouldRemoveList.Add (savedUnit.Key);
		}
		foreach (var key in shouldRemoveList) {
			var unit = towerUnits [key].GetComponent<Unit> ();
			unit.RemovedFromOwnerTower ();
			towerUnits.Remove (key);
		}
	}

	private void AddUnit (UnitData unitData)
	{
		Unit unit = null;
		if (towerUnits.ContainsKey (unitData.Id)) {
			//game object created before
			unit = towerUnits [unitData.Id];
		} else {
			//game object not created before
			GameObject go = UnitManager.Current.GetUnitGameObject (this, unitData, out unit);
			towerUnits.Add (unitData.Id, unit);
		}
		if (unit == null)
			Debug.Log ("why unit is null!!!");
		unit.UpdateData (unitData);
	}

	/*
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
	*/
}
