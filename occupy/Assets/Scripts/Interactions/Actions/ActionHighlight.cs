using UnityEngine;
using System.Collections;

public class ActionHighlight : TouchAction {
	public GameObject HighlightObject;

	public override void Select ()
	{
		if (HighlightObject != null)
			HighlightObject.SetActive (true);
	}
	public override void SecondSelect ()
	{
//		if (HighlightObject != null)
//			HighlightObject.SetActive (true);
//		Debug.Log ("Highlight Second Select");
	}
	public override void Deselect ()
	{
		if (HighlightObject != null)
			HighlightObject.SetActive (false);
	}

	void Start(){
		if (HighlightObject != null)
			HighlightObject.SetActive (false);
	}
}
