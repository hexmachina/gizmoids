using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundWave : MonoBehaviour
{

	public bool firing = false;
	public bool isActive;
	public int power;
	public float firingRate;
	public float delay;
	public GizmoidView tracker;
	//public CheckArena checkArena;
	public ParticleSystem particle;
	//public List<EnemySheet> enemies;
	// Use this for initialization
	public AudioSource bagPipes;
	void Start()
	{
		particle.Stop();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		HandleFire();
	}

	public void HandleFire()
	{
		if (!firing)
		{
			//if (checkArena.CheckAisle())
			//{
			//	print("PIPES");
			//	isActive = true;
			//	firing = true;
			//	bagPipes = AudioPlayer.Instance.LoopSoundClip(tracker.assets.clipUtility1, transform);
			//	var enemies = FindEnemies();
			//	tracker.animationManager.anim.SetBool("Active", isActive);
			//	StopAllCoroutines();
			//	StartCoroutine(Fire(enemies));
			//}
			//else
			//{
			//	StopBagpipes();

			//	particle.Stop();
			//	isActive = false;
			//	tracker.animationManager.anim.SetBool("Active", isActive);

			//}
		}


	}

	void StopBagpipes()
	{
		if (bagPipes != null)
		{
			AudioPlayer.Instance.StopLoopingClip(bagPipes);
			bagPipes = null;
		}
	}

	public List<EnemyView> FindEnemies()
	{
		List<EnemyView> enemies = new List<EnemyView>();
		//foreach (var item in checkArena.GetListeningRows())
		//{
		//	foreach (var en in item.FindOccupiers("Enemy Point"))
		//	{
		//		//print(this.name + " scrap " + scrap.tag);
		//		var enemy = en.GetComponentInParent<EnemyView>();
		//		if (enemy)
		//		{
		//			enemies.Add(enemy);
		//		}
		//	}
		//}
		return enemies;
	}

	public IEnumerator Fire(List<EnemyView> enemies)
	{
		particle.Play();
		tracker.unitGraphics.anim.Play("Bagpipes_Active_01", 0, 0);
		yield return new WaitForSeconds(delay);
		List<EnemyView> enems = new List<EnemyView>(enemies);
		foreach (var en in enems)
		{
			if (en)
			{
				en.health.TakeDamage(power);
			}
			else
			{
				enemies.Remove(en);
			}
		}
		//foreach (var row in checkArena.GetListeningRows())
		//{
		//	row.StartFadeGraphics(0.25f, 2f);
		//}
		firing = false;
		isActive = false;
		particle.Stop();
		yield return null;
	}
}
