using System;
using System.Collections.Generic;

namespace GizLib
{
	public enum ConditionType
	{
		None = 0,
		RotateShip = 1,
		PickUpCollectible = 2,
		PlaceGizmoid = 3,
		DestroyEnemy = 4,
		StockpileScrap = 5,
		WaitForSeconds = 6,
		UseButton = 7,
		DestroyGizmoid = 8
	}

	[Serializable]
	public class Condition
	{
		public ConditionType conditionType;
		public bool requiredForProgression = true;
		public string promptText;
		public string argument;

		public List<StringKeyValue> arguments = new List<StringKeyValue>();

		public Dictionary<string, string> GetAuguementsDictionary()
		{
			var dict = new Dictionary<string, string>();

			foreach (var item in arguments)
			{
				if (!dict.ContainsKey(item.key))
				{
					dict.Add(item.key, item.value);
				}
			}

			return dict;
		}
	}
}
