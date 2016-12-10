using UnityEngine;
using System.Collections;

public abstract class EventAction : MonoBehaviour {
	public abstract void PointerClick ();
	public abstract void PointerDown(Vector2 position);
	public abstract void PointerUp(Vector2 position);
	public abstract void PointerDragging(Vector2 position,Vector2 deltaPosition);
}
