using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic/AudioClip List Set")]
public class AudioClipListSet : ScriptableObject
{
	public List<AudioClipListData> data = new List<AudioClipListData>();

	public AudioClipListData GetRandomClipList()
	{
		if (data.Count > 0)
		{
			return data[Random.Range(0, data.Count)];
		}

		return null;
	}
}
