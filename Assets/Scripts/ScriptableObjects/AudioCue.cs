using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioCue : ScriptableObject
{

	public AudioClip clip;

	public void PlayAudioCue()
	{
		AudioPlayer.Instance.PlaySoundClip(clip);
	}
}
