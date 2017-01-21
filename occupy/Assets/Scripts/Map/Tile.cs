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

	private Renderer renderer;
	public Tile(){}
	public void Start(){
		this.renderer = GetComponent<Renderer> ();
	}

	public void AddTower(GameObject tower){
		if(tower != null)
			towers.Add (tower);
	}

	public void initWithLatLon(Location location,Action<int,int> onComplete){
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "getTile";
		sm.Params.Add (location.Latitude.ToString());
		sm.Params.Add (location.Longitude.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			//load tile info
			string tileDataStr = data.value.Params[0];
			var tileData = JsonUtility.FromJson<TileData>(tileDataStr);

			byte[] tile = Convert.FromBase64String(tileData.ImageBytes);

			Center.Latitude = tileData.CenterLat;
			Center.Longitude = tileData.CenterLon;
			North = tileData.North;
			East = tileData.East;
			South = tileData.South;
			West = tileData.West;

			TileX = tileData.PositionX;
			TileY = tileData.PositionY;
		
			//set tile texture
			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			renderer.material.mainTexture = texture;

			//load towers on this tile
			foreach(var towerData in tileData.towers){
				var go = TowerManager.Current.CreateTower(towerData,this);
				AddTower(go);
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
			string tileDataStr = data.value.Params[0];
			var tileData = JsonUtility.FromJson<TileData>(tileDataStr);

			byte[] tile = Convert.FromBase64String(tileData.ImageBytes);

			Center.Latitude = tileData.CenterLat;
			Center.Longitude = tileData.CenterLon;
			North = tileData.North;
			East = tileData.East;
			South = tileData.South;
			West = tileData.West;
			//set tile texture
			Texture2D texture = new Texture2D(256, 256);
			texture.LoadImage(tile);
			renderer.material.mainTexture = texture;

			//load towers on this tile
			foreach(var towerData in tileData.towers){
				var go = TowerManager.Current.CreateTower(towerData,this);
				AddTower(go);
			}

			DataLoaded = true;
		});
	}
}
