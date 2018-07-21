using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GizLib;


[CreateAssetMenu]
public class LevelData : ScriptableObject
{
	public int localId = -1;
	public int worldId = -1;
	public int bronzeMinimum;
	public int silverMinimum;
	public int goldMinimum;
	public string description;
	[Range(0, 8)]
	public int initialBladeCount;
	[UnityEngine.Serialization.FormerlySerializedAs("initialParts")]
	public int initialScraps;
	public float bigScrapInterval;
	public float smallScrapInterval;
	public string levelType;
	public string mutations;
	public List<int> defaultLoadout = new List<int>();
	public List<Act> actRoster = new List<Act>();
}
