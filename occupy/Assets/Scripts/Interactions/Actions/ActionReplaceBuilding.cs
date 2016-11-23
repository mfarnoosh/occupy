using UnityEngine;
using Lean.Touch;
using System.Collections;

public class ActionReplaceBuilding : TouchAction {

	private Vector3 originalPosition;
	private bool isMoving = false;

	Renderer rend;
	Color originalColor;
	Color Red = new Color(1,0,0,0.5f);
	Color Green = new Color (0, 1, 0, 0.5f);
	Color Yellow = new Color(0.5f,0.5f,0.5f,0.5f);

	void Start(){
		rend = GetComponent<Renderer> ();
	}

	public override void Select ()	{}
	public override void SecondSelect ()
	{
		TouchManager.Current.enabled = false;
		originalPosition = transform.position;
		originalColor = rend.material.color;

		isMoving = true;
		rend.material.color = Yellow;
	}
	public override void Deselect ()
	{
		TouchManager.Current.enabled = true;
		isMoving = false;
	}

	void Update(){
		if (!isMoving)
			return;
		if (LeanTouch.Fingers == null || LeanTouch.Fingers.Count != 1) {
			Finish ();
			return;
		}
		var screenPosition = LeanTouch.Fingers [0].ScreenPosition;

		var tempTarget = PlayerManager.Current.ScreenPointToMapPosition (screenPosition);
		if (tempTarget.HasValue == false) {
			Finish ();
			return;
		}
		transform.position = tempTarget.Value;

		if (PlayerManager.Current.CanPlaceBuildingHere (gameObject)) {
			rend.material.color = Yellow;
		} else {
			rend.material.color = Red;
		}
	}
	public void Finish(){	
		if (!PlayerManager.Current.CanPlaceBuildingHere (gameObject))
			transform.position = originalPosition;
		isMoving = false;
		rend.material.color = originalColor;
		var highlight = GetComponent<ActionHighlight> ();
		if (highlight != null)
			highlight.Select ();
		TouchManager.Current.enabled = true;
	}
}
