using System;
using System.Collections;

[Serializable]
public class TowerData : BaseObjectData{
	public string PlayerKey;
	public string Id;

	public int Type = -1;

	public double Range = 0.0;
	public double Lat = 0.0;
	public double Lon = 0.0;
	public double Level = 0.0;
	public double Health = 100.0;
}