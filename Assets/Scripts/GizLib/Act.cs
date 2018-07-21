using System;
using System.Collections.Generic;

namespace GizLib
{
	public enum ActType
	{
		None = 0,
		Wave = 1,
		Message = 2,
		Action = 3,
		Condition = 4
	}

	[Serializable]
	public class Act
	{
		public ActType actType = ActType.None;
		public Dialog dialog;
		public LevelAction action;
		public Wave wave;
		public Condition condition;
	}
}
