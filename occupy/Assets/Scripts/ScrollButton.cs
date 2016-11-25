using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
public class ScrollButton : MonoBehaviour {
	private bool open = false;
	ArrayList list = new ArrayList();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		if (!open) {			
			float width = gameObject.GetComponent<RectTransform> ().sizeDelta.x;
			float height = gameObject.GetComponent<RectTransform> ().sizeDelta.y;
			float margin = -30;

			for (int i = 0; i < 3; i++) {
				list.Add (CreateButton ((i + 1) * (margin + width), 70, width, height));
			}
			open = true;
		} else {
			for (int i = 0; i < 3; i++) {
				Destroy ((GameObject)list[i]);
			}
			list.Clear ();
			open = false;
		}
	}
	GameObject CreateButton(float x, float y, float width, float height) {
		GameObject buttonGO = new GameObject();
		buttonGO.transform.parent = gameObject.transform;
		buttonGO.transform.position = Vector3.zero;


		Image image = buttonGO.AddComponent<Image> ();
		image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");



		Button buttonBU = buttonGO.AddComponent<Button>();
		buttonBU.targetGraphic = image;

		RectTransform rectTransform = buttonGO.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = Vector3.zero;
		rectTransform.anchorMax = Vector2.zero;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.pivot = Vector2.zero;
		rectTransform.position = new Vector3 (x, y, 0);
		rectTransform.localScale = new Vector3 (1, 1, 1);
		rectTransform.sizeDelta = new Vector2 (width, height);
		return buttonGO;
	}
}
