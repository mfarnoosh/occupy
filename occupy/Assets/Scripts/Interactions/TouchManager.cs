using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Lean.Touch;

public class TouchManager : MonoBehaviour{
	public Text InfoText;

	[Tooltip("The minimum field of view angle we want to zoom to")]
	public float Minimum = 10.0f;

	[Tooltip("The maximum field of view angle we want to zoom to")]
	public float Maximum = 30.0f;

	private Interactive Selected;

	public static TouchManager Current;
	public TouchManager(){
		Current = this;
	}


	public void OnFingerDown(LeanFinger finger) { }
	public void OnFingerUp(LeanFinger finger) {	}

	public void OnFingerHeldUp(LeanFinger finger){
		//use leantouch GetRay function instead
//		var ray = Camera.main.ScreenPointToRay (finger.ScreenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (finger.GetRay(Camera.main), out hit))
			return;
		
		var interact = hit.transform.GetComponent<Interactive> ();
		if (interact == null) {
			if (Selected != null) {
				Selected.Deselect ();
				Selected = null;
			}
			return;
		}
		
		if (interact == Selected)
			return;


		RaycastHit startedHit;
		if (!Physics.Raycast (finger.GetStartRay(Camera.main), out startedHit))
			return;
		var startInteract = startedHit.transform.GetComponent<Interactive> ();
		if (startInteract == null || startInteract != interact)
			return;

		if (Selected != null) {
			Selected.Deselect ();
			Selected = null;
		}
		Selected = interact;
		interact.SecondSelect ();
	}
	void OnFingerTap(LeanFinger finger){
		var ray = Camera.main.ScreenPointToRay (finger.ScreenPosition);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit))
			return;

		var interact = hit.transform.GetComponent<Interactive> ();
		if (interact == null) {
			if (Selected != null) {
				Selected.Deselect ();
				Selected = null;
			}
			return;
		}
		if (interact == Selected)
			return;
		if (Selected != null) {
			Selected.Deselect ();
			Selected = null;
		}

		Selected = interact;

		interact.Select ();
	}
	public void OnFingerPinch(float pinchScale){
		
		if (pinchScale <= 0.0f || Camera.main == null)
			return;

		//Perspectivce camera
		var fieldOfView = Camera.main.fieldOfView;
		fieldOfView /= LeanTouch.PinchScale;
		fieldOfView = Mathf.Clamp(fieldOfView, Minimum, Maximum);

		Camera.main.fieldOfView = fieldOfView;


		if(InfoText != null)
			InfoText.text = "Pinching";


		//Orthographic camera
//		var orthographicSize = Camera.main.orthographicSize;
//		orthographicSize /= LeanTouch.PinchScale;
//		orthographicSize = Mathf.Clamp(orthographicSize, Minimum, Maximum);
//
//		Camera.main.orthographicSize = orthographicSize;

	}
	public void OnFingerSwipe(LeanFinger finger)
	{
		InfoText.text = finger.Index.ToString ();
		// Make sure the info text exists
		if (InfoText != null)
		{
			// Store the swipe delta in a temp variable
			var swipe = finger.SwipeDelta;
			var left  = new Vector2(-1.0f,  0.0f);
			var right = new Vector2( 1.0f,  0.0f);
			var down  = new Vector2( 0.0f, -1.0f);
			var up    = new Vector2( 0.0f,  1.0f);

			if (SwipedInThisDirection(swipe, left) == true)
			{
				InfoText.text = "You swiped left!";
			}

			if (SwipedInThisDirection(swipe, right) == true)
			{
				InfoText.text = "You swiped right!";
			}

			if (SwipedInThisDirection(swipe, down) == true)
			{
				InfoText.text = "You swiped down!";
			}

			if (SwipedInThisDirection(swipe, up) == true)
			{
				InfoText.text = "You swiped up!";
			}

			if (SwipedInThisDirection(swipe, left + up) == true)
			{
				InfoText.text = "You swiped left and up!";
			}

			if (SwipedInThisDirection(swipe, left + down) == true)
			{
				InfoText.text = "You swiped left and down!";
			}

			if (SwipedInThisDirection(swipe, right + up) == true)
			{
				InfoText.text = "You swiped right and up!";
			}

			if (SwipedInThisDirection(swipe, right + down) == true)
			{
				InfoText.text = "You swiped right and down!";
			}
		}
	}

	private bool SwipedInThisDirection(Vector2 swipe, Vector2 direction)
	{
		// Find the normalized dot product between the swipe and our desired angle (this will return the acos between the vectors)
		var dot = Vector2.Dot(swipe.normalized, direction.normalized);

		// With 8 directions, each direction takes up 45 degrees (360/8), but we're comparing against dot product, so we need to halve it
		var limit = Mathf.Cos(22.5f * Mathf.Deg2Rad);

		// Return true if this swipe is within the limit of this direction
		return dot >= limit;
	}



	protected virtual void OnEnable()
	{
		// Hook into the events we need
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		LeanTouch.OnPinch += OnFingerPinch;
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerUp += OnFingerUp;
		LeanTouch.OnFingerTap += OnFingerTap;
		LeanTouch.OnFingerHeldSet += OnFingerHeldUp;
	}

	protected virtual void OnDisable()
	{
		// Unhook the events
		LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		LeanTouch.OnPinch -= OnFingerPinch;
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerUp -= OnFingerUp;
		LeanTouch.OnFingerTap -= OnFingerTap;
		LeanTouch.OnFingerHeldSet -= OnFingerHeldUp;
	}
}
