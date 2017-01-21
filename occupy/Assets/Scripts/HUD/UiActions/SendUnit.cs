using UnityEngine;
using System.Collections;

public class SendUnit : EventAction
{
	private GameObject ghostObject;
	private Renderer ghostRend;

	public UnitData unit { get;	set;}

	void Start ()
	{
	}

	public override void PointerDown (Vector2 position)
	{
	}

	public override void PointerUp (Vector2 position)
	{
		var targetTower = GetTargetTower (position);
		if (targetTower != null) {
			SocketMessage sm = new SocketMessage ();
			sm.Cmd = "sendUnit";
			sm.Params.Add (unit.Type.ToString ());
			sm.Params.Add (targetTower.id);

			NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {

			});
		}
		Destroy (ghostObject);
	}

	public override void PointerDragging (Vector2 position, Vector2 delta)
	{
		if (this.ghostObject == null && this.unit != null) {
			ghostObject = UnitManager.Current.GetUnitGhostPrefabByType (this.unit.Type);
			ghostRend = ghostObject.GetComponent<Renderer> ();
		}
		MoveGhost (position);
	}

	public override void PointerClick ()
	{
		//TODO: Farnoosh - Go to clicked unit position on map(call MapManager.Current.MoveMap(new Location(unit.lat,unit.lon));
	}

	private void MoveGhost (Vector2 screenPosition)
	{
		if (ghostObject == null || ghostRend == null)
			return;
		Vector3? tempTarget = MapManager.Current.ScreenPointToMapPosition (screenPosition);

		if (tempTarget.HasValue == false)
			return;
		ghostObject.transform.position = tempTarget.Value;

		var tower = GetTargetTower (screenPosition);
		if (tower == null) {
			ghostRend.material.color = Color.red;
		} else {
			ghostRend.material.color = Color.green;
		}
	}

	private Tower GetTargetTower (Vector2 screenPosition)
	{
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit))
			return null;
		var tower = hit.transform.GetComponent<Tower> ();
		return tower;
	}
}
