using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

public class HUDManager : MonoBehaviour {
	public Button Profile;
	public Button Units;
	public Button Towers;
	public Button Giant;
	public Button MyLocation;
	public Button MyHome;

	public RectTransform UnitsPanel;
	public RectTransform TowersPanel;

	private bool UnitsPanelActivated = false;
	private bool TowersPanelActivated = false;

	public static HUDManager Current;
	public HUDManager(){
		Current = this;
	}
		
	void Start () {
		UnitsPanel.gameObject.SetActive (false);
		TowersPanel.gameObject.SetActive (false);
	}

	public void ToggleUnitsPanel(){
		if (UnitsPanelActivated) {
			UnitsPanel.gameObject.SetActive (false);
		} else {
			var pos = Units.gameObject.transform.position;

			UnitsPanel.gameObject.SetActive (true);
			UnitsPanel.gameObject.transform.position = 
			new Vector3 (pos.x + Units.GetComponent<RectTransform> ().rect.width + 10, pos.y, pos.z);
		}
		UnitsPanelActivated = !UnitsPanelActivated;
	}

	public void ToggleTowersPanel(){
		if (TowersPanelActivated) {
			TowersPanel.gameObject.SetActive (false);
		} else {
			var pos = Towers.gameObject.transform.position;

			TowersPanel.gameObject.SetActive (true);
			TowersPanel.gameObject.transform.position = 
			new Vector3 (pos.x - Towers.GetComponent<RectTransform> ().rect.width - 10, pos.y, pos.z);
		}
		TowersPanelActivated = !TowersPanelActivated;
	}
}
