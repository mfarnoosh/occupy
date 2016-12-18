using UnityEngine;
using System.Collections;
using System;

namespace GameObjects
{
	public class Building : BaseObject
	{
		/// <summary>
		/// Bank(1),Mosque(2),Park(3)
		/// </summary>
		int type;

		public override void FromObjectData(BaseObjectData data){
		}
		public override BaseObjectData ToObjectData(){
			return null;
		}
	}
}
[Serializable]
public class BuildingData : BaseObjectData{
	public string Id;
	public int Type = -1;
	public double Lat = 0.0;
	public double Lon = 0.0;
	public double Level = 0.0;
	public double Health = 100.0;
}