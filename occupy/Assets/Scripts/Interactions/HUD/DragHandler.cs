using UnityEngine;
using UnityEngine.Events;
using Lean.Touch;
using System.Collections;

public class DragHandler : MonoBehaviour {
	public GameObject Prefab;
	public GameObject Ghost;
	public bool IsCreatingBuilding;

	private Renderer rend;
	void Start(){
		TouchManager.Current.enabled = false;
		rend = GetComponent<Renderer> ();
	}
	void LateUpdate(){
		if (LeanTouch.Fingers == null || LeanTouch.Fingers.Count != 1)
			return;

		var screenPosition = LeanTouch.Fingers [0].ScreenPosition;
		Vector3? tempTarget = MapManager.Current.ScreenPointToMapPosition (screenPosition);

		if (tempTarget.HasValue == false)
			return;
		transform.position = tempTarget.Value;


		if (IsCreatingBuilding) {
			if (PlayerManager.Current.CanPlaceBuildingHere (gameObject)) {
				rend.material.color = Color.green;
			} else {
				rend.material.color = Color.red;
			}
		} else {
			//dragging unit
			var building = GetTargetBuilding(screenPosition);
			if(rend != null){
				if (building == null) {
					rend.material.color = Color.red;
				} else {
					rend.material.color = Color.green;
				}
			}
		}
	}
	public GameObjects.Building GetTargetBuilding(Vector2 screenPosition){
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit))
			return null;
		var building = hit.transform.GetComponent<GameObjects.Building> ();
		return building;
	}
	public void Finish(Vector2 screenPosition){
		if (IsCreatingBuilding)
			FinishSavingBuilding ();
		else
			FinishMovingUnit (screenPosition);
	}
	private void FinishSavingBuilding(){
		var go = GameObject.Instantiate (Prefab);
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);

		//Send Position to server
		Tile tile = MapManager.Current.GetTile(transform.position);

		Location loc = GeoUtils.XYZToLocation(tile,transform.position);

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "saveBuilding";
		sm.Params.Add (loc.Latitude.ToString());
		sm.Params.Add (loc.Longitude.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string lat = data.value.Params[0];
			string lon = data.value.Params[1];
			Location sLoc = new Location(float.Parse(lat),float.Parse(lon));

			CreateBuilding(tile,sLoc,Color.green);


			string lat2 = data.value.Params[2];
			string lon2 = data.value.Params[3];

			Location sLoc2 = new Location(float.Parse(lat2),float.Parse(lon2));

			CreateBuilding(tile,sLoc2,Color.red);

			string lat3 = data.value.Params[4];
			string lon3 = data.value.Params[5];

			Location sLoc3 = new Location(float.Parse(lat3),float.Parse(lon3));

			CreateBuilding(tile,sLoc3,Color.gray);


			string lat4 = data.value.Params[6];
			string lon4 = data.value.Params[7];

			Location sLoc4 = new Location(float.Parse(lat4),float.Parse(lon4));

			CreateBuilding(tile,sLoc4,Color.yellow);

		});

		//End Sending position to server


		go.transform.position = transform.position;
		go.transform.parent = tile.transform;
		Destroy (this.gameObject);
	}
	private void FinishMovingUnit(Vector2 screenPosition){
		var targetBuilding = GetTargetBuilding (screenPosition);
		if (targetBuilding != null) {
			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "moveUnit";
			//TODO: change to building id
			sm.Params.Add (targetBuilding.type.ToString());

			NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {

			});
		}
		Destroy (this.gameObject);
	}
	void OnDestroy(){
		TouchManager.Current.enabled = true;
	}
	//TODO: Farnoosh remove these lines. just for test getting data from server
	private void CreateBuilding(Tile tile,Location loc,Color col){
		var go = GameObject.Instantiate(Prefab);
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);
		
		go.GetComponent<Renderer> ().material.color = col;
		//TODO: Convert Latitude,Longitude to X,Y
		var pos = GeoUtils.LocationToXYZ(tile,loc);

		go.transform.position = pos;
		go.transform.parent = tile.transform;

	}
	//TODO

}
