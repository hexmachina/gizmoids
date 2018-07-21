using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FireOverclockType
{
	RapidFire = 0,
	Homing = 1

}

public class GizmoidFire : MonoBehaviour
{

	public bool activated;
	public bool check;
	private bool reloading;
	private bool firing;
	private bool fired;

	public int power;
	public float firingRate;
	public float fireDelay;
	private bool startAnim;

	public GizmoidView tracker;
	public Projectile ammo;

	public bool overclocked;
	public FireOverclockType overclockType;
	//Acquired
	//public CheckArena checkArena;

	//public AudioClip fireClip;

	// Use this for initialization
	void Start()
	{
		//fire = fireInitial;
		//checkArena = GetComponent<CheckArena>();
		//StartCoroutine(Fire());
		//tracker.overclocking.onOverclockChanged.AddListener(OnOverClocking);

	}

	// Update is called once per frame
	void Update()
	{
		//HandleFire();
	}

	void FixedUpdate()
	{
		if (!overclocked)
		{
			HandleFire();
		}
	}

	void SetActivation(bool f)
	{
		activated = f;
	}

	void HandleFire()
	{
		if (fired)
		{
			if (!reloading)
			{
				StopAllCoroutines();
				StartCoroutine(Reload(firingRate));
				reloading = true;
			}
		}
		else
		{
			//if (checkArena.CheckAisle())
			//{
			//	if (!firing)
			//	{
			//		StopAllCoroutines();
			//		StartCoroutine(Fire(fireDelay, true));
			//		firing = true;
			//	}
			//}
		}
	}

	IEnumerator Fire(float anticipation, bool checkRequired)
	{
		//tracker.animationManager.anim.SetTrigger("Fire");

		tracker.unitGraphics.anim.Play("Active", 0, 0);
		yield return new WaitForSeconds(anticipation);
		AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility1, transform);
		if (checkRequired)
		{
			//if (checkArena.CheckAisle())
			//{
			//	SpawnProjectile();
			//	//AudioPlayer.Instance.PlaySound(fireClip, transform);
			//	//SendMessageUpwards("PlayUtility");
			//	fired = true;
			//}
		}
		else
		{
			//SpawnProjectile();

			//SendMessageUpwards("PlayUtility");
			fired = true;
		}

		firing = false;
		yield return null;
	}

	IEnumerator Reload(float time)
	{
		yield return new WaitForSeconds(time);
		reloading = false;
		fired = false;
		yield return null;
	}

	public void SetAnimationSpeed(float s)
	{
		tracker.unitGraphics.anim.speed = s;
	}

	//void SpawnProjectile()
	//{
	//	Debug.Log("Projectile spawned @" + Time.frameCount);
	//	Projectile p = Instantiate(ammo, transform.position, transform.rotation) as Projectile;
	//	//p.placement.aisle = tracker.placement.aisle;

	//	//Audio
	//	p.impactClip = tracker.assets.clipUtility2;

	//	//p.SetDestination();
	//	//print("projectile "+ p.GetInstanceID() + " " + p.gameObject.transform.eulerAngles.z);

	//	p.mover.SetDirection(p.placement.aisle);
	//	p.mover.Init();

	//	if (Mathf.Floor(p.gameObject.transform.eulerAngles.z) != p.placement.aisle * 45)
	//	{
	//		if (p.placement.aisle != 1)
	//		{
	//			p.gameObject.transform.eulerAngles = new Vector3(0, 0, p.placement.aisle * 45);
	//			//Vector3 center = tracker.placement.currentAisle.rows[tracker.placement.row + 2].gameObject.GetComponent<Collider2D>().bounds.center;
	//			//p.mover.RedirectPath(new Vector3(center.x,center.y), 3);
	//			//p.mover.StartMovement(MoveType.Detour, 3, new Vector3(center.x, center.y));
	//		}
	//		else
	//		{
	//			p.gameObject.transform.eulerAngles = new Vector3(0, 0, p.placement.aisle * 45);
	//			//p.gameObject.transform.position = tracker.placement.currentAisle.rows[tracker.placement.row + 1].bottom.position;
	//		}

	//	}
	//	//Set Damage
	//	p.mover.moving = true;
	//	p.power = power;
	//}

	public void BeginOverclocking()
	{
		//print("overclocking");
		overclocked = true;
		StopAllCoroutines();
		reloading = false;
		fired = false;
		firing = false;
		switch (overclockType)
		{
			case FireOverclockType.RapidFire:
				StartCoroutine(RapidFire(5, 0.2f, 0f));
				break;
			case FireOverclockType.Homing:
				//StartCoroutine(Homing(fireDelay));
				break;
		}
	}

	public void OnOverClocking(bool clock)
	{
		overclocked = clock;
		StopAllCoroutines();
		if (overclocked)
		{
			fired = false;
			firing = false;
			switch (overclockType)
			{
				case FireOverclockType.RapidFire:
					StartCoroutine(RapidFire(5, 0.2f, 0f));
					break;
				case FireOverclockType.Homing:
					//StartCoroutine(Homing(fireDelay));
					break;
			}
		}
	}


	public IEnumerator RapidFire(float duration, float preRate, float epiRate)
	{
		float totalTime = 0;
		while (totalTime < duration && overclocked)
		{
			if (fired)
			{
				if (!reloading)
				{
					StopCoroutine("Fire");
					StopCoroutine("Reload");
					StartCoroutine(Reload(epiRate));
					reloading = true;
				}
			}
			else
			{
				if (!firing)
				{
					//StopAllCoroutines();
					StopCoroutine("Fire");
					StopCoroutine("Reload");
					StartCoroutine(Fire(preRate, false));
					firing = true;
				}
			}
			totalTime += Time.deltaTime;
			yield return null;
		}

		//tracker.health.HideOverclocking();
		//overclocked = false;
		yield return null;
	}

	//public IEnumerator Homing(float anticipation)
	//{
	//	var enemies = EnemySpawner.Instance.transform.GetComponentsInChildren<EnemyView>();
	//	if (enemies.Length > 0)
	//	{
	//		yield return null;
	//	}
	//	tracker.unitGraphics.anim.Play("Active", 0, 0);
	//	yield return new WaitForSeconds(anticipation);
	//	foreach (var item in enemies)
	//	{
	//		Projectile p = Instantiate(ammo, transform.position, transform.rotation) as Projectile;
	//		var bub = p.GetComponent<Bubble>();
	//		if (bub != null)
	//		{
	//			bub.bubbleType = BubbleType.Stasis;
	//		}
	//		//p.placement.aisle = tracker.placement.aisle;
	//		p.mover.StartMovement(MoveType.Homing, p.mover.speed, item.transform);
	//		//StartCoroutine(p.mover.FollowTarget(item.transform, p.mover.speed));
	//		//p.mover.moving = true;
	//	}
	//	yield return new WaitForSeconds(2);
	//	//tracker.health.HideOverclocking();
	//	tracker.overclocking.ForceStopOverclock();
	//	yield return null;
	//}

}
