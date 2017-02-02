using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TowerSidebarHandler : MonoBehaviour {
	public GameObject ButtonPrefab;
	public RectTransform Content;
	public List<Sprite> UnitButtonSprites;
	private Tower _selectedTower = null;

	private List<GameObject> contentChildren = new List<GameObject>();
	public void Start(){
		TowerDeselect ();
	}
	public void TowerSelect(Tower tower){
		if (tower == null) {
			TowerDeselect ();
			return;
		}
		if (_selectedTower != null && tower != null && _selectedTower.id.Equals (tower.id))
			return;
		_selectedTower = tower;
		gameObject.SetActive (true);
		fillData ();
	}

	public void TowerDeselect(){
		_selectedTower = null;
		gameObject.SetActive (false);
		foreach (var child in contentChildren)
			Destroy (child);
	}
	private void fillData(){
		if (_selectedTower != null && Content != null) {
			foreach (var ud in _selectedTower.towerUnits) {
				var go = CreateButton (ud);
				if (go != null) {
					contentChildren.Add (go);
					go.transform.SetParent (Content, false);
				}
			}
		}
	}

	private GameObject CreateButton(UnitData unitData) {
		if (ButtonPrefab == null)
			return null;
		GameObject buttonGO = GameObject.Instantiate (ButtonPrefab);
		buttonGO.name = UnitManager.Current.GetUnitNameByType (unitData.Type);
		buttonGO.transform.localScale = new Vector3 (1, 1, 1);
		var sendUnit = buttonGO.GetComponent<SendUnit> ();
		if (sendUnit == null)
			return null;
		sendUnit.unit = unitData;

		var button = buttonGO.GetComponent<Button> ();
		if (button != null) {
			button.image.sprite = UnitButtonSprites [unitData.Type - 1];
		}
		//Image image = buttonGO.AddComponent<Image> ();

		//Button buttonBU = buttonGO.AddComponent<Button>();
		//buttonBU.targetGraphic = image;

		//RectTransform rectTransform = buttonGO.GetComponent<RectTransform> ();
		//rectTransform.anchoredPosition = Vector2.zero;
		//rectTransform.anchorMax = Vector2.zero;
		//rectTransform.anchorMin = Vector2.zero;
		//rectTransform.position = new Vector3 (x, y, 0);
		//rectTransform.localScale = new Vector3 (1, 1, 1);
		//rectTransform.sizeDelta = new Vector2 (200, 50);

		return buttonGO;
	}

}
