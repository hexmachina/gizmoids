using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplaySelectClickHandler : MonoBehaviour
{

	public ToggleDragGroup toggleDragGroup;

	public void OnClick(PointerEventData eventData)
	{
		if (toggleDragGroup.AnyTogglesOn())
		{
			foreach (var item in toggleDragGroup.ActiveToggles())
			{
				item.onDragEnabled.Invoke(true, eventData);
				item.onDrag.Invoke(eventData);
				var drag = item.gameObject;
				StartCoroutine(Wait(eventData, item));
			}
		}
	}

	IEnumerator Wait(PointerEventData eventData, ToggleDrag toggleDrag)
	{
		yield return new WaitForFixedUpdate();
		eventData.pointerDrag = toggleDrag.gameObject;
		toggleDrag.onDragEnd.Invoke(eventData);
		toggleDrag.isOn = false;
	}
}
