using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class ColliderAbility : Ability
{

	public class TargetEvent : UnityEvent<bool> { }

	public List<string> targetTags = new List<string>();

	public TargetEvent onTargetSighted = new TargetEvent();

	protected bool targeting;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetTags.Contains(collision.gameObject.tag))
		{
			TargetEntered(collision);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (targetTags.Contains(collision.gameObject.tag))
		{
			TargetExited(collision);
		}
	}

	protected virtual void TargetEntered(Collider2D collision)
	{
		if (!targeting)
		{
			targeting = true;
			onTargetSighted.Invoke(true);
			StartCoroutine(Targeting());
		}
	}

	protected virtual void TargetExited(Collider2D collision)
	{

	}

	protected IEnumerator Targeting()
	{
		var rb = GetComponent<Rigidbody2D>();
		if (rb)
		{
			while (targeting)
			{
				var contacts = GetContacts();
				targeting = false;
				for (int i = 0; i < contacts.Length; i++)
				{
					if (targetTags.Contains(contacts[i].gameObject.tag))
					{
						targeting = true;
						break;
					}
				}

				yield return null;
			}
		}
		onTargetSighted.Invoke(false);
	}

	protected Collider2D[] GetContacts(int startCount = 50)
	{
		var contacts = new Collider2D[startCount];
		var rb = GetComponent<Rigidbody2D>();
		if (!rb)
		{
			Debug.LogWarning("No Rigid Body Found.");
			return contacts;
		}

		var count = rb.GetContacts(contacts);
		System.Array.Resize(ref contacts, count);
		return contacts;
	}



}
