using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using System.Collections;

public class CreateTowerEvent : EventAction {
	public GameObject TowerPrefab;
	public GameObject TowerGhostPrefab;

	private GameObject ghostObject;
	private Renderer rend;

	private Interactive ghostInteractive;

	public override void PointerDown (Vector2 position)
	{
		if (TowerPrefab == null || TowerGhostPrefab == null)
			return;

		ghostObject = GameObject.Instantiate (TowerGhostPrefab);
		
		rend = ghostObject.GetComponent<Renderer> ();
		rend.material.color = Color.yellow;

		ghostInteractive = ghostObject.GetComponent<Interactive> ();
		MoveGhost (position);
	}
	public override void PointerUp (Vector2 position)
	{
		if (!MapManager.Current.CanPlaceTowerHere (ghostObject) || ghostInteractive.Collisioned) {
		} else {
			var go = GameObject.Instantiate (TowerPrefab);

			var tower = go.GetComponent<Tower> ();
			//Send Position to server
			Tile tile = MapManager.Current.GetTile (ghostObject.transform.position);

			Location loc = GeoUtils.XYZToLocation (tile, ghostObject.transform.position);

			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "createTower";
			sm.Params.Add (loc.Latitude.ToString ());
			sm.Params.Add (loc.Longitude.ToString ());
			sm.Params.Add (tower.type.ToString ());
			NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
				string towersStr = data.value.Params [0];
				var towerData = JsonUtility.FromJson<TowerData> (towersStr);

				TowerManager.Current.SetTowerInfo(tower.gameObject,towerData,tile);
			});

			//End Sending position to server


			go.transform.position = ghostObject.transform.position;
			go.transform.parent = tile.transform;

		}
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

		if (MapManager.Current.CanPlaceTowerHere (ghostObject) && !ghostInteractive.Collisioned) {
			rend.material.color = Color.green;
		} else {
			rend.material.color = Color.red;
		}
	}
}
