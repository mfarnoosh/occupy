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


	void HideAllButtons() {
		
		Units.enabled = false;
		Towers.enabled = false;
		Giant.enabled = false;
		MyLocation.enabled = false;
		MyHome.enabled = false;

	}
	void ShowAllButtons() {		
		Units.enabled = true;
		Towers.enabled = true;
		Giant.enabled = true;
		MyLocation.enabled = true;
		MyHome.enabled = true;
	}

	void OnProfileClicked() {
	}
	void OnUnitsClicked() {		
		HideAllButtons ();
		//ShowScrollView (0);
	}
	void OnBuildingsClicked() {
		HideAllButtons ();
		//ShowScrollView (1);


	}
	void OnGiantClicked() {
		
	}
	void OnMyLocationClicked() {
		
	}
	void OnMyHomeClicked() {
		
	}
	void OnCloseClicked() {
//		scrollRect.content.offsetMax = new Vector2 (0, scrollRect.content.offsetMax.y);
//		scrollRect.content.offsetMin = new Vector2 (0, scrollRect.content.offsetMin.y);
//		HideScrollView ();
		ShowAllButtons ();
		GameObject[] objects = GameObject.FindGameObjectsWithTag("scrollable");
		foreach (GameObject g in objects) {
			Destroy (g);
		}

	}

	GameObject CreateButton(float x, float y, float width, float height, string tag) {
		GameObject buttonGO = new GameObject();
		buttonGO.tag = tag;
		buttonGO.transform.localScale = new Vector3 (1, 1, 1);
		Image image = buttonGO.AddComponent<Image> ();
//		image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");



		Button buttonBU = buttonGO.AddComponent<Button>();
		buttonBU.targetGraphic = image;

		RectTransform rectTransform = buttonGO.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.anchorMax = Vector2.zero;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.position = new Vector3 (x, y, 0);
		rectTransform.localScale = new Vector3 (1, 1, 1);
		rectTransform.sizeDelta = new Vector2 (width, height);
		return buttonGO;
	}
}
