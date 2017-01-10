using UnityEngine;
using System.Collections;
using System;

public class Tower : MonoBehaviour
{
	/// <summary>
	/// Sentry(1),MachineGun(2),RocketLauncher(3),AntiAircraft(4),Stealth(5)
	/// </summary>

	public int type = -1;
	public String playerKey;
	public String id;
	public int level = 0;
	public float currentHitPoint = 0.0f;

	public Location location;

	public bool isAttacking = false;
	public bool isUpgrading = false;

	public TowerConfigData Config {
		get{ return TowerManager.Current.GetTowerConfig (type, level); }
	}
}
