using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic/Sprite List Set")]
public class SpriteListSet : ScriptableObject
{

	public List<SpriteListData> spriteListData = new List<SpriteListData>();
}
