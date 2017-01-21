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

	public RectTransform TowerSidebarPanel;
	public GameObject TowerPopupGameObject;

	private TowerSidebarHandler towerSidebarHandler;
	private TowerPopupHandler towerPopupHandler;

	public static HUDManager Current;
	public HUDManager(){
		Current = this;
	}
	public void Start(){
		if (TowerSidebarPanel != null)
			towerSidebarHandler = TowerSidebarPanel.GetComponent<TowerSidebarHandler> ();
		if (TowerPopupGameObject != null)
			towerPopupHandler = TowerPopupGameObject.GetComponent<TowerPopupHandler> ();
	}
	public void UnitsPanelToggled(bool activated){
		//TODO: Farnoosh - do sth. e.g. hide all other panels
	}

	public void TowersPanelToggled(bool activated){
		//TODO: Farnoosh - do sth. e.g. hide all other panels
	}

	public void LateUpdate(){
		
	}

	public void TowerSelect(Tower tower){
		if(towerSidebarHandler != null){
			towerSidebarHandler.TowerSelect (tower);
		}
		if (towerPopupHandler != null) {
			towerPopupHandler.TowerSelect (tower);
		}
	}

	public void TowerDeselect(){
		if (towerPopupHandler != null)
			towerPopupHandler.TowerDeselect ();
		if (towerSidebarHandler != null)
			towerSidebarHandler.TowerDeselect ();
	}
}
