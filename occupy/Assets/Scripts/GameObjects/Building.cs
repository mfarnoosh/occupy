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