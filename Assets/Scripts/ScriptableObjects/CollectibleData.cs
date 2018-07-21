using UnityEngine;
using GizLib;

[CreateAssetMenu(menuName = "Data/Collectible")]
public class CollectibleData : ScriptableObject
{
	public CollectibleType collectibleType;
	public string collectibleSubtype;
	public int value;
	//public int tier = -1;
	public float dropRarity;
	public bool phasing;
	public int phaseCount;
	public float phaseDelay;
	public Vector3 scale = Vector3.one;
	[UnityEngine.Serialization.FormerlySerializedAs("sprite")]
	public Sprite icon;
	public RuntimeAnimatorController animatorController;
	public CollectibleButtonIcon buttonIcon;
	public AudioClip pickupClip;
	public MoverData moverData;
}
