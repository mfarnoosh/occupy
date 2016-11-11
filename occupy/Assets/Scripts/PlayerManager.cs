using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public Collider MapCollider;

	public static PlayerManager Current;
	public PlayerManager(){
		Current = this;
	}

	public Vector3? ScreenPointToMapPosition(Vector2 point){
		var ray = Camera.main.ScreenPointToRay (point);
		RaycastHit hit;

		if (!MapCollider.Raycast (ray, out hit, Mathf.Infinity))
			return null;
		return hit.point;
	}
}
