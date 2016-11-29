using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour
{

	//TODO: Farnoosh
	/*
	 * http://tile.mapzen.com/mapzen/vector/v1/{layers}/{z}/{x}/{y}.{format}?api_key={api_key}
	 * mapzen-etTgDC5
	 * 35.72135,51.44947
	 */
	public GameObject TilePrefab;
	public GameObject MapObject;
	public Vector3 WorldCenter = new Vector3 (0, 0, 0);


	private Tile[,] tiles = new Tile[3, 3];

	public Vector3 TileSize;
	private bool InitCenterFinished = false;
	private static float moveSpeed = 20.0f;
	public static MapManager Current;

	public MapManager ()
	{
		Current = this;
	}

	public void MoveMap (Vector3 deltaPosition, float sharpness)
	{
		MapObject.transform.position += new Vector3 (deltaPosition.x * moveSpeed, 0, deltaPosition.z * moveSpeed * 2);
		WorldCenter = MapObject.transform.position;
		AdjustTiles ();
	}

	public void Start ()
	{
		TileSize = TilePrefab.GetComponent<Renderer> ().bounds.size;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				var go = GameObject.Instantiate (TilePrefab);
				go.transform.parent = MapObject.transform;
				go.transform.position = new Vector3 ((i - 1) * TileSize.x, 0, (1 - j) * TileSize.z);
				go.transform.name = String.Format ("Tile({0},{1})", i, j);
				tiles [i, j] = go.GetComponent<Tile> ();
			}
		}
		tiles [1, 1].initWithLatLon (PlayerManager.Current.WorldCenter, (x, y) => {
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					if (i == 1 && j == 1)
						continue;
					tiles [i, j].TileX = x + (i - 1);
					tiles [i, j].TileY = y + (j - 1);
					tiles [i, j].LoadTileByXY ();
				}
			}
			InitCenterFinished = true;
		});

	}
	public Tile GetTile(Vector3 targetPosition){
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
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

	#region Move Tiles
	private void AdjustTiles(){
		if (!InitCenterFinished)
			return;
		if (tiles [1, 1].transform.position.x < -1 * TileSize.x / 2) {
			LoadRightTiles ();
		}
		if (tiles [1, 1].transform.position.x > 1 * TileSize.x / 2) {
			LoadLeftTiles ();
		}
		if (tiles [1, 1].transform.position.z < -1 * TileSize.z / 2) {
			LoadUpTiles ();
		}
		if (tiles [1, 1].transform.position.z > 1 * TileSize.z / 2) {
			LoadDownTiles ();
		}
	}
	private void LoadRightTiles(){
		for (int i = 0; i < 3; i++) {
			Tile t = tiles [0, i];

			tiles [0, i] = tiles [1, i];
			tiles [0, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 0, i);

			tiles [1 , i] = tiles [2, i];
			tiles [1, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 1, i);

			t.TileX = tiles [1, i].TileX + 1;
			var newPos = new Vector3 (tiles[1,i].gameObject.transform.position.x + TileSize.x,t.gameObject.transform.position.y,t.gameObject.transform.position.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", 2, i);
			tiles [2, i] = t;
			t.LoadTileByXY ();
		}
	}
	private void LoadLeftTiles(){
		for (int i = 0; i < 3; i++) {
			Tile t = tiles [2, i];

			tiles [2, i] = tiles [1, i];
			tiles [2, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 2, i);

			tiles [1 , i] = tiles [0, i];
			tiles [1, i].gameObject.transform.name = String.Format ("Tile({0},{1})", 1, i);

			t.TileX = tiles [1, i].TileX - 1;
			var newPos = new Vector3 (tiles[1,i].gameObject.transform.position.x - TileSize.x,t.gameObject.transform.position.y,t.gameObject.transform.position.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", 0, i);
			tiles [0, i] = t;
			t.LoadTileByXY ();
		}
	}
	private void LoadDownTiles(){
		for (int i = 0; i < 3; i++) {
			Tile t = tiles [i, 0];

			tiles [i, 0] = tiles [i, 1];
			tiles [i, 0].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 0);

			tiles [i, 1] = tiles [i, 2];
			tiles [i, 1].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 1);

			t.TileY = tiles [i, 1].TileY + 1;
			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, 1].gameObject.transform.position.z - TileSize.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, 2);
			tiles [i, 2] = t;
			t.LoadTileByXY ();
		}
	}
	private void LoadUpTiles(){
		for (int i = 0; i < 3; i++) {
			Tile t = tiles [i, 2];

			tiles [i, 2] = tiles [i, 1];
			tiles [i, 2].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 2);

			tiles [i, 1] = tiles [i, 0];
			tiles [i, 1].gameObject.transform.name = String.Format ("Tile({0},{1})", i, 1);

			t.TileY = tiles [i, 1].TileY - 1;
			var newPos = new Vector3 (t.gameObject.transform.position.x, t.gameObject.transform.position.y, tiles [i, 1].gameObject.transform.position.z + TileSize.z);
			t.gameObject.transform.position = newPos;
			t.gameObject.transform.name = String.Format ("Tile({0},{1})", i, 0);
			tiles [i, 0] = t;
			t.LoadTileByXY ();
		}
	}
	#endregion
}
