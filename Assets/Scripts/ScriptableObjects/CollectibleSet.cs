using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Collectible Set")]
public class CollectibleSet : ScriptableObject
{

	public List<CollectibleData> dataSet = new List<CollectibleData>();
}
