using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class ForceFlexibleLayout : UIBehaviour, ILayoutSelfController
{
	[NonSerialized] private RectTransform m_Rect;
	protected RectTransform rectTransform
	{
		get
		{
			if (m_Rect == null)
				m_Rect = GetComponent<RectTransform>();
			return m_Rect;
		}
	}

	[SerializeField]
	private bool m_width = true;
	public bool width { get { return m_width; } set { SetProperty(ref m_width, value); } }

	[SerializeField]
	private bool m_height = true;
	public bool height { get { return m_height; } set { SetProperty(ref m_height, value); } }

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		SetDirty();

	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		SetDirty();
	}

	protected override void OnEnable()
	{
		SetDirty();
	}

	public void CalculateSize()
	{
		if (!height && !width)
		{
			return;
		}
		var prt = transform.parent as RectTransform;

		float widthOffset = 0;
		float heightOffset = 0;
		for (int i = 0; i < prt.childCount; i++)
		{
			var child = prt.GetChild(i);
			if (child != transform)
			{
				if (width)
				{
					widthOffset += (child as RectTransform).rect.width;
				}

				if (height)
				{
					heightOffset += (child as RectTransform).rect.height;
				}
			}
		}

		var layout = prt.GetComponent<HorizontalOrVerticalLayoutGroup>();
		if (layout)
		{
			if (width)
			{
				widthOffset += layout.padding.left + layout.padding.right;
				if (layout is HorizontalLayoutGroup)
				{
					widthOffset += layout.spacing * (prt.childCount - 1);
				}
			}

			if (height)
			{
				heightOffset += layout.padding.top + layout.padding.bottom;
				if (layout is VerticalLayoutGroup)
				{
					heightOffset += layout.spacing * (prt.childCount - 1);
				}
			}
		}
		var rt = transform as RectTransform;
		rt.sizeDelta = new Vector2(prt.rect.width - widthOffset, prt.rect.height - heightOffset);
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		SetDirty();
	}

#endif

	protected void SetProperty<T>(ref T currentValue, T newValue)
	{
		if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			return;
		currentValue = newValue;
		SetDirty();
	}

	protected void SetDirty()
	{
		if (!IsActive())
			return;


		if (!CanvasUpdateRegistry.IsRebuildingLayout())
		{
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			CalculateSize();
		}
		else
			StartCoroutine(DelayedSetDirty(rectTransform));
	}

	IEnumerator DelayedSetDirty(RectTransform rectTransform)
	{
		yield return null;
		LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		CalculateSize();
	}

	public void SetLayoutHorizontal()
	{
	}

	public void SetLayoutVertical()
	{
	}
}
