using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class AttractMover : ColliderAbility, IAbility, IListenable
{
	public class AttractEvent : UnityEvent<bool> { }

	[UnityEngine.Serialization.FormerlySerializedAs("attractionSpeed")]
	public float attractSpeed;


	protected override void TargetEntered(Collider2D collision)
	{
		var mover = collision.GetComponent<Mover>();
		if (mover)
		{
			AudioPlayer.Instance.PlaySoundClip(assetData.clipUtility1, transform);
			mover.StartMovement(MoveType.Homing, attractSpeed, transform);

			base.TargetEntered(collision);
		}
	}

	protected override void TargetExited(Collider2D collision)
	{
		var mover = collision.GetComponent<Mover>();
		if (mover)
		{
			mover.RestoreOriginalPath();
		}
	}

	public void SetFields(Dictionary<string, float> valuePairs)
	{
		if (valuePairs.ContainsKey("AttractSpeed"))
		{
			attractSpeed = (int)valuePairs["AttractSpeed"];
		}
	}

	public void SetAudioAsset(AudioAssetData audioAsset)
	{
		assetData = audioAsset;
	}

	public void AddGraphicsListener(UnitGraphics unitGraphics)
	{
		onTargetSighted.AddListener(unitGraphics.OnActivateBool);
	}
}

