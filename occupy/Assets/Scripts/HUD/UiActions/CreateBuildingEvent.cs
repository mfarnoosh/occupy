using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using System.Collections;

public class CreateBuildingEvent : EventAction {
	public GameObject BuildingPrefab;
	public GameObject BuildingGhostPrefab;

	private GameObject ghostObject;
	private Renderer rend;

	public override void PointerDown (Vector2 position)
	{
		if (BuildingPrefab == null || BuildingGhostPrefab == null)
			return;

		ghostObject = GameObject.Instantiate (BuildingGhostPrefab);
		ghostObject.transform.localScale = new Vector3(
			ghostObject.transform.localScale.x * PlayerController.Current.ObjectScaleMultiplier.x,
			ghostObject.transform.localScale.y * PlayerController.Current.ObjectScaleMultiplier.y,
			ghostObject.transform.localScale.z * PlayerController.Current.ObjectScaleMultiplier.z);
		
		rend = ghostObject.GetComponent<Renderer> ();
		MoveGhost (position);
	}
	public override void PointerUp (Vector2 position)
	{
		var go = GameObject.Instantiate (BuildingPrefab);
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerController.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerController.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerController.Current.ObjectScaleMultiplier.z);

		var tower = go.GetComponent<GameObjects.Tower> ();
		//Send Position to server
		Tile tile = MapManager.Current.GetTile(ghostObject.transform.position);

		Location loc = GeoUtils.XYZToLocation(tile,ghostObject.transform.position);

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "createTower";
		sm.Params.Add (loc.Latitude.ToString());
		sm.Params.Add (loc.Longitude.ToString());
		sm.Params.Add (tower.Type.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string towersStr = data.value.Params[0];
			string towerId = JsonUtility.FromJson<TowerData>(towersStr).Id;
			TowerData towerData = TowerManager.Current.GetTowerData(tower.Type,1);
			towerData.Id = towerId;

			tower.FromObjectData(towerData);
			tile.AddTower(go);

		});

		//End Sending position to server


		go.transform.position = ghostObject.transform.position;
		go.transform.parent = tile.transform;
		Destroy (ghostObject);
	}

	public override void PointerDragging (Vector2 position, Vector2 delta)
	{
		MoveGhost (position);
	}

	public override void PointerClick (){}


	private void MoveGhost(Vector2 screenPosition){
		if (ghostObject == null || rend == null)
			return;
		Vector3? tempTarget = MapManager.Current.ScreenPointToMapPosition (screenPosition);

		if (tempTarget.HasValue == false)
			return;
		ghostObject.transform.position = tempTarget.Value;

		if (MapManager.Current.CanPlaceBuildingHere (ghostObject)) {
			rend.material.color = Color.green;
		} else {
			rend.material.color = Color.red;
		}
	}
	private void CreateBuilding(Tile tile,Location loc,Color col){
		var go = GameObject.Instantiate(BuildingPrefab);
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerController.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerController.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerController.Current.ObjectScaleMultiplier.z);

		go.GetComponent<Renderer> ().material.color = col;
		//TODO: Convert Latitude,Longitude to X,Y
		var pos = GeoUtils.LocationToXYZ(tile,loc);

		go.transform.position = pos;
		go.transform.parent = tile.transform;

	}
}
