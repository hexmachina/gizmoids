using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Scrap : CollectibleView
{
	public float speed = 0.3f;
	//public bool attracted;
	//public float attractionSpeed;
	public float startRadiusJitter;
	public float startAngleRange;
	public Animator animator;

	// Use this for initialization
	void Start()
	{
		animator.Play("Scrap", 0, Random.value);
		mover.onTargetReached.AddListener(TargetReached);
	}

	public override void Collect()
	{
		base.Collect();
		gameObject.SetActive(false);
	}

	public override void Mobilize(int dir)
	{
		base.Mobilize(dir);
		//mover.SetDirection(dir);
		mover.SetAngleWithinRange(dir, startAngleRange);
		//0.4f
		mover.AddStartRaduisJitter(startRadiusJitter);
		mover.Mobilize(speed);
		mover.GoToStartPoint();
		transform.eulerAngles = new Vector3(0, 0, mover.degree - 90);
		mover.moving = true;
	}

	protected override void TargetReached()
	{
		if (phasesOut)
		{
			Phase(phaseCount, () => gameObject.SetActive(false));
		}
	}
}
