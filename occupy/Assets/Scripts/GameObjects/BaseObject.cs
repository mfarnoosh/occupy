using UnityEngine;
using System.Collections;
using System;

namespace GameObjects{
	public abstract class BaseObject : MonoBehaviour {
		public string Id;
		public double Lat;
		public double Lon;
		public double Level = 0.0;
		public double Health = 100.0;
		//protected double defenceFactor = 0.0; ==> calc by game object level

		public abstract void FromObjectData(BaseObjectData data);
		public abstract BaseObjectData ToObjectData();
	}
}