using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Blade")]
public class BladeData : UnitData
{
	public GizLib.BladeType type;

	public List<Sprite> spriteStates = new List<Sprite>();
}
