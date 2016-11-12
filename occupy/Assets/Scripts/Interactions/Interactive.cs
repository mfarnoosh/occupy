using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour {

	private bool _selected = false;
	public bool Selected{ get { return _selected; } }

	public bool Swap = false;

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
}
