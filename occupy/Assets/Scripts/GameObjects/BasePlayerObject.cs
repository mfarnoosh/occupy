using UnityEngine;
using System.Collections;
using System;

namespace GameObjects
{
	public abstract class BasePlayerObject : BaseObject
	{
		/* TODO: calc by game object level
		protected double power = 0.0;
		protected double powerFactor = 0.0;
		protected double range = 0;
		*/
		/* TODO: Later load from server
		protected boolean isInWar = false;
		protected boolean isRangeAttack = false;
		protected boolean needAttention = false;
		*/
		public string PlayerKey;
	}
}