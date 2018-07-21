using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GizLib
{
	[System.Serializable]
	public class Profile
	{
		public string name = "";
		public int gizmoidSlots = 5;
		public int xenos = 0;
		public int spaceCrystals = 0;
		public List<GameProgression> gameProgress = new List<GameProgression>();
		public List<LevelResult> completedLevels = new List<LevelResult>();
		public List<string> featuresUnlocked = new List<string>();
		public List<int> gizmoids = new List<int>();
		//Unlocked gizmoids
		//public List<BurnCard> burnCards = new List<BurnCard>();
		//public int burnCardSlots = 10;
		public System.DateTime dateCreated = System.DateTime.Now;
		public System.DateTime lastModified;


		public Profile()
		{
			gizmoids.Add(1);
		}

		public Profile(Profile _profile)
		{
			name = _profile.name;
			gameProgress = _profile.gameProgress;
			completedLevels = new List<LevelResult>(_profile.completedLevels);
			xenos = _profile.xenos;
			spaceCrystals = _profile.spaceCrystals;
			gizmoids = new List<int>(_profile.gizmoids);
			//burnCards = new List<BurnCard>(burnCards);
			//burnCardSlots = _profile.burnCardSlots;
			gizmoidSlots = _profile.gizmoidSlots;
			featuresUnlocked = new List<string>(featuresUnlocked);
		}
	}
}
