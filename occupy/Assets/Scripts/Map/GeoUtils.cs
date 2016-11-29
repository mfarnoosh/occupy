using UnityEngine;
using System.Collections;

public static class GeoUtils {

	private static float EarthRadius = 500;
	public static Vector3 LocationToXYZ (Tile tile,Location location){
		/*
		float finalLat = location.Latitude - tile.Center.Latitude;
		float finalLon = location.Longitude - tile.Center.Longitude;
		Vector3 qq = Quaternion.AngleAxis (finalLon, -Vector3.up) * Quaternion.AngleAxis (finalLat, -Vector3.right) * new Vector3(0,0,1);
		//These are just some Random numbers for zoom level 16 !!!
		//Other zoom level should be found new numbers
		//return new Vector3(qq.x * EarthRadius * 0.42379532f + 128, 0, qq.y * EarthRadius * 0.51797206f + 128);
		*/




		//float x = EarthRadius * Mathf.Cos (lat2) * Mathf.Sin (lon2);
		//float z = EarthRadius * Mathf.Sin (lat2) * Mathf.Cos (lon2);



		float alphaX = 256 / (tile.East - tile.West);
		float alphaY = 256 / (tile.North - tile.South);

//		float alphaLat = 256 / (tile.East - tile.West);
//		float alphaLon = 256 / (tile.North - tile.South);

//		return new Vector3 (
//			-(-x * 0.42379532f + (MapManager.Current.TileSize.x / 2)),
//			0, 
//			z * 0.51797206f + (MapManager.Current.TileSize.z /2));
		return new Vector3 (
			((location.Longitude - tile.West) * alphaX) - (MapManager.Current.TileSize.x / 2),
			0, 
			((location.Latitude - tile.South) * alphaY) - (MapManager.Current.TileSize.z / 2));
	}
	public static Vector3 LocationToCartesian(Location loc){
		float lat = loc.Latitude;
		float lon = loc.Longitude;

		float cosLat = Mathf.Cos(lat * Mathf.PI / 180.0f);
		float sinLat = Mathf.Sin(lat * Mathf.PI / 180.0f);
		float cosLon = Mathf.Cos(lon * Mathf.PI / 180.0f);
		float sinLon = Mathf.Sin(lon * Mathf.PI / 180.0f);
		float rad = EarthRadius;
		float f = 1.0f / 298.257224f;
		float C = 1.0f / Mathf.Sqrt(cosLat * cosLat + (1f - f) * (1f - f) * sinLat * sinLat);
		float S = (1.0f - f) * (1.0f - f) * C;
		float h = 0.0f;
		float x = (rad * C + h) * cosLat * cosLon;
		float y = (rad * C + h) * cosLat * sinLon;
		float z = (rad * S + h) * sinLat;

		return new Vector3 (x, y, z);
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
