using UnityEngine;
using System.Collections;

public class Units : EventAction {
	public override void PointerClick ()
	{
		HUDManager.Current.ToggleUnitsPanel ();
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }
}
