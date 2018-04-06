﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	public GameObject ManagerGameObject;
	public Location WorldCenter = new Location (35.70423f, 51.40570f);

	#region Test mode...

	//TODO: Farnoosh - remove these lines in real production and change them with UI elements
	public string Email = "m.h.farnoosh88@gmail.com";
	public string Name = "mehrdad";
	public string LastName = "farnoosh"; 

	public bool SignoutVar = false;
	#endregion

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
		Init();
	}
	void Update(){
		if (SignoutVar) {
			Signout ();
			SignoutVar = false;
			Init ();
		}
	}
	private void Init(){
		var cachedData = LoadCachedData ();
		if (!string.IsNullOrEmpty (cachedData.Email) && !string.IsNullOrEmpty (cachedData.PlayerKey)) {
			Debug.LogFormat ("Cached data: {0} , {1}",cachedData.Email,cachedData.PlayerKey);
			Login (cachedData);
		} else {
			Debug.Log ("Invalid Cached data");
			GetUserDataAndSignup ();
		}
	}
	private SignupLoginData LoadCachedData ()
	{
		SignupLoginData data = new SignupLoginData ();
		data.PlayerKey = PlayerPrefs.GetString ("player-key");
		data.Email = PlayerPrefs.GetString ("player-email");

		return data;
	}

	private void SaveCachedData (SignupLoginData cachedData)
	{
		PlayerPrefs.SetString ("player-key", cachedData.PlayerKey);
		PlayerPrefs.SetString ("player-email", cachedData.Email);

		PlayerPrefs.Save ();
	}

	private SignupLoginData GetUserData ()
	{
		SignupLoginData data = new SignupLoginData ();
		//TODO: Farnoosh - theses variables filled from UI elements in production...
		data.Email = this.Email;
		data.Name = this.Name;
		data.LastName = this.LastName;

		return data;
	}

	private void GetUserDataAndSignup ()
	{
		var userEnteredData = GetUserData ();
		Signup (userEnteredData);
	}
	private void Signup (SignupLoginData signupData)
	{
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "signup";
		sm.Params.Add (signupData.Email);
		sm.Params.Add (signupData.Name);
		sm.Params.Add (signupData.LastName);

		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			string key = data.value.PlayerKey;
			if (!string.IsNullOrEmpty (key) && string.IsNullOrEmpty (data.value.ExceptionMessage)) {
				//successful signed up
				SaveCachedData (new SignupLoginData{ PlayerKey = key, Email = signupData.Email });
				Debug.Log ("Sign up successfull");
				Init();
			} else {
				//unsuccessful signed up
				Debug.Log ("Unsuccessful sign up");
				Init();
			}
		}).OnError ((callback) => {
			Debug.Log ("Exception occured in signup: " + callback.error.Message);
			Init();
		});
	}

	private void Login (SignupLoginData loginData)
	{
		if (string.IsNullOrEmpty (loginData.Email) || string.IsNullOrEmpty (loginData.PlayerKey)) {
			Init ();
			return;
		}
		string configVersion = PlayerPrefs.GetString ("config-version");
		if (string.IsNullOrEmpty (configVersion))
			configVersion = "0";

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "login";
		sm.PlayerKey = loginData.PlayerKey;
		sm.Params.Add (loginData.Email);
		sm.Params.Add (configVersion);
		Debug.LogFormat ("login req: {0},{1},{2}",loginData.PlayerKey,loginData.Email,configVersion);
		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			bool loginSuccess = bool.Parse(data.value.Params[0]);
			if(loginSuccess && !string.IsNullOrEmpty(data.value.PlayerKey)){
				this.PlayerKey = data.value.PlayerKey;

				//below line must called first, before any config processed in managers...
				ManagerGameObject.SetActive (true);

				bool isConfigVersionOk = bool.Parse(data.value.Params[1]);
				if(isConfigVersionOk){
					//login success and config version ok...
				}else{
					ConfigData configData = JsonUtility.FromJson<ConfigData> (data.value.Params [2]);
					Debug.LogFormat ("new Config detected, ver[{0}]. ", configData.version);

					if(configData != null){
						PlayerPrefs.DeleteAll ();
						MapManager.Current.SaveMapConfig (configData.mapConfig);
						TowerManager.Current.SaveTowersConfig (configData.towers);
						UnitManager.Current.SaveUnitsConfig (configData.units);

						PlayerPrefs.SetString ("config-version", configData.version);
						PlayerPrefs.Save ();
					}
				}
				//this boolean variable exists because we must enable Managers but prevent to continue
				//their process before sure managers saved their config data...
				_loggedIn = true;
				Debug.Log ("logged in successfully");
			}else{
				PlayerPrefs.DeleteAll ();
				Init();
				return;
			}
		}).OnError ((callback) => {
			Debug.Log ("Exception occured in login: " + callback.error.Message);
			Init();
		});
	}
	//Known BUG
	//TODO: Farnoosh - after Signout and signup again, two player document created...
	private void Signout(){
		ManagerGameObject.SetActive (false);
		_loggedIn = false;

		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();

		Init ();
	}
}
