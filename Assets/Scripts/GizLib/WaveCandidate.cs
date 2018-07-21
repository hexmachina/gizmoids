using System;
using System.Collections.Generic;

namespace GizLib
{
	[Serializable]
	public class WaveCandidate
	{
		public int spawnLane = -1;

		public List<EnemyData> possibleEnemies = new List<EnemyData>();

		public EnemyData PickRandomCandidate()
		{
			if (possibleEnemies.Count == 1)
			{
				return possibleEnemies[0];
			}

			var enemyFrequencies = new Dictionary<float, EnemyData>();
			float sumFrequency = 0;
			foreach (var item in possibleEnemies)
			{
				enemyFrequencies.Add(item.frequency + sumFrequency, item);
				sumFrequency += item.frequency;
			}

			float randomNumber = UnityEngine.Random.value * sumFrequency;

			foreach (var item in enemyFrequencies)
			{
				if (randomNumber <= item.Key)
				{
					return item.Value;
				}
			}
			return null;
		}
	}
}
