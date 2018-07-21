using UnityEngine;
using System.Collections;

public class ProjectileParticle : Spawn
{

	public string sortingLayerName;
	public int sortingOrder;
	public bool hasRotation;

	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start()
	{
		if (!particleSystem)
		{
			particleSystem = GetComponent<ParticleSystem>();
		}

		if (particleSystem)
		{
			var main = particleSystem.main;
			//main
		}

		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;
		if (!hasRotation)
		{
			SetParticleRotation();
		}
	}

	public void SetParticleRotation()
	{
		var rot = gameObject.transform.parent.eulerAngles.z;
		var ps = GetComponent<ParticleSystem>();
		if (ps)
		{
			var main = ps.main;
			main.startRotation = (360 - rot) * (Mathf.PI / 180);
		}
		//print(particleSystem.startRotation);
	}

	private void OnEnable()
	{
		if (!hasRotation)
		{
			SetParticleRotation();
		}
	}
}

