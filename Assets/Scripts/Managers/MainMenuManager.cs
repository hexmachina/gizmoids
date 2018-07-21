using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public string worldMapSceneName;

	public string gameplaySceneName;

	public LevelDataSet levelSet;

	public Text activeProfileText;

	public AudioCue audioCueStart;

	public UnityEvent onNoActiveProfile;

	private void Start()
	{
		if (onNoActiveProfile == null)
		{
			onNoActiveProfile = new UnityEvent();
		}
		HandleActiveProfile();
	}

	public void HandleActiveProfile()
	{
		var profile = LocalDatabase.Instance.activeProfile;

		if (profile != null)
		{
			activeProfileText.text = profile.name;
		}
		else
		{
			activeProfileText.text = "Create Profile";
		}
	}

	public void OnProfileChanged(GizLib.Profile profile)
	{
		HandleActiveProfile();
	}

	public void OnPlayClick()
	{
		if (LocalDatabase.Instance.activeProfile == null)
		{
			onNoActiveProfile.Invoke();
		}
		else
		{
			if (audioCueStart)
			{
				audioCueStart.PlayAudioCue();
			}
			string pathName = "";
			if (LocalDatabase.Instance.activeProfile.featuresUnlocked.Contains("worldmap"))
			{
				pathName = worldMapSceneName;
			}
			else
			{
				if (LocalDatabase.Instance.activeProfile.completedLevels.Count == 0)
				{
					SessionManager.Instance.levelData = levelSet.dataSet[0];
				}
				else
				{
					var index = LocalDatabase.Instance.activeProfile.completedLevels.Count;
					if (index < levelSet.dataSet.Count)
					{
						SessionManager.Instance.levelData = levelSet.dataSet[index];
					}
					else
					{
						SessionManager.Instance.levelData = levelSet.dataSet[levelSet.dataSet.Count - 1];
					}
				}
				pathName = gameplaySceneName;
			}

			if (!string.IsNullOrEmpty(pathName))
			{
				SceneManager.LoadScene(pathName);
			}
		}


	}
}
