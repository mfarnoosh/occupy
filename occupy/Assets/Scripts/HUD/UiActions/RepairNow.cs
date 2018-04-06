using UnityEngine;
using System.Collections;

public class RepairNow : EventAction {

	private TowerPopupHandler towerPopupHandler;
	void Start(){
		towerPopupHandler = GetComponent<TowerPopupHandler> ();
	}
	public override void PointerClick ()
	{
		if (towerPopupHandler != null) {
			Debug.Log ("not null");
		} else {
			Debug.Log ("null");
		}
		Debug.Log ("Repair Now Clicked.");
	}

	public override void PointerDown (Vector2 position) { }

	public override void PointerUp (Vector2 position){ }

	public override void PointerDragging (Vector2 position, Vector2 deltaPosition) { }

}
