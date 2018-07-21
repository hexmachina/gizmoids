using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour, IOverclockable
{

	//public EnemySheet enemySheet;
	public int maxHitpoints;
	public int hitPoints;

	[UnityEngine.Serialization.FormerlySerializedAs("overclocked")]
	public bool invulnerable;

	public GizLib.UnityEventIntValueChange onHealthChange;

	void Awake()
	{
	}

	void Start()
	{
		if (onHealthChange == null)
		{
			onHealthChange = new GizLib.UnityEventIntValueChange();
		}
	}

	public void TakeDamage(int damage)
	{
		if (!invulnerable)
		{
			var last = hitPoints;
			hitPoints -= damage;
			if (hitPoints < 0)
			{
				hitPoints = 0;
			}
			HealthChanged(last);
		}
	}

	public void SetHitPoints(int value)
	{
		var old = hitPoints;
		hitPoints = value;
		if (old != hitPoints)
		{
			HealthChanged(old);
		}
	}

	public void Heal(int points)
	{
		var last = hitPoints;

		hitPoints += points;
		if (hitPoints > maxHitpoints)
		{
			hitPoints = maxHitpoints;
		}
		HealthChanged(last);
	}

	public void RestoreHealth()
	{
		var last = hitPoints;

		hitPoints = maxHitpoints;
		HealthChanged(last);
	}

	public void OnOverclocked(bool clocking)
	{
		invulnerable = clocking;
		if (invulnerable)
		{
			RestoreHealth();
		}
	}

	public void SetFullHealth(int max)
	{
		hitPoints = maxHitpoints = max;
	}

	private void HealthChanged(int last)
	{
		if (last == hitPoints)
		{
			return;
		}

		onHealthChange.Invoke(new GizLib.IntValueChange()
		{
			value = hitPoints,
			lastValue = last,
			maxValue = maxHitpoints

		});
	}

	public void SetOverclock(bool active)
	{
		invulnerable = active;
		if (invulnerable)
		{
			RestoreHealth();
		}
	}

	public void Overclocking(float duration)
	{

	}
}
