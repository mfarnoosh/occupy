using UnityEngine;
using System.Collections;

public class SendUnit : EventAction {
	public GameObject UnitPrefab;
	public GameObject UnitGhostPrefab;

	private GameObject ghostObject;
	private Renderer rend;

	private Unit unit;
	void Start(){
		unit = UnitPrefab.GetComponent<Unit> ();
	}
	public override void PointerDown (Vector2 position)
	{
		if (UnitPrefab == null || UnitGhostPrefab == null)
			return;

		ghostObject = GameObject.Instantiate (UnitGhostPrefab);

		rend = ghostObject.GetComponent<Renderer> ();
		MoveGhost (position);
	}

	public override void PointerUp (Vector2 position)
	{
		var targetTower = GetTargetTower (position);
		if (targetTower != null) {
			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "sendUnit";
			sm.Params.Add (unit.id);
			sm.Params.Add (targetTower.id);

			NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {

			});
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

		var tower = GetTargetTower(screenPosition);
		if(rend != null){
			if (tower == null) {
				rend.material.color = Color.red;
			} else {
				rend.material.color = Color.green;
			}
		}
	}

	private Tower GetTargetTower(Vector2 screenPosition){
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit))
			return null;
		var tower = hit.transform.GetComponent<Tower> ();
		return tower;
	}
}
