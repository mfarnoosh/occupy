using System;
using UnityEngine;
using Lean.Touch;
using System.Collections;

public class ActionReplaceTower : TouchAction {

	private Vector3 originalPosition;
	private bool isMoving = false;
	private Tile currentTile = null;
	private Interactive interactive;
	private Renderer rend;
	private Color originalColor;
	/*

	Color originalColor;
	Color Red = new Color(1,0,0,0.5f);
	Color Green = new Color (0, 1, 0, 0.5f);
	Color Yellow = new Color(0.5f,0.5f,0.5f,0.5f);

	bool collisioned = false;
	void Start(){
		rend = GetComponent<Renderer> ();
	}
	void OnTriggerEnter(Collider col){
		if (!col.gameObject.name.StartsWith ("Tile")) {
			collisioned = true;
		}
	}
	void OnTriggerExit(Collider col){
		if (!col.gameObject.name.StartsWith ("Tile")) {
			collisioned = false;
		}
	}
*/
	void Start(){
		rend = GetComponent<Renderer> ();
		originalColor = rend.material.color;
		interactive = GetComponent<Interactive> ();

	}
	public override void Select ()	{}
	public override void SecondSelect ()
	{
		TouchManager.Current.enabled = false;
		originalPosition = transform.position;
		//originalColor = rend.material.color;
		Debug.Log ("original color: " +Time.deltaTime);
		var tile =  gameObject.transform.GetComponentInParent<Tile> ();
		if (tile != null)
			currentTile = tile;
		isMoving = true;
		//rend.material.color = Yellow;
	}
	public override void Deselect ()
	{
		TouchManager.Current.enabled = true;
		isMoving = false;
	}

	void LateUpdate(){
		if(!isMoving)
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

		if (!MapManager.Current.CanPlaceTowerHere (gameObject) || interactive.Collisioned) {
			rend.material.color = Color.red;
		} else {
			rend.material.color = originalColor;
		}
	}
	public void Finish(Tile tile){
		isMoving = false;
		var highlight = GetComponent<ActionHighlight> ();
		rend.material.color = originalColor;

		if (!MapManager.Current.CanPlaceTowerHere (gameObject) || currentTile == null || interactive.Collisioned) {
			transform.position = originalPosition;
		} else {
			var tower = GetComponent<Tower> ();
			if (tower == null) {
				Debug.Log ("salam");
			}
			//Send Position to server
			Location loc = GeoUtils.XYZToLocation (tile, transform.position);
	
			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "moveTower";

			sm.Params.Add (tower.id);
			sm.Params.Add (loc.Latitude.ToString ());
			sm.Params.Add (loc.Longitude.ToString ());
			NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
				gameObject.transform.parent = currentTile.transform;
			});
			//End Sending position to server
		}

		if (highlight != null)
			highlight.Select ();
		TouchManager.Current.enabled = true;
	}
}
