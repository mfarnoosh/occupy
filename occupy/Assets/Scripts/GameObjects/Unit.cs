using UnityEngine;
using System.Collections;

namespace GameObjects{
	public class Unit : BasePlayerObject {
		/// <summary>
		/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
		/// </summary>
		int type;

		public override void FromObjectData(BaseObjectData data){
		}
		public override BaseObjectData ToObjectData(){
			return null;
		}
	}
}