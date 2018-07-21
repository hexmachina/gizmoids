using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GizLib;

[CreateAssetMenu(menuName = "Data/Unit/Cast Member")]
public class CastMemberData : ScriptableObject
{
	public CharacterRoster characterRoster;

	public Animator animatorPrefab;

}
