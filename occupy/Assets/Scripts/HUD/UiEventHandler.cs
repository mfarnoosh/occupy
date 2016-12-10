using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UiEventHandler : EventTrigger
{
	public static UiEventHandler Current;
	public UiEventHandler(){
		Current = this;
	}

	public override void OnPointerDown (PointerEventData eventData)
	{
		base.OnPointerDown (eventData);

		TouchManager.Current.enabled = false;

		var action = GetUiAction (eventData);
		if (action != null)
			action.PointerDown (eventData.position);
	}

	public override void OnPointerUp (PointerEventData eventData)
	{
		base.OnPointerUp (eventData);



		var action = GetUiAction (eventData);
		if (action != null)
			action.PointerUp (eventData.position);

		TouchManager.Current.enabled = true;
	}

	public override void OnDrag (PointerEventData eventData)
	{
		base.OnDrag (eventData);

		var action = GetUiAction (eventData);
		if (action != null)
			action.PointerDragging (eventData.position, eventData.delta);
	}

	public override void OnPointerClick (PointerEventData eventData)
	{
		base.OnPointerClick (eventData);
		var action = GetUiAction (eventData);
		if (action != null)
			action.PointerClick ();
	}

	private EventAction GetUiAction (PointerEventData eventData)
	{
		var go = eventData.selectedObject;
		if (go != null) {
			var action = go.GetComponent<EventAction> ();
			return action;
		}
		return null;
	}
}
