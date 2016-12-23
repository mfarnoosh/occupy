using UnityEngine;
using System.Collections;
using System;

namespace GameObjects{
	public class Tower : BasePlayerObject {
		/// <summary>
		/// Sentry(1),MachineGun(2),RocketLauncher(3),AntiAircraft(4),Stealth(5)
		/// </summary>
		public int Type = -1;
		public double Range;

		public Tower(TowerData data){
			FromObjectData (data);
		}
		public void Start(){
			//FromObjectData(TowerManager.Current.GetTowerData (Type, TowerManager.Current.GetMaxTowerLevel (Type)));
			//Physics.IgnoreCollision(MapManager.Current.MapObject.GetComponent<>);
		}

		public override void FromObjectData(BaseObjectData data){
			if(data is TowerData){
				var td = data as TowerData;

				PlayerKey = td.PlayerKey;
				Id = td.Id;
				Type = td.Type;
				Range = td.Range;
				Lat = td.Lat;
				Lon = td.Lon;
				Level = td.Level;
				Health = td.Health;
			}
		}
		public override BaseObjectData ToObjectData(){
			TowerData td = new TowerData ();
			td.PlayerKey = PlayerKey;
			td.Id = Id;
			td.Type = Type;
			td.Range = Range;
			td.Lat = Lat;
			td.Lon = Lon;
			td.Level = Level;
			td.Health = Health;
			return td;
		}
	}
}