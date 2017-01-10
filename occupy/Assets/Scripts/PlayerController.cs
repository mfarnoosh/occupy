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
			ConfigData configData = null;
			if (!configOK) {
				configData = JsonUtility.FromJson<ConfigData> (data.value.Params [1]);
				Debug.LogFormat ("new Config detected, ver[{0}]. " ,configData.version);
			}

			ManagerGameObject.SetActive (true);

			if(configData != null){
				MapManager.Current.SaveMapConfig (configData.mapConfig);
				TowerManager.Current.SaveTowersConfig (configData.towers);
				UnitManager.Current.SaveUnitsConfig (configData.units);

				PlayerPrefs.SetString ("config-version", configData.version);
				PlayerPrefs.Save ();
			}
			_loggedIn = true;

			Debug.Log ("logged in successfully");
		}).OnError ((callback) => {
			Debug.Log ("Exception occured in login: " + callback.error.Message);
			signup ();
		});
	}
}
