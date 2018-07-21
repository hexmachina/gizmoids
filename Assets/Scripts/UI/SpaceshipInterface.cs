using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpaceshipInterface : Selectable, IDragHandler, IBeginDragHandler
{
	[System.Serializable]
	public class BeginDragEvent : UnityEvent<PointerEventData>
	{ }

	[SerializeField]
	public GizLib.UnityEventVector3 onPointerDown = new GizLib.UnityEventVector3();

	[SerializeField]
	public BeginDragEvent onBeginDrag = new BeginDragEvent();

	[SerializeField]
	public GizLib.UnityEventVector3 onDrag = new GizLib.UnityEventVector3();

	[SerializeField]
	public GizLib.UnityEventVector3 onPointerUp = new GizLib.UnityEventVector3();

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		onPointerDown.Invoke(GetWorldPoint());
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		onBeginDrag.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		onDrag.Invoke(GetWorldPoint());
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		onPointerUp.Invoke(GetWorldPoint());
	}

	public Vector2 GetScreenPoint()
	{
		Vector2 screenPoint = Vector2.zero;
		if (Input.touchSupported && Input.touchCount > 0)
		{
			screenPoint = Input.GetTouch(0).position;
		}
		else if (Input.mousePresent)
		{
			screenPoint = Input.mousePosition;
		}
		else
		{
			Debug.Log("wha happened?");
		}
		return screenPoint;
	}

	public Vector3 GetWorldPoint()
	{
		return Camera.main.ScreenToWorldPoint(GetScreenPoint());
	}
}
