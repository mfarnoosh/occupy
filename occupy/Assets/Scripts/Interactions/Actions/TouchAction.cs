using UnityEngine;
using System.Collections;

public abstract class TouchAction : MonoBehaviour {

	public abstract void Select ();
	public abstract void SecondSelect ();
	public abstract void Deselect();
}
