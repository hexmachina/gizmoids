using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrollRectTransform : MonoBehaviour
{

	public float offset;

	public void OnScrollChange(Vector2 value)
	{
		var rt = transform as RectTransform;
		rt.anchoredPosition = new Vector2(value.x * offset, rt.anchoredPosition.y);
	}
}
