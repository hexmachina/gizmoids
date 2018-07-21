using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
	public string gameplaySceneName = "Gameplay";

	public string mainMenuSceneName = "MainMenu";

	public LevelDataSet levelSet;

	public AlertMessage alertPrompt;

	public ScrollRect scrollRect;

	public List<LevelPlatformButton> platformButtons = new List<LevelPlatformButton>();

	public RectTransform content;

	public AudioClip clipClick;

	public AudioClip clipLock;

	public Color completed;

	public Color locked;

	public Color current;

	// Use this for initialization
	void Start()
	{
		var activeProfile = LocalDatabase.Instance.activeProfile;

		PopulateMap(activeProfile);
	}

	public void PopulateMap(GizLib.Profile profile)
	{
		float anchor = 0.2f;
		bool up = true;

		float scrollX = 0;

		var platformPicks = new List<LevelPlatformButton>(platformButtons);
		for (int i = 0; i < levelSet.dataSet.Count; i++)
		{
			if (platformPicks.Count == 0)
			{
				platformPicks = new List<LevelPlatformButton>(platformButtons);
			}

			var beaconColor = locked;
			var clip = clipLock;
			var index = i;
			System.Action action = () => alertPrompt.ShowMessage("Mission " + levelSet.dataSet[index].localId + ": Locked", "Sorry, Mission " + levelSet.dataSet[index].localId + " is currently locked. Come back later.");

			bool canInteract = false;

			if (profile.completedLevels.Exists(x => x.localId == levelSet.dataSet[i].localId && x.worldId == levelSet.dataSet[i].worldId))
			{
				canInteract = true;
				beaconColor = completed;
			}
			else if (i == 0 || (i - 1 > 0 && profile.completedLevels.Exists(x => x.localId == levelSet.dataSet[i - 1].localId && x.worldId == levelSet.dataSet[i - 1].worldId)))
			{
				canInteract = true;

				beaconColor = current;
				scrollX = ((i * 1f) + 1f) / (levelSet.dataSet.Count * 1f);
				if (scrollX > 0.25f)
				{
					if (scrollX > 1f)
					{
						scrollX = 1f;
					}
				}
				else
				{
					scrollX = 0;
				}
			}

			if (canInteract)
			{
				action = () => alertPrompt.ShowMessage("Launch Mission " + levelSet.dataSet[index].localId, levelSet.dataSet[index].description, () => GoToGameplay(levelSet.dataSet[index]));
				clip = clipClick;
			}

			var randIndex = Random.Range(0, platformPicks.Count);
			var button = BuildPlatform(platformPicks[randIndex], anchor, beaconColor, clip, action);

			platformPicks.RemoveAt(randIndex);
			if (anchor > 0.7f || anchor < 0.2f)
			{
				up = !up;
			}
			if (up)
			{
				anchor += 0.3f;
			}
			else
			{
				anchor -= 0.3f;
			}
		}
		scrollRect.horizontalNormalizedPosition = scrollX;
	}

	public void GoToGameplay(LevelData level)
	{
		SessionManager.Instance.levelData = level;

		SceneManager.LoadScene(gameplaySceneName);
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene(mainMenuSceneName);

	}

	public LevelPlatformButton BuildPlatform(LevelPlatformButton prefab, float anchor, Color beaconColor, AudioClip clip, System.Action callback)
	{
		var go = new GameObject("PlatformContainer", typeof(RectTransform));
		var rt = go.GetComponent<RectTransform>();
		rt.SetParent(content);
		rt.localScale = Vector3.one;
		rt.sizeDelta = new Vector2(360, 100);
		var lp = Instantiate(prefab, rt);
		lp.transform.localScale = Vector3.one;
		var lprt = lp.transform as RectTransform;
		lprt.sizeDelta = new Vector2(360, 360);
		lprt.anchorMin = new Vector2(0.5f, anchor);
		lprt.anchorMax = new Vector2(0.5f, anchor);
		lprt.anchoredPosition = Vector2.zero;

		lp.beacon.color = beaconColor;
		lp.button.onClick.AddListener(() => AudioPlayer.Instance.PlaySoundClip(clip));
		if (callback != null)
		{
			lp.button.onClick.AddListener(callback.Invoke);
		}

		return lp;
	}
}
