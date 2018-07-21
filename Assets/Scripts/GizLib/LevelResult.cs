using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GizLib
{
	[System.Serializable]
	public class LevelResult
	{
		public int worldId;
		public int localId;
		public int highScore = 0;

		public LevelResult(int world, int local)
		{
			worldId = world;
			localId = local;
		}
	}

}
