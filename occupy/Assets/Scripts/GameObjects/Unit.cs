using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
	/// <summary>
	/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
	/// </summary>
	public int type = -1;
	public UnitData unitData;

	public Tower parentTower;

	public UnitConfigData Config {
		get{ return UnitManager.Current.GetUnitConfig (type, unitData.Level); }
	}

	void Start ()
	{
		//InvokeRepeating ("GetDataFromServer", 3.0f, 1.0f);
	}
/*	unit will updated in Owner Tower script
 * private void GetDataFromServer(){
		SocketMessage message = new SocketMessage ();
		message.Cmd = "getUnitData";
		message.Params.Add (unitData.id);
		NetworkManager.Current.SendToServer (message).OnSuccess ((data) => {
			string unitStr = data.value.Params[0];
			var unitData = JsonUtility.FromJson<UnitData>(unitStr);

			UnitManager.Current.SetUnitInfo(this.gameObject,unitData,parentTower);
		});
	}
	*/
}
