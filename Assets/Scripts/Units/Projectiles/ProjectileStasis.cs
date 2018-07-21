using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStasis : Projectile, IAbility
{
	public enum StasisState
	{
		Random,
		Normal,
		Stasis
	}

	public StasisState state;

	public float stasisChance;

	public float stasisDuration = 4;

	public Color colorNormal = Color.white;
	public Vector3 scaleNormal = Vector3.one;

	public Color colorStasis = Color.white;
	public Vector3 scaleStasis = Vector3.one;

	public SpriteRenderer spriteRenderer;

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		throw new System.NotImplementedException();
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("StasisChance"))
		{
			stasisChance = valuePairs["StasisChance"];
		}

		if (valuePairs.ContainsKey("StasisDuration"))
		{
			stasisDuration = valuePairs["StasisDuration"];
		}
	}

	private void OnEnable()
	{
		state = StasisState.Random;
		SetupState();
	}

	public void SetupState()
	{
		if (state == StasisState.Random)
		{
			if (Random.value < stasisChance)
			{
				state = StasisState.Stasis;
			}
			else
			{
				state = StasisState.Normal;
			}
		}

		switch (state)
		{
			case StasisState.Normal:
				spriteRenderer.color = colorNormal;
				spriteRenderer.transform.localScale = scaleNormal;
				break;
			case StasisState.Stasis:
				spriteRenderer.color = colorStasis;
				spriteRenderer.transform.localScale = scaleStasis;
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetTags.Contains(collision.gameObject.tag))
		{
			var impact = collision.GetComponent<IImpactable>();
			if (impact != null)
			{
				impact.Impact(collision.gameObject);
			}

			if (state == StasisState.Stasis)
			{
				var pauser = collision.GetComponent<IPausable>();
				if (pauser != null)
				{
					pauser.Pause(stasisDuration);
				}

				var en = collision.GetComponent<EntityView>();
				if (en)
				{
					if (collision.transform.Find("Bubble") == null)
					{
						var bub = Instantiate(spriteRenderer, collision.transform);
						bub.name = "Bubble";
						bub.transform.localPosition = Vector3.zero;
						bub.transform.localScale = new Vector3(0.8f, 0.8f);
						bub.sortingLayerName = en.unitGraphics.animSortingLayer;
						bub.sortingOrder = en.unitGraphics.GetHighestSortingOrder() + 1;
						var des = bub.gameObject.AddComponent<Destroy>();
						des.delay = stasisDuration;
					}
				}
			}
			else
			{
				var health = collision.GetComponent<Health>();
				if (!health)
				{
					health = collision.GetComponentInParent<Health>();
				}
				if (health)
				{
					Impact(health);
				}
			}
			onImpact.Invoke(collision.gameObject);

			gameObject.SetActive(false);
		}
	}
}
