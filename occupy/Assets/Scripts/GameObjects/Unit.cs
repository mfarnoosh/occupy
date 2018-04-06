using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
	/// <summary>
	/// Solider(1),Machine(2),Tank(3),Helicopter(4),Aircraft(5),Titan(6)
	/// </summary>
	public int type = -1;
	public UnitData unitData;
	private Vector3 newPosition = Vector3.zero;
	private bool showUnit = false;
	public Tower parentTower;
	public bool ghost = false;
	Animator animator;
	public UnitConfigData Config {
		get{ return UnitManager.Current.GetUnitConfig (type, unitData.Level); }
	}
	void Start(){
		if(ghost)
			this.gameObject.SetActive (true);
		if(parentTower != null)
			gameObject.transform.position = parentTower.transform.position;
		animator = GetComponent<Animator> ();
	}
	void Update(){
		if (ghost) {
			return;
		}
		if (!showUnit) {
			return;
		}

		if (newPosition != Vector3.zero) {
			var currentPos = transform.position;
			transform.LookAt (newPosition);
			//Lerp is a delay, intentionally added to the game by Valve for smoothing movements of players who use unstable internet connection
			//Ref: https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=2&cad=rja&uact=8&ved=0ahUKEwjVubKxu_fRAhVBSRoKHX3HDbEQFggfMAE&url=https%3A%2F%2Fsteamcommunity.com%2Fsharedfiles%2Ffiledetails%2F%3Fid%3D366151973&usg=AFQjCNHyw9xAFBJ2GvKHDSIkcZHAoO2zPg&sig2=gnPMH0v68zgBSd45wVknLw
			transform.position = Vector3.Lerp (currentPos, newPosition, (float)(Config.Speed * 0.05f) * Time.deltaTime / 10);

		}
		if (animator != null) {
			animator.SetBool ("isMoving", unitData.IsMoving && !unitData.IsAttacking);
			animator.SetBool ("isAttacking", unitData.IsAttacking);
			if (unitData.CurrentHitPoint <= 0) {
				animator.SetTrigger ("Death");
			}
		}
	}

	public void UpdateData(UnitData unitData){
		this.unitData = unitData;
		showUnit = UnitManager.Current.ShowUnitObject (parentTower, unitData);

		if (showUnit) {
			this.gameObject.SetActive (true);
		} else {
			this.gameObject.SetActive (false);
		}

		if (parentTower != null && parentTower.ParentTile != null) {
			Location newLocation = new Location ((float)(unitData.Lat), (float)(unitData.Lon));
			newPosition = GeoUtils.LocationToXYZ (parentTower.ParentTile, newLocation);
		}
	}
	public void RemovedFromOwnerTower(){
		Destroy (gameObject);
	}
}
