
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class BladeView : MonoBehaviour, IImpactable
{
	[System.Serializable]
	public class ImpactEvent : UnityEvent<BladeView, GameObject> { }

	public float radiusOffset = 0.9f;

	public BladeData bladeData;
	[UnityEngine.Serialization.FormerlySerializedAs("noidHolder")]
	public GizmoidHolder gizmoidHolder;

	public SpriteRenderer spriteRenderer;

	[UnityEngine.Serialization.FormerlySerializedAs("audioClip")]
	public AudioClip clipDestroy;

	public AudioClip clipPlace;

	public ImpactEvent onImpact = new ImpactEvent();

	public void Impact(GameObject gameObject)
	{

		if (gizmoidHolder.gizmoids.Count == 0)
		{
			AudioPlayer.Instance.PlaySoundClip(clipDestroy, transform);
			onImpact.Invoke(this, gameObject);

		}

	}
}
