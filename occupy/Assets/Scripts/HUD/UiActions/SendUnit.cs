using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SendUnit : EventAction
{
	private GameObject ghostObject;
	private Renderer ghostRend;

	private UnitDataKeeper dataKeeper;
	private Progressbar progressbar;

	void Start(){
		dataKeeper = GetComponent<UnitDataKeeper> ();
		progressbar = GetComponent<Progressbar> ();
//		if (dataKeeper != null && progressbar != null) {
//			progressbar.SetProgressAmount ((float)(dataKeeper.CurrentUnitData.CurrentHitPoint / dataKeeper.CurrentUnitConfigData.HitPoint * 100));
//		}
	}
	public override void PointerDown (Vector2 position)
	{
	}

	public override void PointerUp (Vector2 position)
	{
		var targetTower = GetTargetTower (position);
		if (targetTower != null && dataKeeper != null) {
			if (targetTower.playerKey.Equals (PlayerController.Current.PlayerKey)) {
				//Move Action
				SocketMessage sm = new SocketMessage ();
				sm.Cmd = "sendUnit";
				sm.Params.Add (dataKeeper.CurrentUnitData.Id.ToString ());
				sm.Params.Add (targetTower.id);

				NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
					List<Vector3> list = new List<Vector3>();
					for (int i = 0; i < data.value.Params.Count - 1; i+=2) {
						var tile = TowerManager.Current.SelectedTower.ParentTile;
						var p1 = float.Parse(data.value.Params[i]);
						var p2 = float.Parse(data.value.Params[i+1]);
						var loc = new Location(p1, p2);
						list.Add(GeoUtils.LocationToXYZ(tile, loc));
					}
					for (int i = 0; i < list.Count; i++) {
						Vector3 source = list[i];
						GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						g.transform.position = source;
						g.transform.localScale = new Vector3(5, 5, 5);					
						g.transform.SetParent(MapManager.Current.MapObject.transform);
					}



				});
			} else {
				//Attack Action
				SocketMessage sm = new SocketMessage ();
				sm.Cmd = "attack";
				sm.Params.Add (dataKeeper.CurrentUnitData.Id.ToString ());
				sm.Params.Add (targetTower.id);

				NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {

				});
			}
		}
		Destroy (ghostObject);

	}



	public override void PointerDragging (Vector2 position, Vector2 delta)
	{
		if (this.ghostObject == null && this.dataKeeper != null) {
			ghostObject = UnitManager.Current.GetUnitGhostPrefabByType (dataKeeper.CurrentUnitData.Type);
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
