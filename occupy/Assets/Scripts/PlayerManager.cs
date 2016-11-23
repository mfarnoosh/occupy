using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour {
	public Location WorldCenter = new Location(35.70423f,51.40570f);

	public Collider MapCollider;
//	private Bounds MapBounds;

	public static PlayerManager Current;
	public PlayerManager(){
		Current = this;
	}
	void Start(){
//		if (Map != null) {
//			MapCollider = Map.GetComponent<Collider> ();
////			Mesh MapMesh = Map.GetComponent<MeshFilter> ().mesh;
////			MapMesh.RecalculateBounds ();
////			MapBounds = MapMesh.bounds;
//		}
	}
	public Vector3? ScreenPointToMapPosition(Vector2 point){
		var ray = Camera.main.ScreenPointToRay (point);
		RaycastHit hit;

		if (!MapCollider.Raycast (ray, out hit, Mathf.Infinity))
			return null;
		float finalYPosition = MapCollider.transform.position.y + MapCollider.transform.localScale.z;
		Vector3 finalPosition = new Vector3 (hit.point.x, 1.01f,hit.point.z);
			
		return finalPosition;
	}

	public bool CanPlaceBuildingHere(GameObject go){
//		var verts = go.GetComponent<MeshFilter> ().mesh.vertices;
//		var obstacles = GameObject.FindObjectsOfType<NavMeshObstacle> ();
//		var cols = new List<Collider> ();
//		foreach (var o in obstacles) {
//			if (o.gameObject != go) {
//				cols.Add (o.gameObject.GetComponent<Collider> ());
//			}
//		}
//
//		foreach (var v in verts) {
//			NavMeshHit hit;
//			var vReal = go.transform.TransformPoint (v);
//			NavMesh.SamplePosition( vReal, out hit, 0.5f, NavMesh.AllAreas);
//
//			bool onXAxis = Mathf.Abs (hit.position.x - vReal.x) < 2f;
//			bool onZAxis = Mathf.Abs (hit.position.z - vReal.z) < 0.5f;
//			bool hitCollider = cols.Any (c => c.bounds.Contains (vReal));
//
//			Debug.Log (vReal.x  + "|" + Mathf.Abs (hit.position.z - vReal.z) + "|" + hitCollider);
//
//
//			if (!onXAxis || !onZAxis || hitCollider)
//				return false;
//		}


		return true;
	}
}
