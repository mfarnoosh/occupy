using UnityEngine;
using System.Collections;
using System;
public class Test : MonoBehaviour {
	public GameObject Map;
	public GameObject testObject;
	// Use this for initialization

//	private static float initLat = 35.72083f;
//	private static float initLon = 51.44816f;

	private static float initLat = 35.70423f;
	private static float initLon = 51.40570f;

	private static float centerLat = 35.72083f;
	private static float centerLon = 51.44816f;
	void Start () {
		
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "getTile";
		sm.Params.Add (initLat.ToString());
		sm.Params.Add (initLon.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string tileString = data.value.Params[0];
			Debug.Log(data.value.GetType());
			Debug.Log(data.value.Params.Count);
			byte[] tile = Convert.FromBase64String(tileString);

			Test.centerLat = float.Parse(data.value.Params[1]);
			Test.centerLon = float.Parse(data.value.Params[2]);

			Debug.Log("Center: " + centerLat + " | " + centerLon);


			CreateBuilding(float.Parse(data.value.Params[3]),float.Parse(data.value.Params[4]),Color.red);
			CreateBuilding(float.Parse(data.value.Params[5]),float.Parse(data.value.Params[6]),Color.yellow);
			CreateBuilding(float.Parse(data.value.Params[7]),float.Parse(data.value.Params[8]),Color.blue);
			CreateBuilding(float.Parse(data.value.Params[9]),float.Parse(data.value.Params[10]),Color.green);
			CreateBuilding(float.Parse(data.value.Params[11]),float.Parse(data.value.Params[12]),Color.magenta);

			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			// do whatever you want with texture
			Map.GetComponent<Renderer>().material.mainTexture = texture;

		});
	}

	private void CreateBuilding(float latitude,float longitude,Color col){
		Debug.Log(latitude + " | " + longitude);

		var go = GameObject.Instantiate(testObject);
		go.transform.localScale = new Vector3(5,5,5);
		go.GetComponent<Renderer> ().material.color = col;
		float xPos = 0.0f;
		float zPos = 0.0f;
		//TODO: Convert Latitude,Longitude to X,Y
		var pos = LatLonToMeters(latitude,longitude);
		xPos = pos.x;
		zPos = pos.y;
		Debug.Log(xPos + " | " + zPos);
		go.transform.position = new Vector3(-xPos,0,zPos);
	}

	private const int EarthRadius = 6378137;
	private const float OriginShift = (float)(2 * Math.PI * EarthRadius / 2);
	public static Vector2 LatLonToMeters(float lat, float lon)
	{
		float finalLat = lat - Test.centerLat;
		float finalLon = lon - Test.centerLon;
		Vector3 qq = Quaternion.AngleAxis (finalLon, -Vector3.up) * Quaternion.AngleAxis (finalLat, -Vector3.right) * new Vector3(0,0,1);
		//These are just some Random numbers for zoom level 16 !!!
		//Other zoom level should be found new numbers
		return new Vector2(qq.x * 2700000 + 128,qq.y * 3300000 + 128);
	}
}
