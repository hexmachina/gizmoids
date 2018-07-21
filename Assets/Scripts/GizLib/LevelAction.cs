using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GizLib
{
	public enum ActionType
	{
		None = 0,
		SpawnBigScrap = 1, //Optional arg: Number of scrap to spawn
		AddGizmoidButton = 2, //Requires arg: gizmoid Id to create
		SpawnSmallScrap = 3,
		//StartSpawning = 4,
		//StopSpawning = 5,
		AddBlade = 6,
		//StopEnemyScrapDrop = 7,
		//StartEnemyScrapDrop = 8,
		InsertGizmoid = 9,
		Unlock = 10,
		//StartRarityDrops = 11,
		//StopRarityDrops = 12,
		AllowDropScrap = 13,
		AllowDropCollectible = 14,
		AutoSpawnScrap = 15,
		AutoSpawnCollectible = 16,
		AutoSpawnAll = 17,
		RotateShip = 18,
		GizmoidButtonInteractive = 19

		//SpawnEnemy = 3 //Required args, 1: enemy id, 2: enemy lane relative to player (0 for front, 1 for front-left...), optional third arg: quantity of enemies
	}

	[Serializable]
	public class LevelAction
	{
		public ActionType actionType = ActionType.None;
		public string argument;
	}
}
