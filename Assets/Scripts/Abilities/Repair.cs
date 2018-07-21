using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class Repair : Ability, IAbility, ISpawnable, ILastActionable
{
	public bool healing;
	public float duration;
	public Healer projectile;
	public Color healingColor;

	private Transform mainTransform;

	[SerializeField]
	private EntityView receiver;

	public GameObject finalBlast;

	public UnityEvent onLastAction = new UnityEvent();

	public void StartHealing()
	{
		var view = GetView();
		if (view)
		{
			mainTransform = view.transform;
		}

		receiver = GetReceiver();
		if (receiver)
		{
			healing = true;
			StartCoroutine(Healing(duration));
			AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);
		}
	}

	public EntityView GetView()
	{
		var ent = GetComponent<EntityView>();
		if (!ent)
		{
			ent = GetComponentInParent<EntityView>();
		}
		return ent;
	}

	public EntityView GetReceiver()
	{
		var index = mainTransform.GetSiblingIndex();
		if (index > 0)
		{
			var child = mainTransform.parent.GetChild(index - 1);
			return child.GetComponent<EntityView>();
		}

		return null;
	}

	public IEnumerator Healing(float seconds)
	{
		float progress = 0;
		int healAmount = receiver.health.maxHitpoints - receiver.health.hitPoints;
		int lastHealth = receiver.health.hitPoints;
		int increment = (int)(healAmount / (seconds * 2));

		var heal = Instantiate(projectile, receiver.transform);
		heal.name = "Heal Projectile";
		heal.transform.localPosition = Vector3.zero;
		heal.transform.localEulerAngles = new Vector3(-90, 90, 0);
		heal.EnableParticles(true, true, true);
		receiver.unitGraphics.StartColorPhase(0.5f, healingColor);

		while (progress < seconds)
		{
			if (!receiver)
			{
				receiver = GetReceiver();
			}
			if (receiver)
			{
				receiver.health.Heal(increment);
				yield return new WaitForSeconds(0.5f);
				progress += 0.5f;
			}
		}

		healing = false;

		Destroy(heal.gameObject);
		if (receiver != null)
		{
			receiver.unitGraphics.EndColorPhase();
			receiver.health.RestoreHealth();
			receiver.reskinAnimation.Restore();
		}
		var blast = Instantiate(finalBlast);
		blast.transform.eulerAngles = new Vector3(-90, 90, transform.eulerAngles.z);
		blast.transform.position = transform.position;
		onLastAction.Invoke();
	}

	private void OnDestroy()
	{
		if (healing && receiver)
		{
			receiver.unitGraphics.EndColorPhase();
		}
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("Duration"))
		{
			duration = valuePairs["Duration"];
		}

		StartHealing();
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void AttachSpawn(GameObject spawn)
	{

		finalBlast = spawn;
	}

	public void AddLastActionListener(Action callback)
	{
		onLastAction.AddListener(callback.Invoke);
	}
}
