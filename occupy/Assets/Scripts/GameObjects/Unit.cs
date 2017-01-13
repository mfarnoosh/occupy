using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
	/// <summary>
	/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
	/// </summary>
	public int type = -1;
	public String playerKey;
	public String id;
	public int level = 0;
	public float currentHitPoint = 0.0f;
	public Location location;
	public bool isMoving = false;
	public bool isAttacking = false;
	public bool isUpgrading = false;

	public Tile parentTile;

	public UnitConfigData Config {
		get{ return UnitManager.Current.GetUnitConfig (type, level); }
	}

	void Start ()
	{
		InvokeRepeating ("GetDataFromServer", 3.0f, 1.0f);
	}
	private void GetDataFromServer(){
		SocketMessage message = new SocketMessage ();
		message.Cmd = "getUnitData";
		message.Params.Add (id);
		NetworkManager.Current.SendToServer (message).OnSuccess ((data) => {
			string unitStr = data.value.Params[0];
			var unitData = JsonUtility.FromJson<UnitData>(unitStr);

			UnitManager.Current.SetUnitInfo(this.gameObject,unitData,parentTile);
		});
	}
}
