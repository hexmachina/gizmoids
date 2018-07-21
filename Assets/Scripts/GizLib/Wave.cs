using System;
using System.Collections.Generic;

namespace GizLib
{
	[Serializable]
	public class Wave
	{
		/// <summary>
		/// If true the wave will look through the enemyRoster until the time limit is met.
		/// </summary>
		public bool timeDriven;

		public float duration = 0;

		/// <summary>
		/// The durarion in seconds between enemy spawns
		/// </summary>
		public float delay = 0;

		/// <summary>
		/// If true the Wave acti will not continue until all enemies are destroyed.
		/// </summary>
		public bool waitForEnemies = true;

		public List<WaveCandidate> waveCandidates = new List<WaveCandidate>();

		public CollectibleData dropItem;
	}
}
