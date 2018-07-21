using System.Collections;
using System.Collections.Generic;
using GizLib;
using UnityEngine;

public class DebrisGraphics : UnitGraphics
{

	public float rotateSpeed = 10;

	public float rotateDetourSpeed = 50;

	public int debrisType;

	public ParticleSystem particle;

	private int parts;
	private int damageIndex;

	private bool detouring;
	// Use this for initialization
	void Start()
	{

		int range = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, 0, range);
	}

	private void Update()
	{
		RotateBody();
	}

	public void SetDebrisType(string internalName)
	{
		damageIndex = 0;
		switch (internalName)
		{
			case "Debris0":
				debrisType = 0;
				parts = 3;
				break;
			case "Debris1":
				debrisType = 1;
				parts = 4;
				break;
			case "Debris2":
				debrisType = 2;
				parts = 5;
				break;
		}
		anim.Play("Debris" + debrisType + "_BD" + damageIndex + "_Idle", 0, 0);
	}

	public override void OnHealthChanged(IntValueChange change)
	{
		base.OnHealthChanged(change);

		var triggerIncrement = (change.maxValue * 1f) / (parts * 1f);
		int interval = (parts - 1) - damageIndex;
		float amount = triggerIncrement * interval;
		if (amount > change.value)
		{
			anim.Play("Debris" + debrisType + "_BD" + damageIndex, 0, 0);
			if (damageIndex < parts)
			{
				damageIndex++;
			}
		}
	}

	public void OnDetour(bool detour)
	{
		if (detouring != detour)
		{
			detouring = detour;
		}
	}

	public void RotateBody()
	{
		float rotation = 0;
		if (detouring)
		{
			rotation = Time.deltaTime * rotateDetourSpeed * -1;
		}
		else
		{
			rotation = Time.deltaTime * rotateSpeed;
		}
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotation);
	}

	public void BuildParticle(Transform transform)
	{
		var parti = Instantiate(particle);
		parti.transform.position = transform.position;
		parti.transform.eulerAngles = new Vector3(transform.eulerAngles.z - 180, 90, 0);
	}
}
