using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxScrollTexture : MonoBehaviour
{
	public float offset;

	public RawImage rawImage;

	public void OnScrollChange(Vector2 value)
	{
		rawImage.uvRect = new Rect(value.x / offset, rawImage.uvRect.y, rawImage.uvRect.width, rawImage.uvRect.height);
	}
}
