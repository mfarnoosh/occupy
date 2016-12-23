using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactive : MonoBehaviour {

	private bool _selected = false;
	public bool Selected{ get { return _selected; } }

	public bool Swap = false;
	public bool Collisioned{ get; set; }

	public void Select(){
		_selected = true;
		foreach (var selection in GetComponents<TouchAction>()) {
			selection.Select ();
		}
	}
	public void SecondSelect(){
		_selected = true;
		foreach (var selection in GetComponents<TouchAction>()) {
			selection.SecondSelect ();
		}
	}
	public void Deselect(){
		_selected = false;
		foreach (var selection in GetComponents<TouchAction>()) {
			selection.Deselect ();
		}
	}

	void Update () {
		if (Swap) {
			Swap = false;
			if (_selected)
				Deselect ();
			else
				Select ();
		}
	}

	//TODO: Farnoosh: 951003 - sometimes when a tower is very exactly between two others, collision detection not working well.

	List<Collider> collisions = new List<Collider>();
	void OnTriggerEnter(Collider col){
		
		if (!col.gameObject.name.StartsWith ("Tile")) {
			if (!collisions.Contains (col)) {
				collisions.Add (col);
				Collisioned = true;
			}
		}
	}
	void OnTriggerExit(Collider col){
		
		if (!col.gameObject.name.StartsWith ("Tile")) {
			collisions.Remove (col);
			if(collisions.Count == 0)
				Collisioned = false;
		}
	}
}
