using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	public GameObject ManagerGameObject;

	public Location WorldCenter = new Location (35.70423f, 51.40570f);

	public Vector3 ObjectScaleMultiplier { get { return new Vector3 (4, 4, 4); } }

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
				SaveMapConfig (configData.mapConfig);
				SaveTowersConfig (configData.towers);
				SaveUnitsConfig (configData.units);

				PlayerPrefs.SetString ("config-version", configVersion);
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
		foreach (var tower in towers) {
			string key = "tower." + tower.Type + "." + tower.Level;
			PlayerPrefs.SetFloat (key + ".range", (float)(tower.Range));
			PlayerPrefs.SetFloat (key + ".health", (float)(tower.Health));
		}
	}

	private void SaveUnitsConfig (List<UnitData> units)
	{
		foreach (var unit in units) {
			string key = "unit." + unit.Type + "." + unit.Level;
			PlayerPrefs.SetFloat (key + ".range", (float)(unit.Range));
			PlayerPrefs.SetFloat (key + ".health", (float)(unit.Health));
		}
	}
}
