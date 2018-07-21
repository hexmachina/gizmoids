using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;

public class CollectibleView : MonoBehaviour
{
	[Serializable]
	public class CollectEvent : UnityEvent<CollectibleData>
	{ }

	public CollectibleData collectibleData;

	public bool dropped;

	public bool phasesOut;

	public int phaseCount;

	public Mover mover;

	public CollectEvent onCollected = new CollectEvent();

	protected Sequence sequence;

	public virtual void Collect()
	{
		if (collectibleData)
		{
			onCollected.Invoke(collectibleData);
		}
	}

	protected virtual void TargetReached()
	{

	}

	public virtual void SetCollectibleData(CollectibleData data)
	{
		collectibleData = data;
		SetMovingData(data.moverData);
		transform.localScale = data.scale;
		phasesOut = collectibleData.phasing;
		if (collectibleData.animatorController)
		{
			SetAnimator(collectibleData.animatorController);
		}
	}

	public void SetMovingData(MoverData data)
	{
		if (data)
		{
			if (!mover)
			{
				mover = GetComponent<Mover>();
				if (!mover)
				{
					mover = gameObject.AddComponent<Mover>();
				}
			}
			mover.useRadius = true;
			mover.speed = data.speed;
			mover.startRadius = data.startRadius;
			mover.endRadius = data.endRadius;
			mover.randomized = data.randomRadian;

		}
		else
		{
			if (mover)
			{
				mover.moving = false;
			}
			else
			{
				mover = GetComponent<Mover>();
				if (mover)
				{
					mover.moving = false;
				}
			}
		}
	}

	public void SetAnimator(RuntimeAnimatorController animatorController)
	{
		var animator = GetComponent<Animator>();
		if (animator)
		{
			animator.runtimeAnimatorController = animatorController;

		}
	}

	public virtual void Mobilize(int dir)
	{
		if (!mover)
		{
			mover = GetComponent<Mover>();
			if (!mover)
			{
				mover = gameObject.AddComponent<Mover>();
			}
		}
	}

	private IEnumerator DelayPhase(float delay, Action callback)
	{
		yield return new WaitForSeconds(delay);
		callback.Invoke();
	}

	protected virtual void Phase(int phases, Action callback, float delay)
	{
		if (delay > 0)
		{
			StartCoroutine(DelayPhase(delay, () => Phase(phases, callback)));
		}
		else
		{
			Phase(phases, callback);
		}
	}

	protected virtual void Phase(int phases, Action callback)
	{
		var sp = GetComponent<SpriteRenderer>();
		if (sp)
		{
			sequence = DOTween.Sequence();
			sequence.SetLoops(phases, LoopType.Yoyo)
				.Append(sp.DOFade(0f, 0.4f))
				.OnComplete(callback.Invoke);
		}
	}
}
