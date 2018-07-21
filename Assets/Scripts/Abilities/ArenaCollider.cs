using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArenaCollider : MonoBehaviour
{
	public ArenaColliderGridData arenaGrid;

	private List<ColliderCollection> colliders = new List<ColliderCollection>();

	public void BuildColliders(int offset)
	{
		ResetColliders();
		colliders = arenaGrid.BuildColliders(transform, offset);
	}

	public void ResetColliders()
	{
		foreach (var item in colliders)
		{
			if (item.gameObject)
			{
				Destroy(item.gameObject);
			}
		}
		colliders.Clear();
	}
}
