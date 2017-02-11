using UnityEngine;
using System.Collections;
using System;
using UnityToolbag;
public class GPS : MonoBehaviour {
	public Location location = null;
	public bool dataReady = false;
	public IEnumerator GetLocation(Action<Location> callback) {		

		if (!Input.location.isEnabledByUser) {
			location = null;
			dataReady = true;
			print("not enabled");
			yield break;
		}
		print("starting...");
		Input.location.Start();
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			print("waiting...");
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			location = null;	
			dataReady = true;
			yield break;
		}
		if (Input.location.status == LocationServiceStatus.Failed) {
			print ("Unable to determine device location");
			location = null;	
			dataReady = true;
			yield break;
		} else {
			// Access granted and location value could be retrieved
			float lat = 0;
			float lon = 0;
			float accuracy = 0;
			float alt = 0;
			double timeStamp = 0;
			lat = Input.location.lastData.latitude;  
			lon = Input.location.lastData.longitude;
			alt = Input.location.lastData.altitude;
			accuracy = Input.location.lastData.horizontalAccuracy;
			timeStamp = Input.location.lastData.timestamp;
			print("lat = " + lat + " lon = " + lon);
			location = new Location(lat, lon);
			dataReady = true;
			callback (location);
			yield break;

		}			
		location = null;		
		dataReady = true;
		yield break;
	}

}
