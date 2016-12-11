using UnityEngine;
using System.Collections;

public class Units : EventAction {
	public RectTransform UnitsPanel;

	private bool UnitsPanelActivated = false;
	void Start(){
		UnitsPanel.gameObject.SetActive (false);
	}

	public override void PointerClick ()
	{
		ToggleUnitsPanel ();
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

	public void ToggleUnitsPanel(){
		if (UnitsPanelActivated) {
			UnitsPanel.gameObject.SetActive (false);
		} else {
			var pos = gameObject.transform.position;

			UnitsPanel.gameObject.SetActive (true);
//			UnitsPanel.gameObject.transform.position = 
//				new Vector3 (pos.x + GetComponent<RectTransform> ().rect.width + 10, pos.y, pos.z);
		}
		UnitsPanelActivated = !UnitsPanelActivated;
		HUDManager.Current.UnitsPanelToggled (UnitsPanelActivated);
	}
}
