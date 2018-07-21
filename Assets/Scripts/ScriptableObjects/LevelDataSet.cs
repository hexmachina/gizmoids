using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelDataSet : ScriptableObject
{
	[UnityEngine.Serialization.FormerlySerializedAs("levelDatas")]
	public List<LevelData> dataSet = new List<LevelData>();
}
