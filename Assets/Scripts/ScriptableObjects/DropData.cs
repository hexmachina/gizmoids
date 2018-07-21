using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Drop Data")]
public class DropData : ScriptableObject
{
	[Range(0, 1)]
	public float chance;

	public CollectibleSet collectibleSet;

	public CollectibleData GetDrop()
	{
		if (Random.value < chance)
		{
			var enemyFrequencies = new Dictionary<float, CollectibleData>();
			float sumFrequency = 0;
			foreach (var item in collectibleSet.dataSet)
			{
				enemyFrequencies.Add(item.dropRarity + sumFrequency, item);
				sumFrequency += item.dropRarity;
			}

			float randomNumber = Random.value * sumFrequency;

			foreach (var item in enemyFrequencies)
			{
				if (randomNumber <= item.Key)
				{
					return item.Value;
				}
			}
			return null;
		}
		else
		{
			return null;
		}
	}
}
