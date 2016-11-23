using UnityEngine;
using System.Collections;

public class Location{

	private float _latitude = 35.72083f;
	private float _longitude = 51.44816f;

	public float Latitude { get { return _latitude; } set { _latitude = value; } }

	public float Longitude { get { return _longitude; } set { _longitude = value; } }

	public Location(float latitude,float longitude){
		Latitude = latitude;
		Longitude = longitude;
	}
}
