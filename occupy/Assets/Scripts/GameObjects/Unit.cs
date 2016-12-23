using UnityEngine;
using System.Collections;
using System;

namespace GameObjects{
	public class Unit : BasePlayerObject {
		/// <summary>
		/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
		/// </summary>
		public int Type;
		public double Range;
		public override void FromObjectData(BaseObjectData data){
			if(data is UnitData){
				var ud = data as UnitData;

				PlayerKey = ud.PlayerKey;
				Id = ud.Id;
				Type = ud.Type;
				Range = ud.Range;
				Lat = ud.Lat;
				Lon = ud.Lon;
				Level = ud.Level;
				Health = ud.Health;
			}
		}
		public override BaseObjectData ToObjectData(){
			UnitData ud = new UnitData ();
			ud.PlayerKey = PlayerKey;
			ud.Id = Id;
			ud.Type = Type;
			ud.Range = Range;
			ud.Lat = Lat;
			ud.Lon = Lon;
			ud.Level = Level;
			ud.Health = Health;
			return ud;
		}
	}
}
