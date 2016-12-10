using UnityEngine;
using System.Collections;

public class Towers : EventAction {

	public override void PointerClick ()
	{
		HUDManager.Current.ToggleTowersPanel ();
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

}
