using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(RectTransform))]
public class ToggleDrag : Selectable, IDragHandler, IPointerClickHandler, IEndDragHandler
{
	public enum ToggleTransition
	{
		None,
		Fade
	}

	[Serializable]
	public class ToggleEvent : UnityEvent<bool>
	{ }

	[Serializable]
	public class DragBeginEvent : UnityEvent<bool, PointerEventData>
	{ }

	[Serializable]
	public class PointerEvent : UnityEvent<PointerEventData>
	{ }

	public DragBeginEvent onDragEnabled = new DragBeginEvent();

	public PointerEvent onDrag = new PointerEvent();

	public PointerEvent onDragEnd = new PointerEvent();

	public ToggleEvent onValueChanged = new ToggleEvent();

	public ToggleEvent onValueChangedInverse = new ToggleEvent();

	public ToggleTransition toggleTransition = ToggleTransition.Fade;

	/// <summary>
	/// Graphic the toggle should be working with.
	/// </summary>
	public Graphic graphic;

	[SerializeField]
	private bool m_IsOn;

	public bool isOn
	{
		get { return m_IsOn; }
		set
		{
			Set(value);
		}
	}

	[SerializeField]
	private ToggleDragGroup m_Group;

	public ToggleDragGroup group
	{
		get { return m_Group; }
		set
		{
			m_Group = value;
#if UNITY_EDITOR
			if (Application.isPlaying)
#endif
			{
				SetToggleGroup(m_Group, true);
			}
		}
	}

	protected override void Start()
	{
		PlayEffect(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		SetToggleGroup(m_Group, false);
	}

	protected override void OnDisable()
	{
		SetToggleGroup(null, false);
		base.OnDisable();
	}

	protected override void OnDidApplyAnimationProperties()
	{
		if (graphic != null)
		{
			bool oldValue = !Mathf.Approximately(graphic.canvasRenderer.GetColor().a, 0);
			if (m_IsOn != oldValue)
			{
				m_IsOn = oldValue;
				Set(!oldValue);
			}
		}

		base.OnDidApplyAnimationProperties();
	}

	private void PlayEffect(bool instant)
	{
		if (graphic == null)
			return;

#if UNITY_EDITOR
		if (!Application.isPlaying)
			graphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
		else
#endif
			graphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
	}

	private void SetToggleGroup(ToggleDragGroup newGroup, bool setMemberValue)
	{
		ToggleDragGroup oldGroup = m_Group;

		// Sometimes IsActive returns false in OnDisable so don't check for it.
		// Rather remove the toggle too often than too little.
		if (m_Group != null)
			m_Group.UnregisterToggle(this);

		// At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
		// That's why we use the setMemberValue parameter.
		if (setMemberValue)
			m_Group = newGroup;

		// Only register to the new group if this Toggle is active.
		if (newGroup != null && IsActive())
			newGroup.RegisterToggle(this);

		// If we are in a new group, and this toggle is on, notify group.
		// Note: Don't refer to m_Group here as it's not guaranteed to have been set.
		if (newGroup != null && newGroup != oldGroup && isOn && IsActive())
			newGroup.NotifyToggleOn(this);
	}

	void Set(bool value)
	{
		Set(value, true);
	}

	void Set(bool value, bool sendCallback)
	{
		if (m_IsOn == value)
			return;

		m_IsOn = value;
		if (m_Group != null && IsActive())
		{
			if (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff))
			{
				m_IsOn = true;
				m_Group.NotifyToggleOn(this);
			}
		}
		PlayEffect(toggleTransition == ToggleTransition.None);
		if (sendCallback)
		{
			onValueChanged.Invoke(m_IsOn);
			onValueChangedInverse.Invoke(!m_IsOn);
		}
	}

	private void InternalToggle()
	{
		if (!IsActive() || !IsInteractable())
			return;

		isOn = !isOn;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		if (eventData.pointerDrag != eventData.pointerEnter)
		{
			onDrag.Invoke(eventData);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		InternalToggle();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Set(false);
		onDragEnd.Invoke(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		if (eventData.dragging && eventData.selectedObject == gameObject)
		{
			Set(true);
			onDragEnabled.Invoke(true, eventData);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!IsActive() || !IsInteractable())
			return;

		if (eventData.dragging && eventData.selectedObject == gameObject)
		{
			onDragEnabled.Invoke(false, eventData);
		}
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		InternalToggle();
	}
}
