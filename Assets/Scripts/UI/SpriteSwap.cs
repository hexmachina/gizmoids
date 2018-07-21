using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour
{
	public Image image;

	public Sprite onSprite;

	public Sprite offSprite;

	public void SwapSprite(bool swap)
	{
		if (swap)
		{
			image.sprite = onSprite;
		}
		else
		{
			image.sprite = offSprite;
		}
	}

}
