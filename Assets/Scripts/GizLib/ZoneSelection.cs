using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GizLib
{
	[System.Serializable]
	public class ZoneSelection
	{

		[Range(0, 7)]
		public int zone;

		[Range(0, 7)]
		public List<int> sections = new List<int>();
	}
}

