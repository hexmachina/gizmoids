using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Arena Collider Grid")]
public class ArenaColliderGridData : ScriptableObject
{
	public ColliderCollection zoneColliderPrefab;

	public List<GizLib.ZoneSelection> zoneSelections = new List<GizLib.ZoneSelection>();

	public List<ColliderCollection> BuildColliders(Transform parent, int offset)
	{
		var zones = new List<ColliderCollection>();
		for (int i = 0; i < zoneSelections.Count; i++)
		{
			var zone = Instantiate(zoneColliderPrefab, parent);
			zone.transform.position = Vector3.zero;
			zone.transform.localEulerAngles = new Vector3(0, 0, -45.05f * zoneSelections[i].zone);

			var coll = new List<Collider2D>(zone.colliders);
			for (int j = 0; j < coll.Count; j++)
			{
				if (j < offset || !zoneSelections[i].sections.Contains(j - offset))
				{
					Destroy(zone.colliders[j].gameObject);
				}
				else
				{
					zone.colliders[j].usedByComposite = true;
				}
			}
			zones.Add(zone);
		}
		return zones;
	}
}
