using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Enemy")]
public class EnemyData : EntityData
{
	public float speed;

	[UnityEngine.Serialization.FormerlySerializedAs("dropAmount")]
	public int dropScrapAmount;

	public float frequency;

	public bool canReskin = true;

	public Vector2 colliderSize = new Vector2(0.6f, 0.6f);

	public CapsuleDirection2D capsuleDirection;

	public DropData dropData;

	//public float rareChance;
	//public int rareTier;
	//public EnemyUtility utility;

	//public List<GizLib.Utility> utilities = new List<GizLib.Utility>();

}
