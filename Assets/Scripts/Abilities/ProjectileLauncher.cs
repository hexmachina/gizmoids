using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLauncher : ColliderAbility, IAbility, ISpawnable, IListenable
{
	public int power;
	public float firingRate;
	public float fireDelay;
	public float projectileSpeed;

	public Projectile projectilePrefab;

	public UnityEvent onLaunch = new UnityEvent();

	private bool firing;

	private Dictionary<string, float> fields = new Dictionary<string, float>();

	public void AttachSpawn(GameObject spawn)
	{
		var projectile = spawn.GetComponent<Projectile>();
		if (projectile)
		{
			projectilePrefab = projectile;
		}
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Power"))
		{
			power = (int)valuePairs["Power"];
		}
		if (valuePairs.ContainsKey("Rate"))
		{
			firingRate = valuePairs["Rate"];
		}
		if (valuePairs.ContainsKey("Delay"))
		{
			fireDelay = valuePairs["Delay"];
		}
		if (valuePairs.ContainsKey("ProjectileSpeed"))
		{
			projectileSpeed = valuePairs["ProjectileSpeed"];
		}
		fields = valuePairs;
	}

	protected override void TargetEntered(Collider2D collision)
	{
		if (!targeting)
		{
			targeting = true;
			if (!firing)
			{
				firing = true;
				StartCoroutine(Fire());
			}

			onTargetSighted.Invoke(true);
			StartCoroutine(Targeting());
		}
	}

	IEnumerator Fire()
	{
		while (targeting)
		{
			onLaunch.Invoke();
			yield return new WaitForSeconds(fireDelay);
			if (targeting)
			{
				AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);

				SpawnProjectile();

				yield return new WaitForSeconds(firingRate);
			}
		}
		firing = false;
	}

	void SpawnProjectile()
	{
		if (!projectilePrefab)
		{
			return;
		}

		var p = ProjectilePool.Instance.GetProjectile(projectilePrefab.GetInstanceID());

		if (p)
		{
			p.gameObject.SetActive(true);
			p.transform.position = transform.position;
		}
		else
		{
			p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			p.name = projectilePrefab.name;
			p.spawnId = projectilePrefab.GetInstanceID();
			p.impactClip = assetData.clipUtility2;
			p.targetTags = targetTags;
			p.power = power;

			if (p is IAbility)
			{
				(p as IAbility).SetFields(fields);
			}

			ProjectilePool.Instance.AddProjectile(p);
		}

		p.transform.eulerAngles = transform.eulerAngles;
		p.mover.speed = projectileSpeed;
		p.Orient();
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void AddGraphicsListener(UnitGraphics unitGraphics)
	{
		onLaunch.AddListener(unitGraphics.OnActivePlay);
	}
}
