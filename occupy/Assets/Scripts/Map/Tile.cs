using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour
{
	private Location _center = new Location(35.72083f,51.44816f);

	public int _tileX;
	public int _tileY;

	public Location Center { get { return _center; } set { _center = value; } }
	public float North;
	public float East;
	public float South;
	public float West;
	public Location BottomRight { get { return _center; } set { _center = value; } }

	public int TileX { 
		get { return _tileX; } 
		set { 
			if (value != _tileX) {
				GetComponent<Renderer>().material.mainTexture = null;
				DataLoaded = false;
			}
			_tileX = value; 
		} 
	}

	public int TileY { 
		get { return _tileY; } 
		set { 
			if (value != _tileY) {
				GetComponent<Renderer>().material.mainTexture = null;
				DataLoaded = false;
			}
			_tileY = value; 
		} 
	}

	public bool DataLoaded = false;
	public Tile(){}

	public void initWithLatLon(Location location,Action<int,int> onComplete){
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "getTile";
		sm.Params.Add (location.Latitude.ToString());
		sm.Params.Add (location.Longitude.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string tileString = data.value.Params[0];
			byte[] tile = Convert.FromBase64String(tileString);

			Center.Latitude = float.Parse(data.value.Params[1]);
			Center.Longitude = float.Parse(data.value.Params[2]);
			North = float.Parse(data.value.Params[3]);
			East = float.Parse(data.value.Params[4]);
			South = float.Parse(data.value.Params[5]);
			West = float.Parse(data.value.Params[6]);

			TileX = int.Parse(data.value.Params[7]);
			TileY = int.Parse(data.value.Params[8]);

			//TODO: Farnoosh - load and create buildings
			//CreateBuilding(float.Parse(data.value.Params[3]),float.Parse(data.value.Params[4]),Color.red);


			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			// do whatever you want with texture
			GetComponent<Renderer>().material.mainTexture = texture;
			DataLoaded = true;
			onComplete(TileX,TileY);
		});
	}

	public void LoadTileByXY(){
		if (TileX <= 0 || TileY <= 0) {
			Debug.LogError ("Error in loading tile.");
			return;
		}
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "getTileByNumber";
		sm.Params.Add (TileX.ToString());
		sm.Params.Add (TileY.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string tileString = data.value.Params[0];
			byte[] tile = Convert.FromBase64String(tileString);

			Center.Latitude = float.Parse(data.value.Params[1]);
			Center.Longitude = float.Parse(data.value.Params[2]);

			North = float.Parse(data.value.Params[3]);
			East = float.Parse(data.value.Params[4]);
			South = float.Parse(data.value.Params[5]);
			West = float.Parse(data.value.Params[6]);

			//TODO: Farnoosh - load and create buildings
			//CreateBuilding(float.Parse(data.value.Params[3]),float.Parse(data.value.Params[4]),Color.red);


			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			// do whatever you want with texture
			GetComponent<Renderer>().material.mainTexture = texture;
			DataLoaded = true;
		});
	}

}
