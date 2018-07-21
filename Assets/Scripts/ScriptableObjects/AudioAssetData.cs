using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Audio Asset")]
public class AudioAssetData : ScriptableObject
{

	public List<AudioClip> clipArrivals;
	public AudioClip clipUtility1;
	public AudioClip clipUtility2;
	public AudioClip clipDestroy;
	public AudioClip clipDestroyed;
}
