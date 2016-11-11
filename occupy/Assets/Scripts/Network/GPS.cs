using UnityEngine;
using System.Collections;
using System;
public class GPS : MonoBehaviour {
	private float lat = 0;
	private float lon = 0;
	private float accuracy = 0;
	private float alt = 0;
	private double timeStamp = 0;
	// Use this for initialization
	IEnumerator Start () {
		// First, check if user has location service enabled
		long d = new DateTime (1970, 1, 1).Ticks;
		if (!Input.location.isEnabledByUser)
			yield break;
		
		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}




	}
	
	// Update is called once per frame
	void Update () {
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
		}
		else
		{
			// Access granted and location value could be retrieved
			if (lat != Input.location.lastData.latitude || lon != Input.location.lastData.longitude) {
				lat = Input.location.lastData.latitude;  
				lon = Input.location.lastData.longitude;
				alt = Input.location.lastData.altitude;
				accuracy = Input.location.lastData.horizontalAccuracy;
				timeStamp = Input.location.lastData.timestamp;
				SocketMessage sm = new SocketMessage ();
				sm.Cmd = "updateLoc";
				sm.Params.Add (lat.ToString());
				sm.Params.Add (lon.ToString());
				sm.Params.Add (alt.ToString());
				sm.Params.Add (accuracy.ToString());
				sm.Params.Add (timeStamp.ToString());
				new ClientManager().SendToServer (sm).OnSuccess((data)=>{});
			}

		}
	}
}
