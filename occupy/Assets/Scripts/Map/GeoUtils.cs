using UnityEngine;
using System.Collections;

public static class GeoUtils {

	private static float EarthRadius = 6371000f;
	public static Vector3 LocationToXYZ (Tile tile,Location location){
		/*
		float finalLat = location.Latitude - tile.Center.Latitude;
		float finalLon = location.Longitude - tile.Center.Longitude;
		Vector3 qq = Quaternion.AngleAxis (finalLon, -Vector3.up) * Quaternion.AngleAxis (finalLat, -Vector3.right) * new Vector3(0,0,1);
		//These are just some Random numbers for zoom level 16 !!!
		//Other zoom level should be found new numbers
		//return new Vector3(qq.x * EarthRadius * 0.42379532f + 128, 0, qq.y * EarthRadius * 0.51797206f + 128);
		*/
		double lat = location.Latitude - tile.Center.Latitude;
		double lon = location.Longitude - tile.Center.Longitude;

		float lat2 = Mathf.PI * (float)lat / 180;
		float lon2 = Mathf.PI * (float)lon / 180;

		float x = EarthRadius * Mathf.Cos (lat2) * Mathf.Sin (lon2);
		float z = EarthRadius * Mathf.Sin (lat2) * Mathf.Cos (lon2);


		return new Vector3 (
			-(-x * 0.42379532f + (MapManager.Current.TileSize.x / 2)),
			0, 
			z * 0.51797206f + (MapManager.Current.TileSize.z /2));
	}
	public static Location XYZToLocation (Tile tile,Vector3 position){
		float x = -((-(position.x) - (MapManager.Current.TileSize.x / 2)) / 0.42379532f);
		float z = (position.z - (MapManager.Current.TileSize.z / 2)) / 0.51797206f;

		float lat = Mathf.Asin (z / EarthRadius);
		float lon = Mathf.Atan2 (z , x);

		lon = lon * 180 / Mathf.PI;

		return PlayerManager.Current.WorldCenter;
	}
}
