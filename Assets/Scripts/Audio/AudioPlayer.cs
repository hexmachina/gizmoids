using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MusicCue
{
	None = -1,
	GameplayWorld1 = 0,
	Title = 1,
	Map = 2
}

[System.Serializable]
public class MusicClipPair
{
	public AudioClip clipWithIntro;
	public AudioClip clipNoIntro;
}

public class AudioPlayer : MonoBehaviour
{

	private static AudioPlayer _instance;
	public static AudioPlayer Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<AudioPlayer>();
				if (_instance == null)
				{
					var go = new GameObject("Audio Player");
					_instance = go.AddComponent<AudioPlayer>();
				}
			}
			return _instance;
		}
	}

	public int clipIndex;

	public GameObject audioSourcePrefab;

	public AudioSource musicSource;

	public AudioClip nextSound;
	public AudioClip backSound;
	public AudioClip saveSound;
	public AudioClip playSound;

	//music
	public AudioClip titleClip;
	public AudioClip mapClip;
	public List<MusicClipPair> world1Clips;

	//public AudioClip titleNoIntro;

	//public Dictionary<ButtonSound, AudioClip> soundLookup;

	public List<AudioSource> sfxSources = new List<AudioSource>();

	public List<AudioSource> loopingClips = new List<AudioSource>();

	//public AudioClip utilityClip;
	//public AudioClip destroyedClip;
	//private float _volume = 1f;
	//	public float Volume
	//	{
	//		get
	//		{
	//			return _volume;
	//		}
	//		set
	//		{
	//			_volume = value;
	//				Debug.Log ("Volume set to "+value);
	//		}
	//	}

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			if (!musicSource)
			{
				musicSource = gameObject.AddComponent<AudioSource>();
			}

		}
		else if (this != _instance)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		musicSource.volume = LocalDatabase.Instance.userPreferences.musicSetting;
		//audioSource = GetComponent<AudioSource>();
		//soundLookup = new Dictionary<ButtonSound, AudioClip>()
		//{
		//	{ButtonSound.NextSound, nextSound},
		//	{ButtonSound.ErrorSound, backSound},
		//	{ButtonSound.SaveSound, saveSound},
		//	{ButtonSound.PlaySound, playSound}

		//};
	}

	public void SetSFXSourcesVolume(float value)
	{
		foreach (var item in sfxSources)
		{
			item.volume = value;
		}
	}

	public AudioSource BuildSfxSource()
	{
		var go = new GameObject("SFX Source");
		go.transform.SetParent(transform);
		var aud = go.AddComponent<AudioSource>();
		aud.volume = LocalDatabase.Instance.userPreferences.soundSetting;

		sfxSources.Add(aud);
		return aud;
	}

	public AudioSource PlaySoundClip(AudioClip audioClip)
	{
		if (audioClip)
		{
			AudioSource source = null;
			foreach (var item in sfxSources)
			{
				if (!item.isPlaying)
				{
					source = item;
					break;
				}
			}
			if (!source)
			{
				source = BuildSfxSource();
			}
			source.clip = audioClip;
			source.Play();
			return source;
		}
		return null;
	}

	public AudioSource PlaySoundClip(AudioClip audioClip, Transform soundTransform)
	{
		return PlaySoundClip(audioClip);
	}

	//public AudioSource PlaySoundEnum(ButtonSound buttonSound, Transform objectTransform)
	//{
	//	if (buttonSound != ButtonSound.None)
	//	{
	//		return PlaySoundClip(soundLookup[buttonSound]);
	//		//return PlayClipAtPoint(soundLookup[buttonSound], objectTransform);
	//	}
	//	else
	//	{
	//		Debug.Log("No button sound assigned.");
	//		return null;
	//	}
	//}

	public AudioSource LoopSoundClip(AudioClip audioClip, Transform soundTransform)
	{
		if (audioClip != null)
		{
			Debug.Log("Playing " + audioClip);
			return LoopClipAtPoint(audioClip, soundTransform);
		}
		else
		{
			Debug.Log("Audio clip not found", soundTransform.gameObject);
			return null;
		}
	}

	public void PlayBGM(MusicCue musicCue)
	{
		switch (musicCue)
		{
			case MusicCue.Title:
				PlayTrackLooping(titleClip);
				break;
			case MusicCue.Map:
				PlayTrackLooping(mapClip);
				break;
			case MusicCue.GameplayWorld1:
				MusicClipPair musicClipPair = world1Clips[Mathf.FloorToInt(Random.value * world1Clips.Count)];
				PlayTracksLooping(musicClipPair);
				break;
			default:
				Debug.LogError("No handling for music cue: " + musicCue);
				break;
		}
	}

	void PlayTrackLooping(AudioClip musicClip)
	{
		StartMusic(musicClip, true);
	}

	void PlayTracksLooping(MusicClipPair pair)
	{
		StartMusic(pair.clipWithIntro, false);
		StartCoroutine(LoopOnEnd(pair.clipNoIntro));
	}

	IEnumerator LoopOnEnd(AudioClip clipSansIntro)
	{
		//float startTime = Time.realtimeSinceStartup;
		while (musicSource.time < musicSource.clip.length)
		{
			yield return 0;
		}
		StartMusic(clipSansIntro, true);

	}

	void StartMusic(AudioClip clip, bool isLooping)
	{
		musicSource.clip = clip;
		musicSource.loop = isLooping;
		//musicSource.volume = LocalDatabase.Instance.userPreferences.musicSetting;
		musicSource.Play();
	}

	AudioSource PlayClipAtPoint(AudioClip audioClip, Transform soundTransform)
	{

		AudioSource tempAudio = CreateAudioSource(audioClip, soundTransform);
		StartCoroutine(AutoDestroySound(tempAudio));
		return tempAudio;
	}

	AudioSource LoopClipAtPoint(AudioClip audioClip, Transform soundTransform)
	{

		AudioSource tempAudio = FindLoopingClip(audioClip, soundTransform);
		if (tempAudio == null)
		{
			tempAudio = CreateAudioSource(audioClip, soundTransform, true);
			tempAudio.loop = true;
			loopingClips.Add(tempAudio);
		}

		//StartCoroutine(AutoDestroySound(tempAudio));
		return tempAudio;
	}

	AudioSource CreateAudioSource(AudioClip audioClip, Transform soundTransform, bool attach = false)
	{
		GameObject tempAudioGo = Instantiate(audioSourcePrefab, soundTransform.position, Quaternion.identity) as GameObject;

		if (attach)
		{
			tempAudioGo.transform.parent = soundTransform;
			tempAudioGo.transform.position = Vector3.zero;
		}

		AudioSource tempAudio = tempAudioGo.GetComponent<AudioSource>();
		Debug.Log(tempAudio);
		tempAudio.clip = audioClip;
		tempAudio.volume = LocalDatabase.Instance.userPreferences.soundSetting;
		tempAudio.Play();
		return tempAudio;
	}

	IEnumerator AutoDestroySound(AudioSource audioSource)
	{
		//Debug.Log (audioSource.clip.length);
		float startTime = Time.realtimeSinceStartup;
		float clipLength = audioSource.clip.length;
		while (Time.realtimeSinceStartup - startTime < clipLength)
		{
			//Debug.Log("Looping. Delta time, length: "+(Time.time - startTime) + ", "+ audioSource.clip.length);
			yield return 0;
		}
		//Debug.Log ("Loop completed, destroying: " + audioSource.gameObject.name);
		if (audioSource)
		{
			Destroy(audioSource.gameObject);
		}
	}

	public AudioSource FindLoopingClip(AudioClip audioClip, Transform soundTf)
	{
		AudioSource source = null;
		if (loopingClips.Contains(null))
		{
			var templist = new List<AudioSource>(loopingClips);
			foreach (var item in templist)
			{
				if (item == null)
				{
					loopingClips.Remove(item);
				}
			}
		}
		if (loopingClips.Exists(x => x.clip == audioClip))
		{
			var clips = loopingClips.FindAll(x => x.clip == audioClip);
			foreach (var item in clips)
			{
				if (item.gameObject.transform.parent == soundTf)
				{
					source = item;
					break;
				}
			}
			return source;
		}
		else
		{
			return null;
		}

	}

	public void PauseLoopingClips()
	{
		foreach (var item in loopingClips)
		{
			item.Pause();
		}
	}

	public void PlayLoopingClips()
	{
		foreach (var item in loopingClips)
		{
			item.Play();
		}
	}

	public void StopLoopingClip(AudioSource audioSource)
	{
		if (loopingClips.Contains(audioSource))
		{
			var source = loopingClips.Find(x => x == audioSource);
			source.Stop();

			loopingClips.Remove(source);
			Destroy(source.gameObject);
		}


	}

	//public IEnumerator CompleteSound(ButtonSound buttonSound, Transform transform)
	//{
	//	AudioSource tempAudio = CreateAudioSource(soundLookup[buttonSound], transform);
	//	yield return StartCoroutine(AutoDestroySound(tempAudio));
	//}

	public void PlayMusic(AudioClipListData clipData)
	{
		if (clipData)
		{
			clipIndex = 0;
			PlayClipList(clipData);
		}
	}

	private void PlayClipList(AudioClipListData clipData)
	{
		if (clipData.data.Count > 0)
		{
			var multi = clipData.data.Count > 1;
			musicSource.clip = clipData.data[clipIndex];
			musicSource.loop = !multi;
			musicSource.Play();
			if (multi)
			{
				StartCoroutine(OnAudioSourceEnd(musicSource, () => IncrementClipData(clipData)));
			}
		}
	}

	private IEnumerator OnAudioSourceEnd(AudioSource audioSource, System.Action callback)
	{
		while (audioSource.time < audioSource.clip.length)
		{
			yield return null;
		}
		callback.Invoke();
	}

	private void IncrementClipData(AudioClipListData clipData)
	{
		if (clipIndex < clipData.data.Count - 1)
		{
			clipIndex++;
		}
		PlayClipList(clipData);
	}
}
