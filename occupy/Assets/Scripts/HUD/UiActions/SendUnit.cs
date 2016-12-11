using UnityEngine;
using System.Collections;

public class SendUnit : EventAction {
	public GameObject UnitPrefab;
	public GameObject UnitGhostPrefab;

	private GameObject ghostObject;
	private Renderer rend;

	public override void PointerDown (Vector2 position)
	{
		if (UnitPrefab == null || UnitGhostPrefab == null)
			return;

		ghostObject = GameObject.Instantiate (UnitGhostPrefab);
		ghostObject.transform.localScale = new Vector3(
			ghostObject.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
			ghostObject.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
			ghostObject.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);

		rend = ghostObject.GetComponent<Renderer> ();
		MoveGhost (position);
	}

	public override void PointerUp (Vector2 position)
	{
		var targetBuilding = GetTargetBuilding (position);
		if (targetBuilding != null) {
			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "sendUnit";
			//TODO: change to building id
			sm.Params.Add (targetBuilding.type.ToString());

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

		var building = GetTargetBuilding(screenPosition);
		if(rend != null){
			if (building == null) {
				rend.material.color = Color.red;
			} else {
				rend.material.color = Color.green;
			}
		}
	}

	private GameObjects.Building GetTargetBuilding(Vector2 screenPosition){
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit))
			return null;
		var building = hit.transform.GetComponent<GameObjects.Building> ();
		return building;
	}
}
