using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class CircumferenceUIContainer : UIBehaviour
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
	private bool m_useRadius;
	public bool useRadius { get { return m_useRadius; } set { SetProperty(ref m_useRadius, value); } }

	[SerializeField]
	private float m_radiusOffset;
	public float radiusOffset { get { return m_radiusOffset; } set { SetProperty(ref m_radiusOffset, value); } }

	[FormerlySerializedAs("radius")]
	[SerializeField]
	private float m_radius;
	public float radius { get { return m_radius; } set { SetProperty(ref m_radius, value); } }

	[SerializeField]
	[Range(0, 360)]
	private float m_offset;
	public float offset { get { return m_offset; } set { SetProperty(ref m_offset, value); } }

	[SerializeField]
	private float m_spacing;
	public float spacing { get { return m_spacing; } set { SetProperty(ref m_spacing, value); } }

	[SerializeField]
	private Vector2 m_originOffset;
	public Vector2 originOffset { get { return m_originOffset; } set { SetProperty(ref m_originOffset, value); } }

	[FormerlySerializedAs("m_counterClockwise")]
	[SerializeField]
	private bool m_clockwise = true;
	public bool clockwise { get { return m_clockwise; } set { SetProperty(ref m_clockwise, value); } }

	private readonly float offsetIntertnal = -90;

	[NonSerialized] private List<RectTransform> m_RectChildren = new List<RectTransform>();
	protected List<RectTransform> rectChildren { get { return m_RectChildren; } }

	private void SetRadius(float value)
	{
		if (value == m_radius)
			return;

		m_radius = value;
		PlaceItems();
	}

	public Vector2 PointOnCircle(float rad, float angleInDegrees, Vector2 circleOrigin)
	{
		if (clockwise)
		{
			angleInDegrees -= offsetIntertnal;
		}
		else
		{
			angleInDegrees += offsetIntertnal;
			angleInDegrees = 360 - angleInDegrees;
		}

		float x = (rad * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + circleOrigin.x;
		float y = (rad * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + circleOrigin.y;

		return new Vector2(x, y);
	}

	public void PlaceItems()
	{
		if (transform.childCount != rectChildren.Count)
		{
			GetRectChildren();
		}
		float scaleFactor = 1;
		float itemSpacing = m_spacing;
		var origin = Vector2.zero;
		var rt = transform as RectTransform;
		if (rt.root != null)
		{
			var canvas = rt.root.GetComponent<Canvas>();
			if (canvas)
			{
				scaleFactor = canvas.scaleFactor;
				if (!useRadius)
				{
					var widthRadius = (rt.rect.width * scaleFactor) / 2f;
					var hieghtRadius = (rt.rect.height * scaleFactor) / 2f;

					if (widthRadius < hieghtRadius)
					{
						m_radius = widthRadius;
					}
					else
					{
						m_radius = hieghtRadius;
					}

					m_radius -= m_radiusOffset;
				}
				origin = new Vector2(rt.anchoredPosition.x + ((rt.rect.width * scaleFactor) / 2), rt.anchoredPosition.y + ((rt.rect.height * scaleFactor) / 2));
				itemSpacing *= canvas.scaleFactor;
			}
		}
		origin = new Vector2(origin.x + originOffset.x, origin.y + originOffset.y);
		float totalsize = 0;
		for (int i = 0; i < m_RectChildren.Count; i++)
		{
			var size = m_RectChildren[i].rect.size.x;
			if (size > m_RectChildren[i].rect.size.y)
			{
				size = m_RectChildren[i].rect.size.y;
			}
			size = (radius / (size / 2)) * scaleFactor;
			if (i == 0)
			{
				//Debug.Log(size);
			}
			totalsize += size;
			m_RectChildren[i].position = PointOnCircle(m_radius, m_offset + ((m_spacing) * i), origin);
		}
	}

	public void GetRectChildren()
	{
		m_RectChildren.Clear();
		for (int i = 0; i < rectTransform.childCount; i++)
		{
			var rect = rectTransform.GetChild(i) as RectTransform;
			if (rect == null || !rect.gameObject.activeInHierarchy)
				continue;

			m_RectChildren.Add(rect);
		}
	}

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		SetDirty();
	}

	protected virtual void OnTransformChildrenChanged()
	{
		SetDirty();
	}

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
			PlaceItems();
		}
		else
			StartCoroutine(DelayedSetDirty(rectTransform));
	}

	IEnumerator DelayedSetDirty(RectTransform rectTransform)
	{
		yield return null;
		LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		PlaceItems();
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		SetDirty();
	}

#endif
}
