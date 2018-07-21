using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ReskinAnimation : MonoBehaviour
{

	public string unitype;
	public string handle;
	public List<string> spriteSheets;
	public List<SpriteRenderer> renderers;
	public float triggerIncrement;
	public int sheetIndex = 0;
	public bool change = false;

	public void Setup(string unit, string name, int trigger)
	{
		triggerIncrement = trigger;
		unitype = unit;
		handle = name;
		sheetIndex = 0;

		spriteSheets.Add("Main");

		if (handle == "Gultear")
		{
			spriteSheets.Add("Main_Wing");
		}
	}

	public void Reskin()
	{
		foreach (var spriteSheet in spriteSheets)
		{
			var subSprites = Resources.LoadAll<Sprite>(unitype + "/" + handle + "/" + spriteSheet + "_BD" + sheetIndex);
			foreach (var renderer in renderers)
			{
				if (renderer != null)
				{
					string spriteName = renderer.sprite.name;
					var newSprite = Array.Find(subSprites, item => item.name == spriteName);

					if (newSprite)
					{
						renderer.sprite = newSprite;
					}
				}
			}
		}
	}

	public void SetDamageLevel(int h)
	{
		int currentDamageLevel = sheetIndex;
		int div = (int)(h / triggerIncrement);
		if (h % triggerIncrement != 0)
		{
			div++;
		}
		int level = 3 - div;
		if (level > 2)
		{
			level = 2;
		}
		sheetIndex = level;
		if (sheetIndex != currentDamageLevel)
		{
			change = true;
		}

	}

	public void OnValueChanged(GizLib.IntValueChange change)
	{
		var currentDamageLevel = sheetIndex;
		if (change.value < triggerIncrement)
		{
			sheetIndex = 2;
		}
		else if (change.value == change.maxValue)
		{
			sheetIndex = 0;
		}
		else
		{
			sheetIndex = 3 - (int)(change.value / triggerIncrement);
			if (change.value % triggerIncrement > 0)
			{
				sheetIndex--;
			}
			if (sheetIndex < 0)
			{
				sheetIndex = 0;
			}
		}
		if (sheetIndex != currentDamageLevel)
		{
			Reskin();
		}
	}

	public void Restore()
	{
		sheetIndex = 0;
		change = true;
	}
}
