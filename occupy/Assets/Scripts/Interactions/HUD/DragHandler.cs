using UnityEngine;
using UnityEngine.Events;
using Lean.Touch;
using System.Collections;

public class DragHandler : MonoBehaviour {
	public GameObject Prefab;
	public GameObject Ghost;


	void LateUpdate(){
		if (LeanTouch.Fingers == null || LeanTouch.Fingers.Count != 1)
			return;
		Debug.Log ("Update : " + LeanTouch.Fingers.Count);
		var screenPosition = LeanTouch.Fingers [0].ScreenPosition;

		var tempTarget = PlayerManager.Current.ScreenPointToMapPosition (screenPosition);
		if (tempTarget.HasValue == false)
			return;

		transform.position = tempTarget.Value;
	}

	public void Finish(Vector2 screenPosition){
		var go = GameObject.Instantiate (Prefab);
		go.transform.position = transform.position;
		Destroy (this.gameObject);
	}
		
	void OnDestroy(){
		//TouchManager.Current.enabled = true;
	}
}
