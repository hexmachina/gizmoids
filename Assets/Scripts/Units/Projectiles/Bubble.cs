using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BubbleType
{
	Normal = 0,
	Random = 1,
	Stasis = 2
}

public class Bubble : MonoBehaviour
{
	public BubbleType bubbleType;

	public float statisDuration = 4;

	//public int power;
	public List<Color> normalColors;
	public List<Color> stasisColors;

	public float stasisChance;
	public Projectile projectile;

	public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake()
	{
		if (!projectile)
		{
			projectile = GetComponent<Projectile>();
		}
		if (projectile)
		{
			projectile.onImpact.AddListener(Impact);
		}
	}

	private void OnEnable()
	{
		bubbleType = BubbleType.Random;
		SetupBubble();
	}

	public void SetupBubble()
	{
		if (bubbleType == BubbleType.Random)
		{
			if (Random.value < stasisChance)
			{
				bubbleType = BubbleType.Stasis;
				print("Stasis");
			}
			else
			{
				bubbleType = BubbleType.Normal;
			}
		}

		switch (bubbleType)
		{
			case BubbleType.Normal:
				spriteRenderer.material.color = normalColors[0];
				spriteRenderer.transform.localScale = new Vector3(0.5f, 0.5f);

				break;
			case BubbleType.Stasis:
				spriteRenderer.material.color = stasisColors[0];
				spriteRenderer.transform.localScale = new Vector3(0.65f, 0.65f);
				break;
			default:
				break;
		}
	}

	public void Impact(GameObject impacted)
	{
		if (bubbleType == BubbleType.Stasis)
		{
			var pauser = impacted.GetComponent<IPausable>();
			if (pauser != null)
			{
				pauser.Pause(statisDuration);
			}

			var en = impacted.GetComponent<EntityView>();
			if (en)
			{
				if (impacted.transform.Find("Bubble") == null)
				{
					var bub = Instantiate(spriteRenderer, impacted.transform);
					bub.name = "Bubble";
					bub.transform.localPosition = Vector3.zero;
					bub.transform.localScale = new Vector3(0.8f, 0.8f);
					bub.sortingLayerName = en.unitGraphics.animSortingLayer;
					int order = 0;
					foreach (var rend in en.unitGraphics.renderers)
					{
						if (rend.sortingOrder > order)
						{
							order = rend.sortingOrder;
						}
					}
					bub.sortingOrder = order + 1;
					var des = bub.gameObject.AddComponent<Destroy>();
					des.delay = statisDuration;
				}
			}
		}
	}
}
