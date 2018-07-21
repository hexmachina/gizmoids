using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Roster")]
public class UnitRoster : ScriptableObject
{

	public List<UnitData> unitDatas = new List<UnitData>();
}
