using UnityEngine;
using System.Collections;
using System;
public class Test : MonoBehaviour {
	public GameObject Map;
	// Use this for initialization
	void Start () {
		float lat = 35.70167f;
		float lon = 51.40097f;
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "getTile";
		sm.Params.Add (lat.ToString());
		sm.Params.Add (lon.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string tileString = data.value.Params[0];
			byte[] tile = Convert.FromBase64String(tileString);
			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			// do whatever you want with texture
			Map.GetComponent<Renderer>().material.mainTexture = texture;

		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
