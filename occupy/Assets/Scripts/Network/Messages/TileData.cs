using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TileData : BaseObjectData{
	public String ImageBytes;

	public float CenterLat;
	public float CenterLon;

	public float North;
	public float East;
	public float South;
	public float West;

	public int PositionX;
	public int PositionY;

	public List<TowerData> towers;
}
