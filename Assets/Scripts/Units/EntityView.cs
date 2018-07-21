using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityView : MonoBehaviour
{
	[System.Serializable]
	public class EntityDestroy : UnityEvent<EntityView> { }

	public AudioAssetData assets;

	public Health health;

	[UnityEngine.Serialization.FormerlySerializedAs("animationManager")]
	public UnitGraphics unitGraphics;

	public ReskinAnimation reskinAnimation;

	public EntityDestroy onDestroy = new EntityDestroy();

	public List<Ability> abilities = new List<Ability>();

	public virtual void SelfDestruct(bool splode = true)
	{
		health.onHealthChange.RemoveListener(OnHealthChanged);
		health.SetHitPoints(0);
		if (splode)
		{
			if (FXManager.instance)
			{
				FXManager.instance.AddExplosion(transform.position);
			}
			if (assets)
			{
				AudioPlayer.Instance.PlaySoundClip(assets.clipDestroy, transform);
			}
		}
		onDestroy.Invoke(this);
		Destroy(gameObject);
	}

	public virtual void SetupAbilities(List<AbilityData> abilityDatas)
	{
		foreach (var item in abilityDatas)
		{
			abilities.Add(item.BuildAbility(this, assets));
		}
	}

	protected void OnHealthChanged(GizLib.IntValueChange change)
	{
		if (change.value < 1)
		{
			SelfDestruct(true);
		}
	}
}
