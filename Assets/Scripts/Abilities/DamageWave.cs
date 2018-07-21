using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageWave : ColliderAbility, IAbility, ISpawnable, IListenable
{
	public int power;
	public float firingRate;
	public float delay;

	public ParticleSystem particlePrefab;

	private ParticleSystem particle;

	private AudioSource audioSource;

	private bool firing;

	public UnityEvent onLaunch = new UnityEvent();

	private UnitGraphics unitGraphics;

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
			delay = valuePairs["Delay"];
		}
	}

	public void MusicLoop(bool active)
	{
		if (active)
		{
			if (!audioSource)
			{
				audioSource = AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1);
				audioSource.loop = true;
			}
			unitGraphics.OnActivateTrigger();
		}
		else
		{
			unitGraphics.OnRecoverTrigger();
			if (audioSource)
			{
				audioSource.loop = false;
				audioSource.Stop();
				audioSource = null;
			}
			if (particle && particle.isPlaying)
			{
				particle.Stop();
			}
		}
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

			StartCoroutine(Targeting());
			//onTargetSighted.Invoke(true);
		}
	}

	IEnumerator Fire()
	{

		MusicLoop(true);
		while (targeting)
		{
			onLaunch.Invoke();

			yield return new WaitForSeconds(delay);
			if (!particle)
			{
				particle = BuildParticles();
			}
			if (particle)
			{
				particle.Play();
			}
			if (targeting)
			{
				AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility2, transform);

				AffectTargets();

				yield return new WaitForSeconds(firingRate);
			}
		}
		MusicLoop(false);
		firing = false;
	}

	public void AffectTargets()
	{
		var contacts = GetContacts();

		for (int i = 0; i < contacts.Length; i++)
		{
			if (targetTags.Contains(contacts[i].gameObject.tag))
			{
				var health = contacts[i].GetComponent<Health>();
				if (!health)
				{
					health = contacts[i].GetComponentInParent<Health>();
				}
				if (health)
				{
					health.TakeDamage(power);
				}
			}
		}
	}

	private ParticleSystem BuildParticles()
	{
		if (!particlePrefab)
		{
			return null;
		}
		var part = Instantiate(particlePrefab, transform);
		part.transform.localPosition = new Vector3(0, 0.36f, 0);
		part.transform.localEulerAngles = new Vector3(-90, 0, 0);
		return part;
	}

	public void AttachSpawn(GameObject spawn)
	{
		var part = spawn.GetComponent<ParticleSystem>();
		if (part)
		{
			particlePrefab = part;
		}
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void AddGraphicsListener(UnitGraphics unitGraphics)
	{
		this.unitGraphics = unitGraphics;
		//onTargetSighted.AddListener(unitGraphics.OnActivateBool);
	}

}
