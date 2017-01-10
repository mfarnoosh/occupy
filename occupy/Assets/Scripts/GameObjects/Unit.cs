using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
	/// <summary>
	/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
	/// </summary>
	public int type = -1;
	public String playerKey;
	public String id;
	public int level = 0;
	public float currentHitPoint = 0.0f;
	public Location location;
	public bool isMoving = false;
	public bool isAttacking = false;
	public bool isUpgrading = false;

	private UnitConfigData configData = null;

	void Awake ()
	{
		configData = UnitManager.Current.GetUnitConfig (type, level);
	}
}
