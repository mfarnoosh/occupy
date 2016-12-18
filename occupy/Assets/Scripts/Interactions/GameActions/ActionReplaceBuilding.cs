using System;
using UnityEngine;
using Lean.Touch;
using System.Collections;

public class ActionReplaceBuilding : TouchAction {

	private Vector3 originalPosition;
	private bool isMoving = false;
	private Tile currentTile = null;

	Renderer rend;
	Color originalColor;
	Color Red = new Color(1,0,0,0.5f);
	Color Green = new Color (0, 1, 0, 0.5f);
	Color Yellow = new Color(0.5f,0.5f,0.5f,0.5f);

	void Start(){
		rend = GetComponent<Renderer> ();
	}

	public override void Select ()	{}
	public override void SecondSelect ()
	{
		TouchManager.Current.enabled = false;
		originalPosition = transform.position;
		originalColor = rend.material.color;

		var tile =  gameObject.transform.GetComponentInParent<Tile> ();
		if (tile != null)
			currentTile = tile;

		isMoving = true;
		rend.material.color = Yellow;
	}
	public override void Deselect ()
	{
		TouchManager.Current.enabled = true;
		isMoving = false;
	}

	void LateUpdate(){
		if (!isMoving)
			return;
		if (LeanTouch.Fingers == null || LeanTouch.Fingers.Count != 1) {
			Finish (currentTile);
			return;
		}
		RaycastHit hit;
		if (!Physics.Raycast (LeanTouch.Fingers[0].GetRay (Camera.main), out hit))
			return;
		var tile = hit.transform.GetComponent<Tile> ();
		if (tile != null)
			currentTile = tile;

		var screenPosition = LeanTouch.Fingers [0].ScreenPosition;

		var tempTarget = MapManager.Current.ScreenPointToMapPosition (screenPosition);
		if (tempTarget.HasValue == false) {
			Finish (currentTile);
			return;
		}
		transform.position = tempTarget.Value;

		if (MapManager.Current.CanPlaceBuildingHere (gameObject)) {
			rend.material.color = Yellow;
		} else {
			rend.material.color = Red;
		}
	}
	public void Finish(Tile tile){
		if (!MapManager.Current.CanPlaceBuildingHere (gameObject) || currentTile == null)
			transform.position = originalPosition;
		isMoving = false;
		rend.material.color = originalColor;
		var highlight = GetComponent<ActionHighlight> ();
		var tower = GetComponent<GameObjects.Tower> ();
		if (tower == null) {
			Debug.Log ("salam");
		}
		//Send Position to server
		Location loc = GeoUtils.XYZToLocation(tile,transform.position);
	
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "moveTower";

		sm.Params.Add (tower.Id);
		sm.Params.Add (loc.Latitude.ToString());
		sm.Params.Add (loc.Longitude.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			gameObject.transform.parent = currentTile.transform;
		});

		//End Sending position to server
		if (highlight != null)
			highlight.Select ();
		TouchManager.Current.enabled = true;
	}
}
