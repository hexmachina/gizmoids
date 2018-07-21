using UnityEngine;
using System.Collections;

public class FireLaser : MonoBehaviour
{

	private bool reloading;
	private bool firing;
	private bool fired;

	public bool laserOn;

	public float firingRate;

	//public CheckArena checkArena;
	public EnemyView enemySheet;
	public IgnoreCollisions ignoreCollisions;
	public Laser projectile;
	public int gizmoidCount;
	public int power;

	// Use this for initialization
	void Start()
	{
		//gizmoidCount = PlayerResources.Instance.ActiveGizmoids.Count;
		print("active gizmoids " + gizmoidCount);
		projectile.power = power;
		projectile.firingRate = firingRate;
	}

	// Update is called once per frame
	void Update()
	{
		//if (gizmoidCount < PlayerResources.Instance.ActiveGizmoids.Count)
		//{
		//	//print("clear collisions");
		//	gizmoidCount = PlayerResources.Instance.ActiveGizmoids.Count;
		//	ignoreCollisions.IgnoreBlackListCollisions();
		//}
		HandleFire();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		//print(coll.gameObject.tag);
		//var exp = Instantiate(explode, new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y), Quaternion.identity);
		//(exp as GameObject).transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

		switch (coll.gameObject.tag)
		{
			case "Projectile Player":
				//print("projectectile!");
				coll.gameObject.BroadcastMessage("Impact", enemySheet.health);
				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));

				break;
			case "Gizmoid":
				//Physics2D.IgnoreCollision(coll.collider, gameObject.collider2D);
				break;
			case "Enemy":
				//Physics2D.IgnoreCollision(coll.collider, gameObject.collider2D);
				break;
			case "Blade":
				coll.gameObject.BroadcastMessage("Impact", enemySheet, SendMessageOptions.DontRequireReceiver);
				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));

				//coll.gameObject.SetActive(false);
				//SelfDestruct();
				break;
			case "Player Ship":
				coll.gameObject.BroadcastMessage("Impact");
				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));

				break;
		}

	}

	void HandleFire()
	{

		//&& checkArena.CheckAisle()
		if (enemySheet.mover.type != MoveType.Detour)
		{
			if (!laserOn)
			{
				//StopAllCoroutines();
				//StartCoroutine(Fire());
				//projectile.gameObject.SetActive(true);
				//projectile.collider2D.enabled = true;
				//laserOn = true;
				EnableLaser(true);
			}
			enemySheet.mover.moving = false;

		}
		else
		{
			//projectile.gameObject.SetActive(false);
			//laserOn = false;
			EnableLaser(false);
			enemySheet.mover.moving = true;
		}
	}

	public void EnableLaser(bool active)
	{
		projectile.gameObject.SetActive(active);
		projectile.GetComponent<Collider2D>().enabled = active;
		laserOn = active;
	}
}
