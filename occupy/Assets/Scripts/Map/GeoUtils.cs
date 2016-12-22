using UnityEngine;
using System.Collections;

public static class GeoUtils
{
	public static Vector3 LocationToXYZ (Tile tile, Location location)
	{
		Vector3 tileSize = MapManager.Current.TileSize;
		//find 1 unit in Unity is how many in Lat/Lon
		//float alphaX = tileSize.x / (tile.East - tile.West);
		//float alphaZ = tileSize.z / (tile.North - tile.South);

		//find how far is the location from Down-Left of tile
		float x = location.Longitude - tile.West;
		float z = location.Latitude - tile.South;

		//convert x,z position of location to unity position
		x *= MapManager.Current.TileAlphaX;
		z *= MapManager.Current.TileAlphaY;

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

		lon /= MapManager.Current.TileAlphaX;
		lat /= MapManager.Current.TileAlphaY;

		lon += tile.West;
		lat += tile.South;

		return new Location (lat, lon);
	}
}
