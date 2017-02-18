using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionHighlight : TouchAction {
	public GameObject HighlightObject;

	private Tower tower = null;
	void Start(){
		if (HighlightObject != null) {
			HighlightObject.SetActive (false);
		}
		tower = GetComponent<Tower> ();
	}
	public override void Select ()
	{
		if (HighlightObject != null) {
			if (tower != null) {
				HighlightObject.transform.localScale = new Vector3 ((float)(tower.Config.Range) * 10000f, (float)(tower.Config.Range) * 10000f, (float)(tower.Config.Range) * 10000f);
			}
			HighlightObject.SetActive (true);
		}
		TowerManager.Current.SelectedTower = tower;
	}
	public override void SecondSelect ()
	{
	}
	public override void Deselect ()
	{
		if (HighlightObject != null)
			HighlightObject.SetActive (false);
		TowerManager.Current.SelectedTower = null;
	}
}
