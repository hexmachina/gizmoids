using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Projectile : Spawn
{
	[System.Serializable]
	public class ImpactEvent : UnityEvent<GameObject> { }

	public int power = 0;

	public Mover mover;

	public AudioClip impactClip;

	public List<string> targetTags = new List<string>();

	public ImpactEvent onImpact = new ImpactEvent();

	public void Orient()
	{
		StartCoroutine(Wait(SetCoarse));
	}

	protected void SetCoarse()
	{
		if (transform.eulerAngles.z % 45 == 0)
		{
			mover.SetDirection((int)(transform.eulerAngles.z / 45));
			mover.Mobilize(mover.speed);
		}

		var rb = GetComponent<Rigidbody2D>();
		if (!rb)
		{
			Debug.LogWarning("No Rigid Body Found.");
			return;
		}
		var contacts = new Collider2D[50];
		var count = rb.GetContacts(contacts);
		System.Array.Resize(ref contacts, count);

		var sectors = new List<SectorHandler>();
		for (int i = 0; i < contacts.Length; i++)
		{
			var sector = contacts[i].GetComponent<SectorHandler>();
			if (sector)
			{
				sectors.Add(sector);
			}
		}

		if (sectors.Count == 0)
		{
			return;
		}

		SectorHandler sect = sectors[0];
		for (int i = 1; i < sectors.Count; i++)
		{
			if (Mathf.Abs(sectors[i].transform.eulerAngles.z - transform.eulerAngles.z) < Mathf.Abs(sect.transform.eulerAngles.z - transform.eulerAngles.z))
			{
				sect = sectors[i];
			}
		}

		int direction = 0;
		if (sect.transform.eulerAngles.z >= 0)
		{
			direction = (int)(sect.transform.eulerAngles.z / 45f);
		}
		else
		{
			direction = (int)(360 + sect.transform.eulerAngles.z / 45f);
		}
		mover.SetDirection(direction);
		mover.Mobilize(mover.speed);
		transform.eulerAngles = sect.transform.eulerAngles;
		var radius = Vector3.Distance(transform.position, Vector3.zero) + 0.5f;
		var coarse = Mathfx.GetPointOnCircle(radius, (direction * 45) + 90, Vector3.zero);
		mover.StartMovement(MoveType.Detour, 3, coarse);

	}

	protected IEnumerator Wait(System.Action callback)
	{
		yield return new WaitForFixedUpdate();
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	protected void Impact(Health h)
	{
		if (mover.type != MoveType.Homing)
		{
		}
		AudioPlayer.Instance.PlaySoundClip(impactClip, transform);
		h.TakeDamage(power);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetTags.Contains(collision.gameObject.tag))
		{
			var health = collision.GetComponent<Health>();
			if (!health)
			{
				health = collision.GetComponentInParent<Health>();
			}
			if (health)
			{
				Impact(health);
			}

			var impact = collision.GetComponent<IImpactable>();
			if (impact != null)
			{
				impact.Impact(collision.gameObject);
			}
			onImpact.Invoke(collision.gameObject);

			gameObject.SetActive(false);
		}
	}

}
