using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	public GameObject ManagerGameObject;

	public Location WorldCenter = new Location (35.70423f, 51.40570f);

	//public Vector3 ObjectScaleMultiplier { get { return new Vector3 (4, 4, 4); } }

	public string PlayerKey;

	private bool _loggedIn = false;

	public bool LoggedIn{ get { return _loggedIn; } }

	public static PlayerController Current;

	public PlayerController ()
	{
		Current = this;
	}

	void Start ()
	{
		//PlayerPrefs.DeleteKey ("config-version");

		string key = PlayerPrefs.GetString ("player-key");
		if (string.IsNullOrEmpty (key)) {
			signup ();
		} else {
			login (key);
		}
	}

	private void signup ()
	{
		//TODO: Farnoosh - Sign up process
		string email = "m.h.farnoosh88@gmail.com";
		string name = "mehrdad";
		string lastName = "farnoosh";

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "signup";
		sm.Params.Add (email);
		sm.Params.Add (name);
		sm.Params.Add (lastName);

		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			string key = data.value.PlayerKey;
			PlayerPrefs.SetString ("player-key", key);
			Debug.Log ("sign up successfully");
			login (key);
		}).OnError ((callback) => {
			Debug.Log ("Exception occured in signup: " + callback.error.Message);
		});
	}

	private void login (string key)
	{
		string configVersion = PlayerPrefs.GetString ("config-version");
		if (string.IsNullOrEmpty (configVersion))
			configVersion = "0";

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "login";
		sm.PlayerKey = key;
		sm.Params.Add (configVersion);
		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			PlayerKey = data.value.PlayerKey;

			bool configOK = bool.Parse (data.value.Params [0]);
			if (!configOK) {
				var configData = JsonUtility.FromJson<ConfigData> (data.value.Params [1]);
				Debug.LogFormat ("new Config detected, ver[{0}]. " ,configData.version);
				SaveMapConfig (configData.mapConfig);
				SaveTowersConfig (configData.towers);
				SaveUnitsConfig (configData.units);

				PlayerPrefs.SetString ("config-version", configData.version);
				PlayerPrefs.Save ();
			}

			_loggedIn = true;
			ManagerGameObject.SetActive (true);
			Debug.Log ("logged in successfully");
		}).OnError ((callback) => {
			Debug.Log ("Exception occured in login: " + callback.error.Message);
			signup ();
		});
	}

	private void SaveMapConfig (MapConfigData mapConfigData)
	{
		PlayerPrefs.SetFloat ("map.serverTileSizeX", (float)(mapConfigData.tileSizeX));
		PlayerPrefs.SetFloat ("map.serverTileSizeY", (float)(mapConfigData.tileSizeY));
		PlayerPrefs.SetInt ("map.tilesGridWidth", mapConfigData.tileGridWidth);
		PlayerPrefs.SetFloat ("map.moveSpeed", (float)(mapConfigData.moveSpeed));
	}

	private void SaveTowersConfig (List<TowerData> towers)
	{
		Dictionary<int,double> towerMaxLevel = new Dictionary<int, double> ();
		foreach (var tower in towers) {
			if (towerMaxLevel.ContainsKey (tower.Type)) {
				if (towerMaxLevel [tower.Type] < tower.Level)
					towerMaxLevel [tower.Type] = tower.Level;
			} else {
				towerMaxLevel.Add (tower.Type, tower.Level);
			}

			string key = "tower." + tower.Type + "." + tower.Level;
			PlayerPrefs.SetFloat (key + ".range", (float)(tower.Range));
			PlayerPrefs.SetFloat (key + ".health", (float)(tower.Health));
		}
		foreach (KeyValuePair<int,double> item in towerMaxLevel) {
			PlayerPrefs.SetFloat("tower." + item.Key+ ".maxLevel", (float)(towerMaxLevel[item.Key]));
			Debug.LogFormat ("tower.{0}.maxLevel = {1}",item.Key,towerMaxLevel[item.Key] );
		}
	}

	private void SaveUnitsConfig (List<UnitData> units)
	{
		Dictionary<int,double> unitMaxLevel = new Dictionary<int, double> ();
		foreach (var unit in units) {
			if (unitMaxLevel.ContainsKey (unit.Type)) {
				if (unitMaxLevel [unit.Type] < unit.Level)
					unitMaxLevel [unit.Type] = unit.Level;
			} else {
				unitMaxLevel.Add (unit.Type, unit.Level);
			}

			string key = "unit." + unit.Type + "." + unit.Level;
			PlayerPrefs.SetFloat (key + ".range", (float)(unit.Range));
			PlayerPrefs.SetFloat (key + ".health", (float)(unit.Health));
		}
		foreach (KeyValuePair<int,double> item in unitMaxLevel) {
			PlayerPrefs.SetFloat("unit." + item.Key+ ".maxLevel", (float)(unitMaxLevel[item.Key]));
			Debug.LogFormat ("unit.{0}.maxLevel = {1}",item.Key,unitMaxLevel[item.Key] );
		}
	}
}
