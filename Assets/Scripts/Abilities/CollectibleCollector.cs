using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCollector : Ability
{

	public List<string> targetTags = new List<string>();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		for (int i = 0; i < targetTags.Count; i++)
		{
			if (targetTags[i] == collision.gameObject.tag)
			{
				var collectible = collision.GetComponent<CollectibleView>();
				if (collectible)
				{
					collectible.Collect();
				}
			}
		}
	}
}
