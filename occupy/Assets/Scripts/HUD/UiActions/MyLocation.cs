using UnityEngine;
using System.Collections;
using System;
public class MyLocation : EventAction {

	public override void PointerClick ()
	{
		Debug.Log ("My Location Clicked.");
		GPS gps = new GPS ();
		StartCoroutine (gps.GetLocation (new Action<Location>(loc => {
			Debug.Log ("My Location finished");
			if (loc != null) {
				Debug.Log (gps.location);
				MapManager.Current.MoveTo (gps.location);
			}
		})));						
			
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }
}
