using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeTime : MonoBehaviour
{

	public bool firing = false;
	public bool isActive;

	public float freezeDuration;
	public float delay;

	public GizmoidView tracker;
	//public CheckArena checkArena;

	public Hourglass hourglass;

	public bool overclocked;

	public GameObject finalBlast;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		HandleFreeze();
	}

	void FixedUpdate()
	{

	}

	public void HandleFreeze()
	{
		if (!firing)
		{
			//if (checkArena.CheckAisle())
			//{
			//	firing = true;
			//	StartCoroutine(FreezingAction());

			//}
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

	//public IEnumerator FreezingAction()
	//{
	//	tracker.unitGraphics.anim.Play("TimeMonkey_Ready_01", 0, 0);
	//	yield return new WaitForSeconds(delay);
	//	AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility1, transform);

	//	yield return null;
	//}

	public void FreezeEnemy(EnemyView en, float freezeTime)
	{
		en.Pause(freezeTime);
		var timer = Instantiate(hourglass) as Hourglass;
		//timer.gameObject.transform.parent = en.graphics.transform;
		timer.gameObject.transform.position = Vector3.zero;
		timer.gameObject.transform.localPosition = Vector3.zero;

		foreach (var time in timer.GetComponentsInChildren<SpriteRenderer>())
		{
			time.sortingLayerName = en.unitGraphics.animSortingLayer;
			int order = 0;
			foreach (var rend in en.unitGraphics.renderers)
			{
				if (rend.sortingOrder > order)
				{
					order = rend.sortingOrder;
				}
			}
			time.sortingOrder = order + 1;


		}
		timer.StartHourglass(en.unitGraphics, 8, Color.cyan);
	}

	public void BeginOverclocking()
	{
		overclocked = true;
		//StartCoroutine(FreezeAll());
	}

	//public IEnumerator FreezeAll()
	//{
	//	var enemies = EnemySpawner.Instance.transform.GetComponentsInChildren<EnemyView>();
	//	if (enemies.Length > 0)
	//	{
	//		yield return null;
	//	}
	//	tracker.unitGraphics.anim.Play("TimeMonkey_Ready_01", 0, 0);
	//	yield return new WaitForSeconds(2);
	//	foreach (var enemy in enemies)
	//	{
	//		FreezeEnemy(enemy, freezeDuration);
	//	}
	//	overclocked = false;
	//	var blast = Instantiate(finalBlast) as GameObject;
	//	//blast.transform.eulerAngles = new Vector3(-90, 90, tracker.placement.currentAisle.transform.eulerAngles.z);
	//	blast.transform.position = transform.position;
	//	blast.GetComponent<Renderer>().sortingLayerName = "Projectiles";
	//	tracker.SelfDestruct(false);
	//}
}
