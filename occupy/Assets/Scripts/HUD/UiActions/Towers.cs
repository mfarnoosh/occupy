using UnityEngine;
using System.Collections;

public class Towers : EventAction {
	public RectTransform TowersPanel;

	private bool TowersPanelActivated = false;
	void Start(){
		TowersPanel.gameObject.SetActive (false);
	}

	public override void PointerClick ()
	{
		ToggleTowersPanel ();
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

	public void ToggleTowersPanel(){
		if (TowersPanelActivated) {
			TowersPanel.gameObject.SetActive (false);
		} else {
//			var pos = gameObject.transform.position;

			TowersPanel.gameObject.SetActive (true);
//			TowersPanel.gameObject.transform.position = 
//				new Vector3 (pos.x - GetComponent<RectTransform> ().rect.width - 10, pos.y, pos.z);
		}
		TowersPanelActivated = !TowersPanelActivated;

		HUDManager.Current.TowersPanelToggled (TowersPanelActivated);
	}
}
