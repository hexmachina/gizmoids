using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerClickTrigger : Selectable, IPointerClickHandler
{

	[System.Serializable]
	public class PointerEvent : UnityEvent<PointerEventData>
	{ }

	public PointerEvent onClick = new PointerEvent();


	public void OnPointerClick(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		onClick.Invoke(eventData);
	}
}
