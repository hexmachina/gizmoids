using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GizmoidView : EntityView, IPausable, IOverclockable
{
	[System.Serializable]
	public class OrderEvent : UnityEvent<int> { }

	public OrderEvent onOrderChanged = new OrderEvent();

	private List<IOverclockable> overclockables = new List<IOverclockable>();

	public void OrderChanged(int order)
	{
		unitGraphics.SetSortingLayer("Gizmoid" + order);
		onOrderChanged.Invoke(order);
	}

	public void SetData(GizmoidData data, int order, UnitGraphics preview = null)
	{
		name = data.nameInternal;
		assets = data.assetData;
		if (preview)
		{
			SetupGraphics(preview, false, order);
		}
		else
		{
			SetupGraphics(data.unitGraphics, true, order);
		}
		SetupReskin(data.nameInternal, data.maxHealth, unitGraphics.renderers);
		SetupHealth(data.maxHealth);
		SetupAbilities(data.abilities);
		foreach (var item in abilities)
		{
			var arenaCollider = item.GetComponent<ArenaCollider>();
			if (arenaCollider)
			{
				arenaCollider.BuildColliders(order);
				onOrderChanged.AddListener(arenaCollider.BuildColliders);
			}
		}

		CollectOverclockables();

	}

	public void CollectOverclockables()
	{
		if (health is IOverclockable)
		{
			overclockables.Add(health);
		}

		if (unitGraphics is IOverclockable)
		{
			overclockables.Add(unitGraphics);
		}

		foreach (var item in abilities)
		{
			if (item is IOverclockable)
			{
				overclockables.Add(item as IOverclockable);
			}
		}
	}

	public void SetupGraphics(UnitGraphics animMan, bool instant, int order)
	{
		if (instant)
		{
			unitGraphics = Instantiate(animMan);
		}
		else
		{
			unitGraphics = animMan;
		}
		unitGraphics.transform.SetParent(transform);
		unitGraphics.transform.localPosition = Vector3.zero;
		unitGraphics.transform.localRotation = Quaternion.identity;
		unitGraphics.SetSortingLayer("Gizmoid" + order);
	}

	public void SetupReskin(string handle, int maxHitpoints, List<SpriteRenderer> sRenderers)
	{
		if (!reskinAnimation)
		{
			reskinAnimation = GetComponent<ReskinAnimation>();
		}
		reskinAnimation.Setup("Gizmoids", handle, maxHitpoints / 3);
		reskinAnimation.renderers = sRenderers;
		if (handle == "Crab")
		{
			reskinAnimation.spriteSheets.Add("Main_Blink_L");
			reskinAnimation.spriteSheets.Add("Main_Blink_R");
		}
		else if (handle == "Frog")
		{
			reskinAnimation.spriteSheets.Add("Main_Bubbles");
			reskinAnimation.spriteSheets.Add("Main_Mouth");
		}
		else if (handle == "Healer")
		{
			reskinAnimation.spriteSheets.Add("Main_Mouth");
			reskinAnimation.spriteSheets.Add("Main_Blink");
		}
		else
		{
			reskinAnimation.spriteSheets.Add("Main_Blink");
		}
	}

	public void SetupHealth(int maxHitPoints)
	{
		if (!health)
		{
			GetComponent<Health>();
			if (!health)
			{
				health = gameObject.AddComponent<Health>();
			}
		}
		health.SetFullHealth(maxHitPoints);
		health.onHealthChange.AddListener(OnHealthChanged);
		if (unitGraphics)
		{
			health.onHealthChange.AddListener(unitGraphics.OnHealthChanged);
		}
		if (reskinAnimation)
		{
			health.onHealthChange.AddListener(reskinAnimation.OnValueChanged);
		}
	}

	public void SetPaused(bool isPaused)
	{
		unitGraphics.anim.enabled = isPaused;
		var coll = GetComponent<Collider2D>();
		if (coll)
		{
			coll.enabled = isPaused;
		}
		foreach (var item in abilities)
		{
			var abColl = item.GetComponent<Collider2D>();
			if (abColl)
			{
				abColl.enabled = isPaused;
			}
		}
	}

	public void Pause(float duration)
	{
		throw new System.NotImplementedException();
	}

	public void SetOverclock(bool active)
	{
		foreach (var item in overclockables)
		{
			item.SetOverclock(active);
		}
	}

	public void Overclocking(float duration)
	{
		StartCoroutine(Overclock(duration));
	}

	private IEnumerator Overclock(float duration)
	{
		SetOverclock(true);
		yield return new WaitForSeconds(duration);
		SetOverclock(false);
	}
}
