using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class OnScreenManager : MonoBehaviour {
	public GameObject buildingPrefab;

	public Button profile;
	public Button units;
	public Button buildings;
	public Button giant;
	public Button myLocation;
	public Button myHome;
	public Button closeScrollView;
	public ScrollRect scrollRect;
	public List<GameObjects.Building> buildingObjects = new List<GameObjects.Building> ();
	public List<GameObjects.Unit> unitObjects = new List<GameObjects.Unit> ();
	// Use this for initialization
	void Start () {
		// create test objects
		unitObjects.Add(new GameObjects.Unit());
		unitObjects.Add(new GameObjects.Unit());
		unitObjects.Add(new GameObjects.Unit());
		unitObjects.Add(new GameObjects.Unit());
		unitObjects.Add(new GameObjects.Unit());

		buildingObjects.Add (new GameObjects.Building());
		buildingObjects.Add (new GameObjects.Building());
		buildingObjects.Add (new GameObjects.Building());
		buildingObjects.Add (new GameObjects.Building());
		buildingObjects.Add (new GameObjects.Building());
		buildingObjects.Add (new GameObjects.Building());
		// hide scrollview at first and set onclicks
		HideScrollView ();
		profile.onClick.AddListener (() => {
			OnProfileClicked ();
		});
		units.onClick.AddListener (() => {
			OnUnitsClicked ();
		});
		buildings.onClick.AddListener (() => {
			OnBuildingsClicked ();
		});
		giant.onClick.AddListener (() => {
			OnGiantClicked ();
		});
		myLocation.onClick.AddListener (() => {
			OnMyLocationClicked ();
		});
		myHome.onClick.AddListener (() => {
			OnMyHomeClicked ();
		});
		closeScrollView.onClick.AddListener (() => {
			OnCloseClicked ();
		});

	}
	
	// Update is called once per frame
	void Update () {
		// if something is not enabled it should not be shown on canvas
		if (scrollRect.enabled) {
			scrollRect.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			scrollRect.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (closeScrollView.enabled) {
			closeScrollView.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			closeScrollView.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (units.enabled) {
			units.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			units.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (buildings.enabled) {
			buildings.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			buildings.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (giant.enabled) {
			giant.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			giant.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (myLocation.enabled) {
			myLocation.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			myLocation.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}
		if (myHome.enabled) {
			myHome.GetComponentInChildren<CanvasRenderer> ().SetAlpha (1);
		} else {
			myHome.GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
		}


	}

	void HideScrollView() {				
		scrollRect.enabled = false;
		closeScrollView.enabled = false;
	}
	/** 
	 * if type == 0 then it units will be shown else it will show buildings
	 **/
	void ShowScrollView(int type) {				
		scrollRect.enabled = true;
		closeScrollView.enabled = true;
		if (type == 0) {
			int offsetY = 100;
			int offsetX = 70;
			int margin = 20;
			int width = 70;
			int height = 70;
			for (int i = 0; i < unitObjects.Count; i++) {			
				GameObject newButton = CreateButton (offsetX + (i * (width + margin)), offsetY, width, height, "scrollable");
				newButton.transform.SetParent (scrollRect.content, false);
			}
			float right = scrollRect.content.rect.width - (width + margin) * unitObjects.Count;
			if (right < 0 )
				scrollRect.content.offsetMax = new Vector2 (-right, scrollRect.content.offsetMax.y);
		} else {
			int offsetY = 100;
			int offsetX = 70;
			int margin = 20;
			int width = 70;
			int height = 70;
			for (int i = 0; i < buildingObjects.Count; i++) {			
				GameObject newButton = CreateButton (offsetX + (i * (width + margin)), offsetY, width, height, "scrollable");
				newButton.transform.SetParent (scrollRect.content, false);
			}
			float right = scrollRect.content.rect.width - (width + margin) * buildingObjects.Count;
			if (right < 0 )
				scrollRect.content.offsetMax = new Vector2 (-right, scrollRect.content.offsetMax.y);
		}
	}
	void HideAllButtons() {
		
		units.enabled = false;
		buildings.enabled = false;
		giant.enabled = false;
		myLocation.enabled = false;
		myHome.enabled = false;

	}
	void ShowAllButtons() {		
		units.enabled = true;
		buildings.enabled = true;
		giant.enabled = true;
		myLocation.enabled = true;
		myHome.enabled = true;
	}

	void OnProfileClicked() {
	}
	void OnUnitsClicked() {		
		HideAllButtons ();
		ShowScrollView (0);
	}
	void OnBuildingsClicked() {
		HideAllButtons ();
		ShowScrollView (1);


	}
	void OnGiantClicked() {
		
	}
	void OnMyLocationClicked() {
		
	}
	void OnMyHomeClicked() {
		
	}
	void OnCloseClicked() {
		scrollRect.content.offsetMax = new Vector2 (0, scrollRect.content.offsetMax.y);
		scrollRect.content.offsetMin = new Vector2 (0, scrollRect.content.offsetMin.y);
		HideScrollView ();
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
		image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");



		Button buttonBU = buttonGO.AddComponent<Button>();
		buttonBU.targetGraphic = image;

		RectTransform rectTransform = buttonGO.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.anchorMax = Vector2.zero;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.position = new Vector3 (x, y, 0);
		rectTransform.localScale = new Vector3 (1, 1, 1);
		rectTransform.sizeDelta = new Vector2 (width, height);

		var eventHandler = buttonGO.AddComponent<UIEventHandler> ();
		eventHandler.Prefab = buildingPrefab;
		eventHandler.Ghost = buildingPrefab;
		eventHandler.IsCreatingBuilding = true;

		return buttonGO;
	}
}
