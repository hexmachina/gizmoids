using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic/Sprite List")]
public class SpriteListData : ScriptableObject
{

	public List<Sprite> data = new List<Sprite>();
}
