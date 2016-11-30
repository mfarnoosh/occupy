using UnityEngine;
using System.Collections;

public static class GeoUtils
{
	public static Vector3 LocationToXYZ (Tile tile, Location location)
	{
	/*
		float finalLat = location.Latitude - tile.Center.Latitude;
		float finalLon = location.Longitude - tile.Center.Longitude;
		Vector3 qq = Quaternion.AngleAxis (finalLon, -Vector3.up) * Quaternion.AngleAxis (finalLat, -Vector3.right) * new Vector3(0,0,1);
		//These are just some Random numbers for zoom level 16 !!!
		//Other zoom level should be found new numbers
		//return new Vector3(qq.x * EarthRadius * 0.42379532f + 128, 0, qq.y * EarthRadius * 0.51797206f + 128);


		//float x = EarthRadius * Mathf.Cos (lat2) * Mathf.Sin (lon2);
		//float z = EarthRadius * Mathf.Sin (lat2) * Mathf.Cos (lon2);
	*/


		Vector3 tileSize = MapManager.Current.TileSize;
		//find 1 unit in Unity is how many in Lat/Lon
		float alphaX = tileSize.x / (tile.East - tile.West);
		float alphaZ = tileSize.z / (tile.North - tile.South);

		//find how far is the location from Down-Left of tile
		float x = location.Longitude - tile.West;
		float z = location.Latitude - tile.South;

		//convert x,z position of location to unity position
		x *= alphaX;
		z *= alphaZ;

		//move to position from down-left to center !!!
		x -= tileSize.x / 2;
		z -= tileSize.z / 2;

		//consider the movement of map
		x += tile.transform.position.x;
		z += tile.transform.position.z;

		return new Vector3 (x, 0, z);
	}

	public static Location XYZToLocation (Tile tile, Vector3 position)
	{
		Vector3 tileSize = MapManager.Current.TileSize;

		float lon = position.x;
		float lat = position.z;

		lon -= tile.transform.position.x;
		lat -= tile.transform.position.z;

		lon += tileSize.x / 2;
		lat += tileSize.z / 2;

		float alphaX = tileSize.x / (tile.East - tile.West);
		float alphaZ = tileSize.z / (tile.North - tile.South);

		lon /= alphaX;
		lat /= alphaZ;

		lon += tile.West;
		lat += tile.South;

		return new Location (lat, lon);
	}
}
