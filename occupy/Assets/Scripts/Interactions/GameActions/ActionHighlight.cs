using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionHighlight : TouchAction {
	public GameObject HighlightObject;
	public Canvas HUDCanvas;

	private Tower tower = null;
	void Start(){
		if (HighlightObject != null) {
			HighlightObject.SetActive (false);
			if(HUDCanvas != null)
				HUDCanvas.enabled = false;

			tower = GetComponent<Tower> ();
		}
	}
	public override void Select ()
	{
		if (HighlightObject != null) {
			if (tower != null) {
				HighlightObject.transform.localScale = new Vector3 ((float)(tower.Config.Range), (float)(tower.Config.Range), (float)(tower.Config.Range));
			}
			HighlightObject.SetActive (true);
			if(HUDCanvas != null)
				HUDCanvas.enabled = true;
		}
	}
	public override void SecondSelect ()
	{
//		if (HighlightObject != null)
//			HighlightObject.SetActive (true);
//		Debug.Log ("Highlight Second Select");
	}
	public override void Deselect ()
	{
		if (HighlightObject != null) {
			HighlightObject.SetActive (false);
			if(HUDCanvas != null)
				HUDCanvas.enabled = false;
		}
	}
}
