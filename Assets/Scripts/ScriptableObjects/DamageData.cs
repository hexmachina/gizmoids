using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Damage")]
public class DamageData : ScriptableObject
{
	public float hitDuration;
	public Material materialDefault;
	public Material materialHit;
}
