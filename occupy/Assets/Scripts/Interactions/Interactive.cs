using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour {

	private bool _selected = false;
	public bool Selected{ get { return _selected; } }

	public bool Swap = false;

	Renderer rend;
	Color originalColor;
	public bool Interacting{ get; set; }

	void Start(){
		rend = GetComponent<Renderer> ();
		originalColor = rend.material.color;
	}

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

	void OnTriggerEnter(Collider col){
		if (!col.gameObject.name.StartsWith ("Tile") && Selected) {
			//originalColor = rend.material.color;
			rend.material.color = Color.red;
		}
	}
	void OnTriggerExit(Collider col){
		if (!col.gameObject.name.StartsWith ("Tile") && Selected) {
			rend.material.color = originalColor;
		}
	}
}
