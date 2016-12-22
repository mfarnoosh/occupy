using System;
using System.Collections;

[Serializable]
public class BuildingData : BaseObjectData{
	public string Id;
	public int Type = -1;
	public double Lat = 0.0;
	public double Lon = 0.0;
	public double Level = 0.0;
	public double Health = 100.0;
}
