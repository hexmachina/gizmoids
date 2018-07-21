using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Mover Data")]
public class MoverData : ScriptableObject
{
	public float speed;

	public bool randomRadian;

	public bool orientRotation;

	public float startRadius;

	public float endRadius;

	public bool dropInBounds;

	public float maxDropRadius;

	public float minDropRadius;

}
