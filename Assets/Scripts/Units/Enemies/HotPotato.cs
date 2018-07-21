using UnityEngine;
using System.Collections;

public class HotPotato : MonoBehaviour
{


	public float duration;
	private float countdown;
	public EnemyView enemySheet;

	// Use this for initialization
	void Start()
	{
		duration = countdown;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		switch (coll.gameObject.tag)
		{
			case "Projectile Player":
				//print("projectectile!");
				coll.gameObject.BroadcastMessage("Impact", enemySheet.health);
				//coll.bounds.center
				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				//FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));

				break;
			case "Blade":
				coll.gameObject.BroadcastMessage("Impact", enemySheet, SendMessageOptions.DontRequireReceiver);
				//Ricochet();
				FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));
				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				//coll.gameObject.SetActive(false);
				//SelfDestruct();
				break;
			case "Gizmoid":
				//coll.gameObject.BroadcastMessage("TakeDamage", enemySheet.enemyData.power);
				FXManager.instance.AddExplosion(new Vector3(coll.bounds.center.x, coll.bounds.center.y));

				//FXManager.instance.AddExplosion(new Vector3(coll.contacts[0].point.x, coll.contacts[0].point.y));
				//Ricochet();
				//SelfDestruct();
				break;
			case "Player Ship":
				coll.gameObject.BroadcastMessage("Impact");
				break;
		}

	}

	public void Countdown()
	{
		if (countdown < 0)
		{
			enemySheet.SelfDestruct();
		}
		else
		{
			countdown -= Time.deltaTime;
		}

	}
}
