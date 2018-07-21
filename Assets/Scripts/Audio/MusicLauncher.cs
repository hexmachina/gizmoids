using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLauncher : MonoBehaviour
{
	public AudioClipListSet collection;

	private void Start()
	{
		if (collection)
		{
			var clipList = collection.GetRandomClipList();
			AudioPlayer.Instance.PlayMusic(clipList);
		}

	}
}
