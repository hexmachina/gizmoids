using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyView : EntityView, IImpactable, IPausable
{
	[System.Serializable]
	public class ImpactEvent : UnityEvent<Transform> { }

	public bool paused;
	public EnemyData enemyData;
	public Mover mover;
	public AudioClip destroyedClip;

	public ImpactEvent onImpact = new ImpactEvent();

	public void SetData(EnemyData data)
	{
		enemyData = data;
		gameObject.name = enemyData.nameFull;

		SetupGraphics(enemyData.unitGraphics);

		health.SetFullHealth(enemyData.maxHealth);
		health.onHealthChange.AddListener(OnHealthChanged);
		health.onHealthChange.AddListener(unitGraphics.OnHealthChanged);

		reskinAnimation = GetComponentInChildren<ReskinAnimation>();
		if (reskinAnimation && enemyData.canReskin)
		{
			reskinAnimation.renderers = unitGraphics.renderers;
			reskinAnimation.Setup("Enemies", enemyData.nameInternal, enemyData.maxHealth / 3);
			health.onHealthChange.AddListener(reskinAnimation.OnValueChanged);
		}

		var cap = GetComponent<CapsuleCollider2D>();
		if (cap)
		{
			cap.size = data.colliderSize;
			cap.direction = data.capsuleDirection;
		}

		SetupAbilities(enemyData.abilities);
		foreach (var item in abilities)
		{
			if (item is IMovable)
			{
				((IMovable)item).SetMover(mover);
			}
		}
	}

	public void SetupGraphics(UnitGraphics animPrefab)
	{

		unitGraphics = Instantiate(animPrefab, Vector3.zero, Quaternion.identity, transform);
		unitGraphics.SetSortingLayer("Enemies");
		unitGraphics.transform.localRotation = Quaternion.identity;
		unitGraphics.transform.localPosition = Vector3.zero;

		if (unitGraphics is DebrisGraphics)
		{
			var debris = unitGraphics as DebrisGraphics;
			debris.SetDebrisType(enemyData.nameInternal);
			onImpact.AddListener(debris.BuildParticle);
			mover.onDetour.AddListener(debris.OnDetour);
		}
	}

	public void Impact(GameObject gameObject)
	{
		onImpact.Invoke(gameObject.transform);
	}

	public void SetPaused(bool isPaused)
	{
		paused = isPaused;

		if (unitGraphics is DebrisGraphics)
		{
			unitGraphics.enabled = !isPaused;
		}

		mover.moving = !isPaused;
		unitGraphics.anim.enabled = !isPaused;

		foreach (var item in abilities)
		{
			if (item is IPausable)
			{
				(item as IPausable).SetPaused(isPaused);
			}
		}
		if (!isPaused)
		{
			unitGraphics.ShowNormal();
		}
	}

	public void Pause(float duration)
	{
		StartCoroutine(Pausing(duration));
	}

	IEnumerator Pausing(float duration)
	{
		SetPaused(true);
		yield return new WaitForSeconds(duration);
		SetPaused(false);
	}
}
