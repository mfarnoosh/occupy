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

	public static HUDManager Current;
	public HUDManager(){
		Current = this;
	}
	public void UnitsPanelToggled(bool activated){
		//TODO: Farnoosh - do sth. e.g. hide all other panels
	}

	public void TowersPanelToggled(bool activated){
		//TODO: Farnoosh - do sth. e.g. hide all other panels
	}
}
