using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using System.Collections;

public class UIEventHandler : MonoBehaviour,IPointerDownHandler, IPointerUpHandler {
	public GameObject Prefab;
	public GameObject Ghost;
	public bool IsCreatingBuilding = false;

	protected bool IsFingerDown = false;
	private DragHandler activeDragHandler = null;
	public void OnPointerDown(PointerEventData eventData)
	{
		IsFingerDown = true;
		if (IsCreatingBuilding) {
			if (Prefab == null || Ghost == null)
				return;
			var go = GameObject.Instantiate (Ghost);
			go.transform.localScale = new Vector3(
				go.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
				go.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
				go.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);
			
			var dragHandler = go.AddComponent<DragHandler> ();
			dragHandler.Prefab = Prefab;
			dragHandler.Ghost = Ghost;

			activeDragHandler = dragHandler;
		} else {
			//TODO: Unit dragging
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IsFingerDown = false;
		if (activeDragHandler == null)
			return;
		activeDragHandler.Finish (eventData.position);
	}
}
