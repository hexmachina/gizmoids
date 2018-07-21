using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using GizLib;
using System.Runtime.InteropServices;

[System.Serializable]
public enum GameProgression
{
	None = -1,
	WorldMap = 0,
	Loadout = 1,
	Overclock = 2,
	Trash = 3,
	Xeno = 4
}

[System.Serializable]
public class UserPreferences
{
	public float soundSetting = 1;
	public float musicSetting = 1;
	public int lastProfileIndex;
}

public class LocalDatabase : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void SyncFiles();

	[DllImport("__Internal")]
	private static extern void WindowAlert(string message);


	private static LocalDatabase _instance;
	public static LocalDatabase Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<LocalDatabase>();
				if (_instance == null)
				{
					var go = new GameObject("Local Database");
					_instance = go.AddComponent<LocalDatabase>();
				}
			}
			return _instance;
		}
	}

	public Profile activeProfile;
	public List<Profile> userProfiles = new List<Profile>();
	public UserPreferences userPreferences = null;

	void Awake()
	{

		if (_instance == null)
		{
			_instance = this;

			DontDestroyOnLoad(gameObject);
			LoadPreferencesFromUserDirecory();
			LoadProfilesFromUserDirectory();

			if (userPreferences.lastProfileIndex < userProfiles.Count)
			{
				activeProfile = userProfiles[userPreferences.lastProfileIndex];
			}
		}
		else if (this != _instance)
		{

			Destroy(gameObject);
		}
	}

	public void SaveProfileToUserDirectory(Profile profile)
	{
		profile.lastModified = System.DateTime.Now;
		string profileDirecory = Application.persistentDataPath + "/Profiles/";
		string fullFileName = profileDirecory + "Profile_" + profile.name + ".dat";
		if (!Directory.Exists(profileDirecory))
		{
			Directory.CreateDirectory(profileDirecory);
		}

		SaveProfile(profile, fullFileName);
	}

	public void SaveProfile(Profile profile, string fullFileName)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(fullFileName);
		binaryFormatter.Serialize(fileStream, profile);
		fileStream.Close();
		Debug.Log(profile.name + " saved to " + fullFileName);

		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			SyncFiles();
		}
	}

	public void LoadProfilesFromUserDirectory()
	{
		Debug.Log("Attempting to load profiles");
		string profileDirectory = Application.persistentDataPath + "/Profiles/";
		if (Directory.Exists(profileDirectory))
		{
			Debug.Log("Target directory exists.");
			userProfiles.Clear();
			DirectoryInfo directoryInfo = new DirectoryInfo(profileDirectory);
			FileInfo[] files = directoryInfo.GetFiles("*.dat");
			foreach (FileInfo info in files)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(info.FullName, FileMode.Open);
				try
				{
					userProfiles.Add((Profile)binaryFormatter.Deserialize(fileStream));
					fileStream.Close();
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			Debug.Log("Local database loaded from " + profileDirectory + " @" + Time.frameCount);
		}
		else
		{
			Debug.Log("Profile directory not found at " + profileDirectory);
			Directory.CreateDirectory(profileDirectory);
		}
	}

	void ClearUserData()
	{
		if (Directory.Exists(Application.persistentDataPath + "/Profiles/"))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Profiles/");
			directoryInfo.Delete(true);
		}
	}

	public void DeleteProfile(Profile profile)
	{
		if (activeProfile == profile)
		{
			activeProfile = null;
		}
		if (userProfiles.Contains(profile))
		{
			userProfiles.Remove(profile);
			DeleteLocalProfile(profile);
		}
	}

	public Profile CreateProfile(string name)
	{
		Profile profile = new Profile();
		profile.name = name;
		userProfiles.Add(profile);

		activeProfile = profile;
		CheckInProfile(profile);
		return profile;
	}

	public void DeleteLocalProfile(Profile profile)
	{
		var profilePath = Application.persistentDataPath + "/Profiles/Profile_" + profile.name + ".dat";
		if (File.Exists(profilePath))
		{
			File.Delete(profilePath);
		}
	}

	public void CheckInProfile(Profile profile)
	{
		SetLastProfileIndex(profile);
		SaveProfileToUserDirectory(profile);
	}

	public void LoadPreferencesFromUserDirecory()
	{
		string fullFileName = Application.persistentDataPath + "/Preferences.dat";
		if (File.Exists(fullFileName))
		{
			try
			{
				userPreferences = null;
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(fullFileName, FileMode.Open);
				userPreferences = (UserPreferences)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();

			}
			catch (System.Exception ex)
			{
				PlatformSafeMessage(ex.Message);

			}
		}
		else
		{
			userPreferences = new UserPreferences();
			SavePreferencesToUserDirectory();
		}
	}

	void SavePreferencesToUserDirectory()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/Preferences.dat");
		//new FileStream (Application.persistentDataPath + "/Preferences.dat", FileMode.Create);
		binaryFormatter.Serialize(fileStream, userPreferences);
		fileStream.Close();

	}

	public void SetLastProfileIndex(Profile profile)
	{
		if (userProfiles.Contains(profile))
		{
			SetLastProfileIndex(userProfiles.IndexOf(profile));
		}
	}

	public void SetLastProfileIndex(int index)
	{
		userPreferences.lastProfileIndex = index;
		SavePreferencesToUserDirectory();
	}

	public void SetUserPreferences(float musicSetting, float soundSetting)
	{
		userPreferences.musicSetting = musicSetting;
		userPreferences.soundSetting = soundSetting;
		AudioPlayer.Instance.musicSource.volume = musicSetting;
		AudioPlayer.Instance.SetSFXSourcesVolume(soundSetting);
		SavePreferencesToUserDirectory();
	}

	public void SetVolumeMusic(float value)
	{
		userPreferences.musicSetting = value;
		AudioPlayer.Instance.musicSource.volume = value;
		SavePreferencesToUserDirectory();
	}

	public void SetVolumeSFX(float value)
	{
		userPreferences.soundSetting = value;
		AudioPlayer.Instance.SetSFXSourcesVolume(value);
		SavePreferencesToUserDirectory();
	}

	public void CleanUpProfiles()
	{
		if (userProfiles.Exists(x => x.name == string.Empty))
		{
			DeleteProfile(userProfiles.Find(x => x.name == string.Empty));
		}

		foreach (var prof in userProfiles.FindAll(x => x.dateCreated.Year == 1))
		{
			prof.dateCreated = System.DateTime.Now;
			if (prof.completedLevels.Count >= 5)
			{
				var gizs = new int[] { 2, 6, 4, 5 };
				foreach (var giz in gizs)
				{
					if (!prof.gizmoids.Contains(giz))
					{
						prof.gizmoids.Add(giz);
					}
				}
			}
			SaveProfileToUserDirectory(prof);
		}
	}

	private static void PlatformSafeMessage(string message)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			WindowAlert(message);
		}
		else
		{
			Debug.Log(message);
		}
	}
}
