using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour
{
	private static int tilesGrid = 5; //means e.g. grid 3 X 3 of tiles
	private static float moveSpeed = 20.0f;
	private Vector3 _tileSize;
	//TODO: Farnoosh
	/*
	 * http://tile.mapzen.com/mapzen/vector/v1/{layers}/{z}/{x}/{y}.{format}?api_key={api_key}
	 * mapzen-etTgDC5
	 * 35.72135,51.44947
	 */
	public GameObject TilePrefab;
	public GameObject MapObject;

	private Tile[,] tiles = new Tile[tilesGrid, tilesGrid];


	public Vector3 TileSize{get { return _tileSize;}}
	private bool InitCenterFinished = false;

	public static MapManager Current;
	public MapManager ()
	{
		Current = this;
	}
	public void Start ()
	{
		_tileSize = TilePrefab.GetComponent<Renderer> ().bounds.size;
		MoveTo(PlayerManager.Current.WorldCenter);
	}

	public void MoveMap (Vector3 deltaPosition, float sharpness)
	{
		MapObject.transform.position += new Vector3 (deltaPosition.x * moveSpeed, 0, deltaPosition.z * moveSpeed * 2);
		AdjustTiles ();
	}
	public void MoveTo(Location newLocation){
		InitCenterFinished = false;

		//(tilesGrid - 1) / 2 ==> center tile
		int centerTileX = (tilesGrid - 1) / 2;


		for (int i = 0; i < tilesGrid; i++) {
			for (int j = 0; j < tilesGrid; j++) {
				if(tiles[i,j] != null && tiles[i,j].transform.gameObject != null)
					Destroy (tiles [i, j].transform.gameObject);

				var go = GameObject.Instantiate (TilePrefab);
				go.transform.parent = MapObject.transform;
				go.transform.position = new Vector3 ((i - centerTileX) * TileSize.x, 0, (centerTileX - j) * TileSize.z);
				go.transform.name = String.Format ("Tile({0},{1})", i, j);
				tiles [i, j] = go.GetComponent<Tile> ();
			}
		}
		tiles [centerTileX, centerTileX].initWithLatLon (newLocation, (x, y) => {
			for (int i = 0; i < tilesGrid; i++) {
				for (int j = 0; j < tilesGrid; j++) {
					if (i == centerTileX && j == centerTileX)
						continue;
					tiles [i, j].TileX = x + (i - centerTileX);
					tiles [i, j].TileY = y + (j - centerTileX);
					tiles [i, j].LoadTileByXY ();
				}
			}
			InitCenterFinished = true;
		});
	}
	public Tile GetTile(Vector3 targetPosition){
		for (int i = 0; i < tilesGrid; i++) {
			for (int j = 0; j < tilesGrid; j++) {
				if (targetPosition.x >= (tiles [i, j].transform.position.x - TileSize.x / 2) &&
					targetPosition.x <= (tiles [i, j].transform.position.x + TileSize.x / 2) &&
					targetPosition.z >= (tiles [i, j].transform.position.z - TileSize.z / 2) &&
					targetPosition.z <= (tiles [i, j].transform.position.z + TileSize.z / 2)) {
					return tiles [i, j];
				}
			}
		}
		return null;
	}

	public bool CanPlaceBuildingHere(GameObject go){
		//		var verts = go.GetComponent<MeshFilter> ().mesh.vertices;
		//		var obstacles = GameObject.FindObjectsOfType<NavMeshObstacle> ();
		//		var cols = new List<Collider> ();
		//		foreach (var o in obstacles) {
		//			if (o.gameObject != go) {
		//				cols.Add (o.gameObject.GetComponent<Collider> ());
		//			}
		//		}
		//
		//		foreach (var v in verts) {
		//			NavMeshHit hit;
		//			var vReal = go.transform.TransformPoint (v);
		//			NavMesh.SamplePosition( vReal, out hit, 0.5f, NavMesh.AllAreas);
		//
		//			bool onXAxis = Mathf.Abs (hit.position.x - vReal.x) < 2f;
		//			bool onZAxis = Mathf.Abs (hit.position.z - vReal.z) < 0.5f;
		//			bool hitCollider = cols.Any (c => c.bounds.Contains (vReal));
		//
		//			Debug.Log (vReal.x  + "|" + Mathf.Abs (hit.position.z - vReal.z) + "|" + hitCollider);
		//
		//
		//			if (!onXAxis || !onZAxis || hitCollider)
		//				return false;
		//		}


		return true;
	}

	public Vector3? ScreenPointToMapPosition(Vector2 point){
		var ray = Camera.main.ScreenPointToRay (point);
		RaycastHit hit;

		foreach (var item in tiles) {
			if (item.GetComponent<Collider> ().Raycast (ray, out hit, Mathf.Infinity)) {
				return new Vector3 (hit.point.x, 0.01f, hit.point.z);
			}
		}
		return null;
//		if (!MapManager.Current.MapObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
//			return null;
//		//		float finalYPosition = MapCollider.transform.position.y + MapCollider.transform.localScale.z;
//		return new Vector3 (hit.point.x, 0.01f,hit.point.z);

	}

	#region Move Tiles
	private void AdjustTiles(){
		if (!InitCenterFinished)
			return;
		int centerTileX = (tilesGrid - 1) / 2;

		if (tiles [centerTileX, centerTileX].transform.position.x < -1 * TileSize.x / 2) {
			LoadRightTiles ();
		}
		if (tiles [centerTileX, centerTileX].transform.position.x > 1 * TileSize.x / 2) {
			LoadLeftTiles ();
		}
		if (tiles [centerTileX, centerTileX].transform.position.z < -1 * TileSize.z / 2) {
			LoadUpTiles ();
		}
		if (tiles [centerTileX, centerTileX].transform.position.z > 1 * TileSize.z / 2) {
			LoadDownTiles ();
		}
	}
	private void LoadRightTiles(){
		for (int i = 0; i < tilesGrid; i++) {
			Tile t = tiles [0, i];
			for (int j = 0; j < tilesGrid - 1; j++) {
				tiles [j, i] = tiles [j + 1, i];
				tiles [j, i].gameObject.transform.name = String.Format ("Tile({0},{1})", j, i);
			}

			t.TileX = tiles [tilesGrid - 2, i].TileX + 1;
			var newPos = new Vector3 (tiles[tilesGrid - 2,i].gameObject.transform.position.x + TileSize.x,t.gameObject.transform.position.y,t.gameObject.transform.position.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", tilesGrid - 1, i);
			tiles [tilesGrid - 1, i] = t;
			t.LoadTileByXY ();
		}
	}
	private void LoadLeftTiles(){
		for (int i = 0; i < tilesGrid; i++) {
			Tile t = tiles [tilesGrid - 1, i];

			for (int j = tilesGrid - 1; j > 0; j--) {
				tiles [j, i] = tiles [j - 1, i];
				tiles [j, i].gameObject.transform.name = String.Format ("Tile({0},{1})", j, i);
			}
				
			t.TileX = tiles [1, i].TileX - 1;
			var newPos = new Vector3 (tiles[1,i].gameObject.transform.position.x - TileSize.x,t.gameObject.transform.position.y,t.gameObject.transform.position.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", 0, i);
			tiles [0, i] = t;
			t.LoadTileByXY ();
		}

//		for (int i = 0; i < 3; i++) {
//			Tile t = tiles [2, i];
//
//			tiles [2, i] = tiles [1, i];
//			tiles [2, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 2, i);
//
//			tiles [1 , i] = tiles [0, i];
//			tiles [1, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 1, i);
//
//			t.TileX = tiles [1, i].TileX - 1;
//			var newPos = new Vector3 (tiles[1,i].gameObject.transform.position.x - TileSize.x,t.gameObject.transform.position.y,t.gameObject.transform.position.z);
//			t.gameObject.transform.position = newPos;
//			t.gameObject.transform.name = String.Format ("Tile({0},{1})", 0, i);
//			tiles [0, i] = t;
//			t.LoadTileByXY ();
//		}
	}
	private void LoadDownTiles(){
		for (int i = 0; i < tilesGrid; i++) {
			Tile t = tiles [i, 0];
			for (int j = 0; j < tilesGrid - 1; j++) {
				tiles [i, j] = tiles [i, j + 1];
				tiles [i, j].gameObject.transform.name = String.Format ("Tile({0},{1})", i, j);
			}

			t.TileY = tiles [i, tilesGrid - 2].TileY + 1;
			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, tilesGrid - 2].gameObject.transform.position.z - TileSize.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, tilesGrid - 1);
			tiles [i, tilesGrid - 1] = t;
			t.LoadTileByXY ();
		}
//		for (int i = 0; i < 3; i++) {
//			Tile t = tiles [i, 0];
//
//			tiles [i, 0] = tiles [i, 1];
//			tiles [i, 0].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 0);
//
//			tiles [i, 1] = tiles [i, 2];
//			tiles [i, 1].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 1);
//
//			t.TileY = tiles [i, 1].TileY + 1;
//			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, 1].gameObject.transform.position.z - TileSize.z);
//			t.gameObject.transform.position = newPos;
//			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, 2);
//			tiles [i, 2] = t;
//			t.LoadTileByXY ();
//		}
	}
	private void LoadUpTiles(){
		for (int i = 0; i < tilesGrid; i++) {
			Tile t = tiles [i, tilesGrid - 1];
			for (int j = tilesGrid - 1; j > 0; j--) {
				tiles [i, j] = tiles [i, j - 1];
				tiles [i, j].gameObject.transform.name = String.Format ("Tile({0},{1})", i, j);
			}

			t.TileY = tiles [i, 1].TileY - 1;
			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, 1].gameObject.transform.position.z + TileSize.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, 0);
			tiles [i, 0] = t;
			t.LoadTileByXY ();
		}
//		for (int i = 0; i < 3; i++) {
//			Tile t = tiles [i, 2];
//
//			tiles [i, 2] = tiles [i, 1];
//			tiles [i, 2].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 2);
//
//			tiles [i, 1] = tiles [i, 0];
//			tiles [i, 1].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 1);
//
//			t.TileY = tiles [i, 1].TileY - 1;
//			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, 1].gameObject.transform.position.z + TileSize.z);
//			t.gameObject.transform.position = newPos;
//			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, 0);
//			tiles [i, 0] = t;
//			t.LoadTileByXY ();
//		}
	}
	#endregion
}
