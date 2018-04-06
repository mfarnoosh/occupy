using UnityEngine;
using System.Collections;

public class Profile : EventAction {

	public override void PointerClick ()
	{
		Debug.Log ("Profile Clicked.");
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

}
