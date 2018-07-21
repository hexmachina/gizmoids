using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankDamage : ColliderAbility, IAbility, ISpawnable, IMovable, IPausable
{

	public int power;
	public float delay;

	public float flankDistance;

	public Mover mover;

	public GameObject spawnPrefab;

	private GameObject spawn;

	private bool detouring;

	private bool firing;

	private bool paused;

	public void AttachSpawn(GameObject spawn)
	{
		spawnPrefab = spawn;
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Power"))
		{
			power = (int)valuePairs["Power"];
		}
		if (valuePairs.ContainsKey("Delay"))
		{
			delay = valuePairs["Delay"];
		}
		if (valuePairs.ContainsKey("FlankDistance"))
		{
			flankDistance = valuePairs["FlankDistance"];
		}
		onTargetSighted.AddListener(OnTargetChanged);
	}

	public void SetMover(Mover mover)
	{
		this.mover = mover;
		this.mover.onDetour.AddListener(FlankRange);
	}

	protected override void TargetEntered(Collider2D collision)
	{
		if (paused)
		{
			return;
		}

		if (!targeting)
		{
			targeting = true;

			StartCoroutine(Targeting());
		}
		Debug.Log(Vector3.Distance(transform.position, collision.transform.position));
		if (!detouring && Vector3.Distance(transform.position, collision.transform.position) < flankDistance)
		{
			FlankTarget(collision.transform);
		}
		else
		{
			mover.moving = false;
			onTargetSighted.Invoke(true);
			if (!firing)
			{
				StartCoroutine(Fire());
			}
		}
	}

	public void OnTargetChanged(bool targetChanged)
	{
		SpawnObject(targetChanged);
		if (!targetChanged && !mover.moving && !paused)
		{
			mover.moving = true;
		}
	}

	public void FlankTarget(Transform flanked)
	{
		detouring = true;

		var radius = Vector3.Distance(mover.transform.position, Vector3.zero) + flankDistance;

		var target = Mathfx.GetPointOnCircle(radius, mover.transform.eulerAngles.z + 90, Vector3.zero);
		mover.StartMovement(MoveType.Detour, mover.speed * 20, target);
	}

	public void FlankRange(bool detour)
	{
		if (!detour)
		{
			detouring = false;
			if (targeting)
			{
				mover.moving = false;
				if (!firing)
				{
					StartCoroutine(Fire());
				}
			}
		}
	}

	IEnumerator Fire()
	{
		firing = true;
		while (targeting && !paused)
		{
			yield return new WaitForSeconds(delay);
			if (targeting && !paused)
			{
				if (assetData)
				{
					AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);
				}

				AffectTargets();
			}
		}
		firing = false;
	}



	public void SpawnObject(bool active)
	{
		if (active)
		{
			if (spawn)
			{
				if (!spawn.gameObject.activeSelf)
				{
					spawn.gameObject.SetActive(active);
				}
			}
			else
			{
				spawn = Instantiate(spawnPrefab, transform);
				spawn.transform.localPosition = new Vector3(0, -0.13f, 0);
			}
		}
		else
		{
			if (spawn && spawn.gameObject.activeSelf)
			{
				spawn.gameObject.SetActive(active);
			}
		}
	}

	public void AffectTargets()
	{
		var contacts = GetContacts();

		for (int i = 0; i < contacts.Length; i++)
		{
			if (targetTags.Contains(contacts[i].gameObject.tag))
			{
				var health = contacts[i].GetComponent<Health>();
				if (!health && contacts[i].transform.parent != null)
				{
					health = contacts[i].GetComponentInParent<Health>();
				}
				if (health)
				{
					health.TakeDamage(power);
				}
				var impactable = contacts[i].GetComponent<IImpactable>();
				if (impactable != null)
				{
					impactable.Impact(gameObject);
				}
			}
		}
	}

	public void SetPaused(bool isPaused)
	{
		paused = isPaused;
		if (paused)
		{
			SpawnObject(false);
		}
	}

	public void Pause(float duration)
	{
		throw new System.NotImplementedException();
	}
}
