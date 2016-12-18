using UnityEngine;
using System.Collections.Generic;
using System;

public class Tile : MonoBehaviour
{
	private Location _center = new Location(35.72083f,51.44816f);
	private List<GameObject> towers = new List<GameObject>();
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
			//load tile info
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

			//set tile texture
			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			GetComponent<Renderer>().material.mainTexture = texture;

			//load towers on this tile
			//TODO: Farnoosh - load and create towers
			int towerNumbers = int.Parse(data.value.Params[9]);

			for(int i=0;i < towerNumbers; i++){
				string towersStr = data.value.Params[i + 10];
				var towerData = JsonUtility.FromJson<TowerData>(towersStr);

				var go = TowerManager.Current.CreateTower(towerData,this);
				if(go != null){
					towers.Add(go);
				}
			}
			//complete actions
			DataLoaded = true;
			onComplete(TileX,TileY);
		}).OnError((callback) => {
			Debug.Log("Exception occured in getTile: " + callback.error.Message);
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
			foreach(var tower in towers)
			{
				Destroy(tower);
			}
			string tileString = data.value.Params[0];
			byte[] tile = Convert.FromBase64String(tileString);

			Center.Latitude = float.Parse(data.value.Params[1]);
			Center.Longitude = float.Parse(data.value.Params[2]);

			North = float.Parse(data.value.Params[3]);
			East = float.Parse(data.value.Params[4]);
			South = float.Parse(data.value.Params[5]);
			West = float.Parse(data.value.Params[6]);

			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			// do whatever you want with texture
			GetComponent<Renderer>().material.mainTexture = texture;

			//load towers on this tile
			//TODO: Farnoosh - load and create towers
			int towerNumbers = int.Parse(data.value.Params[7]);
			for(int i=0;i < towerNumbers; i++){
				string towersStr = data.value.Params[i + 8];
				var towerData = JsonUtility.FromJson<TowerData>(towersStr);
			
				var go = TowerManager.Current.CreateTower(towerData,this);
				if(go != null){
					towers.Add(go);
				}
			}

			DataLoaded = true;
		});
	}
}
