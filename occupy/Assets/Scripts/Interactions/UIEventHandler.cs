using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using System.Collections;

public class UIEventHandler : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IMoveHandler {
	public GameObject Prefab;
	public GameObject Ghost;

	protected bool IsFingerDown = false;
	private DragHandler activeDragHandler = null;
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Prefab == null || Ghost == null)
			return;
		IsFingerDown = true;

		var go = GameObject.Instantiate (Ghost);
		var dragHandler = go.AddComponent<DragHandler> ();
		dragHandler.Prefab = Prefab;
		dragHandler.Ghost = Ghost;


		activeDragHandler = dragHandler;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IsFingerDown = false;
		if (activeDragHandler == null)
			return;
		activeDragHandler.Finish (eventData.position);
	}

	public void OnMove(AxisEventData eventData){
		
	}
//	private void OnEnable(){
////		LeanTouch.OnFingerDown     += _OnFingerDown;
////		LeanTouch.OnFingerUp       += _OnFingerUp;
////		LeanTouch.OnFingerDrag     += _OnFingerDrag;
////		LeanTouch.OnFingerTap      += _OnFingerTap;
//	}
//	private void OnDisable(){
////		LeanTouch.OnFingerDown     -= _OnFingerDown;
////		LeanTouch.OnFingerUp       -= _OnFingerUp;
////		LeanTouch.OnFingerDrag     -= _OnFingerDrag;
////		LeanTouch.OnFingerTap      -= _OnFingerTap;
//	}
//
//	private void _OnFingerDown(LeanFinger finger){
//		IsFingerDown = true;
//		OnFingerDown (finger.ScreenPosition);
//	}
//
//	private void _OnFingerUp(LeanFinger finger){
//		IsFingerDown = false;
//		OnFingerUp (finger.ScreenPosition);
//	}
//
//	private void _OnFingerDrag(LeanFinger finger){
//		OnDrag (finger.ScreenPosition, finger.DeltaScreenPosition);
//	}
//	private void _OnFingerTap(LeanFinger finger){
//		
//	}
}
