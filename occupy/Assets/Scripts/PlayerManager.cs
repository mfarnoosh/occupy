using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour {
	public Location WorldCenter = new Location(35.70423f,51.40570f);
	public Vector3 ObjectScaleMultiplier { get { return new Vector3 (4, 4, 4); } }
	public string PlayerKey;

	private bool _loggedIn = false;
	public bool LoggedIn{ get { return _loggedIn; } }

	public static PlayerManager Current;
	public PlayerManager(){
		Current = this;
	}
	void Start(){
		string key = PlayerPrefs.GetString ("player-key");
		if (string.IsNullOrEmpty (key)) {
			//TODO: Farnoosh - Sign up process
			string email = "m.h.farnoosh88@gmail.com";
			string name = "mehrdad";
			string lastName = "farnoosh";



			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "signup";
			sm.Params.Add (email);
			sm.Params.Add (name);
			sm.Params.Add (lastName);

			NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
				Debug.Log("Signned up successfully");
			}).OnError((callback) => {
				Debug.Log("Exception occured in signup: " + callback.error.Message);
			});
		} else {
			string configVersion = PlayerPrefs.GetString ("config-version");
			if (string.IsNullOrEmpty (configVersion))
				configVersion = "";

			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "login";
			sm.Params.Add (key);
			sm.Params.Add (configVersion);
			NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
				Debug.Log("logged in successfully");
			}).OnError((callback) => {
				Debug.Log("Exception occured in login: " + callback.error.Message);
			});
		}
	}
}
