using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : UnitData
{
	public string nameInternal;
	public string description;
	public int maxHealth;
	[UnityEngine.Serialization.FormerlySerializedAs("animationManager")]
	public UnitGraphics unitGraphics;
	public AudioAssetData assetData;
	public List<AbilityData> abilities = new List<AbilityData>();
}
