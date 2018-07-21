using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic/SectorDataList")]
public class SectorDataList : ScriptableObject
{
	public List<GizLib.SectorData> sectorData = new List<GizLib.SectorData>();

}
