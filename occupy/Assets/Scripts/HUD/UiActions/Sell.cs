using UnityEngine;
using System.Collections;

public class Sell : EventAction {

	public override void PointerClick ()
	{
		Debug.Log ("Sell Clicked.");
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

}
