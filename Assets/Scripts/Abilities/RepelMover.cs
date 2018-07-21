using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepelMover : ColliderAbility, IAbility, IListenable
{
	public float prepTime = 1.75f;
	public float anticipation = 0.25f;
	public float recoverTime;

	public float repelSpeed;
	public int repelSpan;

	public UnityEvent onPrep = new UnityEvent();

	public UnityEvent onLaunch = new UnityEvent();

	public UnityEvent onRecover = new UnityEvent();

	private bool repelling;

	protected override void TargetEntered(Collider2D collision)
	{
		if (!targeting)
		{
			targeting = true;
			if (!repelling)
			{
				repelling = true;
				StartCoroutine(Popping());
			}

			onTargetSighted.Invoke(true);
			StartCoroutine(Targeting());
		}
	}

	IEnumerator Popping()
	{
		AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);
		onPrep.Invoke();
		yield return new WaitForSeconds(prepTime);
		if (targeting)
		{
			onLaunch.Invoke();
			yield return new WaitForSeconds(anticipation);

			PushAway();
			yield return new WaitForSeconds(recoverTime);
			onRecover.Invoke();
		}
		repelling = false;
		if (targeting)
		{
			repelling = true;
			StartCoroutine(Popping());
		}
	}

	private void CheckAfterRecover()
	{
		var contacts = GetContacts();
		for (int i = 0; i < contacts.Length; i++)
		{
			if (targetTags.Contains(contacts[i].gameObject.tag))
			{
				if (!repelling)
				{
					repelling = true;
					StartCoroutine(Popping());
				}
				break;
			}
		}
	}

	void PushAway()
	{
		var contacts = GetContacts();

		var sectors = new List<SectorHandler>();
		for (int i = 0; i < contacts.Length; i++)
		{
			var sector = contacts[i].GetComponent<SectorHandler>();
			if (sector)
			{
				sectors.Add(sector);
			}
		}

		if (sectors.Count == 0)
		{
			return;
		}

		SectorHandler sect = sectors[0];
		for (int i = 1; i < sectors.Count; i++)
		{
			if (Mathf.Abs(sectors[i].transform.eulerAngles.z - transform.eulerAngles.z) < Mathf.Abs(sect.transform.eulerAngles.z - transform.eulerAngles.z))
			{
				sect = sectors[i];
			}
		}

		for (int i = 0; i < contacts.Length; i++)
		{
			if (targetTags.Contains(contacts[i].gameObject.tag))
			{
				var mover = contacts[i].GetComponent<Mover>();
				if (!mover)
				{
					mover = contacts[i].GetComponentInParent<Mover>();
				}
				if (mover)
				{
					var radius = Vector3.Distance(mover.transform.position, Vector3.zero) + (0.555f * repelSpan);
					var target = Mathfx.GetPointOnCircle(radius + (UnityEngine.Random.value * 0.1f), sect.transform.eulerAngles.z + 90, Vector3.zero);

					mover.StartMovement(MoveType.Detour, repelSpeed, target);
				}
			}
		}
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Rate"))
		{
			recoverTime = valuePairs["Rate"];
		}
		if (valuePairs.ContainsKey("RepelSpeed"))
		{
			repelSpeed = valuePairs["RepelSpeed"];
		}
		if (valuePairs.ContainsKey("RepelSpan"))
		{
			repelSpan = (int)valuePairs["RepelSpan"];
		}
		if (valuePairs.ContainsKey("Prep"))
		{
			prepTime = valuePairs["Prep"];
		}
		if (valuePairs.ContainsKey("Anticipation"))
		{
			anticipation = valuePairs["Anticipation"];
		}
	}

	public void AddGraphicsListener(UnitGraphics unitGraphics)
	{
		onLaunch.AddListener(unitGraphics.OnActivePlay);
		onPrep.AddListener(unitGraphics.OnPrepPlay);
		onRecover.AddListener(unitGraphics.OnRecoverTrigger);
	}

	public void AddLastActionListener(Action callback)
	{

	}
}
