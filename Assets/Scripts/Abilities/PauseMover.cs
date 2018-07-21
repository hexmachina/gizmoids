using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMover : ColliderAbility, IAbility, ISpawnable, IListenable, ILastActionable
{

	public float freezeDuration;
	public float delay;

	public Hourglass hourglass;

	public GameObject finalBlast;

	public UnityEvent onLastAction = new UnityEvent();

	public UnityEvent onLaunch = new UnityEvent();

	private bool freezing;

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Delay"))
		{
			delay = valuePairs["Delay"];
		}
		if (valuePairs.ContainsKey("Duration"))
		{
			freezeDuration = valuePairs["Duration"];
		}
	}

	protected override void TargetEntered(Collider2D collision)
	{
		if (!targeting)
		{
			targeting = true;
			if (!freezing)
			{
				freezing = true;
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
			yield return new WaitForSeconds(delay);
			if (targeting)
			{
				AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);

				AffectTargets();

				var blast = Instantiate(finalBlast);
				blast.transform.position = transform.position;
				blast.GetComponent<Renderer>().sortingLayerName = "Projectiles";

				onLastAction.Invoke();
			}
		}
		freezing = false;
	}

	public void AffectTargets()
	{
		var contacts = GetContacts();

		for (int i = 0; i < contacts.Length; i++)
		{

			if (targetTags.Contains(contacts[i].gameObject.tag))
			{
				var enemy = contacts[i].GetComponent<EntityView>();
				if (!enemy)
				{
					enemy = contacts[i].GetComponentInParent<EntityView>();
				}
				if (enemy)
				{
					BuildHourGlass(enemy.transform, enemy.unitGraphics);
				}
				var pauser = contacts[i].GetComponent<IPausable>();
				if (pauser != null)
				{
					pauser.Pause(freezeDuration);
				}
			}
		}
	}

	public void BuildHourGlass(Transform parent, UnitGraphics am)
	{
		if (!hourglass)
		{
			return;
		}
		var timer = Instantiate(hourglass, parent);
		timer.gameObject.transform.position = Vector3.zero;
		timer.gameObject.transform.localPosition = Vector3.zero;
		timer.gameObject.transform.eulerAngles = Vector3.zero;

		foreach (var time in timer.GetComponentsInChildren<SpriteRenderer>())
		{
			time.sortingLayerName = am.animSortingLayer;
			int order = 0;
			foreach (var rend in am.renderers)
			{
				if (rend.sortingOrder > order)
				{
					order = rend.sortingOrder;
				}
			}
			time.sortingOrder = order + 1;
		}
		timer.StartHourglass(am, 8, Color.cyan);
	}

	public void AttachSpawn(GameObject spawn)
	{
		var hour = spawn.GetComponent<Hourglass>();
		if (hour)
		{
			hourglass = hour;
		}
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void AddGraphicsListener(UnitGraphics unitGraphics)
	{
		onLaunch.AddListener(unitGraphics.OnActivePlay);
	}

	public void AddLastActionListener(Action callback)
	{
		onLastAction.AddListener(callback.Invoke);
	}
}
