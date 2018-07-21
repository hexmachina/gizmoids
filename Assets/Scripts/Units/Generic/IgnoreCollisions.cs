using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IgnoreCollisions : MonoBehaviour
{
	[UnityEngine.Serialization.FormerlySerializedAs("blackList")]
	public List<string> ignoredTags;

	// Use this for initialization
	void Start()
	{
		IgnoreBlackListCollisions();
	}

	public void IgnoreBlackListCollisions()
	{
		foreach (var item in ignoredTags)
		{
			IgnoreCollision(item);
		}
	}

	public void IgnoreCollision(string tag)
	{
		var objects = GameObject.FindGameObjectsWithTag(tag);

		foreach (var o in objects)
		{

			if (o.GetComponent("Collider2D") && o != gameObject)
			{
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), o.GetComponent<Collider2D>());
			}

		}
	}
}
