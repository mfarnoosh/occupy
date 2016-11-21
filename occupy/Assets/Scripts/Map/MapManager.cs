using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class MapManager : MonoBehaviour {

	//TODO: Farnoosh
	/*
	 * http://tile.mapzen.com/mapzen/vector/v1/{layers}/{z}/{x}/{y}.{format}?api_key={api_key}
	 * mapzen-etTgDC5
	 * 35.72135,51.44947
	 */

	public static MapManager Current;
	public MapManager(){
		Current = this;
	}
		
	public void MoveMap(Vector2 deltaPosition,float sharpness){
		
	}
		
	void Start(){
		Vector2 realPos = new Vector2 (9685, 6207);
		Vector2 worldCenter = new Vector2 (0,0);
		//CreateTile (realPos, worldCenter, 14);
	}
	//public IEnumerator CreateTile(World w, Vector2 realPos, Vector2 worldCenter, int zoom){
//	public IEnumerator CreateTile(Vector2 realPos, Vector2 worldCenter, int zoom){
//		var tilename = realPos.x + "_" + realPos.y;
//		var tileurl = realPos.x + "/" + realPos.y;
//		var url = "http://vector.mapzen.com/osm/water,earth,buildings,roads,landuse/" + zoom + "/";
//
//		JSONObject mapData;
//		if (File.Exists(tilename))
//		{
//			var r = new StreamReader(tilename, Encoding.Default);
//			mapData = new JSONObject(r.ReadToEnd());
//		}
//		else
//		{
//			var www = new WWW(url + tileurl + ".json");
//			yield return www;
//
//			var sr = File.CreateText(tilename);
//			sr.Write(www.text);
//			sr.Close();
//			mapData = new JSONObject(www.text);
//		}
//		var rect = GeoConvertor.TileBounds(realPos.ToVector2d(), zoom);
//		CreateBuildings(mapData["buildings"]);
//		CreateRoads(mapData["roads"]);
//	}
//
//	private void CreateBuildings(JSONObject mapData)
//	{
//		foreach (var geo in mapData["features"].list.Where(x => x["geometry"]["type"].str == "Polygon"))
//		{
//			var l = new List<Vector3>();
//			for (int i = 0; i < geo["geometry"]["coordinates"][0].list.Count - 1; i++)
//			{
//				var c = geo["geometry"]["coordinates"][0].list[i];
//				var bm = GeoConvertor.LatLonToMeters(c[1].f, c[0].f);
//				var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
//				l.Add(pm.ToVector3xz());
//			}
//
//			try
//			{
//				var center = l.Aggregate((acc, cur) => acc + cur) / l.Count;
//				if (!BuildingDictionary.ContainsKey(center))
//				{
//					var bh = new BuildingHolder(center, l);
//					for (int i = 0; i < l.Count; i++)
//					{
//						l[i] = l[i] - bh.Center;
//					}
//					BuildingDictionary.Add(center, bh);
//
//					var m = bh.CreateModel();
//					m.name = "building";
//					m.transform.parent = this.transform;
//					m.transform.localPosition = center;
//				}
//			}
//			catch (Exception ex)
//			{
//				Debug.Log(ex);
//			}
//		}
//	}
//	private void CreateRoads(JSONObject mapData)
//	{
//		foreach (var geo in mapData["features"].list)
//		{
//			var l = new List<Vector3>();
//
//			for (int i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
//			{
//				var c = geo["geometry"]["coordinates"][i];
//				var bm = GeoConvertor.LatLonToMeters(c[1].f, c[0].f);
//				var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
//				l.Add(pm.ToVector3xz());
//			}
//
//			var m = new GameObject("road").AddComponent<RoadPolygon>();
//			m.transform.parent = this.transform;
//			try
//			{
//				m.Initialize(geo["id"].str, this, l, geo["properties"]["kind"].str);
//			}
//			catch (Exception ex)
//			{
//				Debug.Log(ex);
//			}
//		}
//	}
}
