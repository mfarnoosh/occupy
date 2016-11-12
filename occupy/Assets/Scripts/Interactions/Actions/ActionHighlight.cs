using UnityEngine;
using System.Collections;

public class ActionHighlight : TouchAction {
	public GameObject HighlightObject;

	public override void Select ()
	{
		if (HighlightObject != null)
			HighlightObject.SetActive (true);
		Debug.Log ("Highlight Select");
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
		Debug.Log ("Highlight Deselect");
	}

	void Start(){
		if (HighlightObject != null)
			HighlightObject.SetActive (false);
	}
}
