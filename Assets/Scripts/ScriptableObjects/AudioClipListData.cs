using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic/AudioClipList")]
public class AudioClipListData : ScriptableObject
{
	public string nameFull;
	public List<AudioClip> data = new List<AudioClip>();

}
