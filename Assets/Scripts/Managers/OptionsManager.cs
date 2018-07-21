using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{

	public Slider sliderMusic;

	public Slider sliderSFX;

	public GameObject buttonShelf;

	public Button buttonExit;

	public Button buttonRestart;

	public Button buttonContinue;

	public StringListData buttonShelfScenes;

	public Text textExit;

	public string exitWorld = "WorldMapNew";

	public string exitMain = "MainMenu";

	private string exitDestination;

	private void OnEnable()
	{

		var show = buttonShelfScenes.data.Contains(SceneManager.GetActiveScene().name);
		buttonExit.gameObject.SetActive(show);
		buttonRestart.gameObject.SetActive(show);

		if (LocalDatabase.Instance.activeProfile != null && LocalDatabase.Instance.activeProfile.featuresUnlocked.Contains("worldmap"))
		{
			exitDestination = exitWorld;
			textExit.text = "EXIT TO MAP";
		}
		else
		{
			exitDestination = exitMain;
			textExit.text = "EXIT TO TITLE";
		}

		sliderMusic.value = LocalDatabase.Instance.userPreferences.musicSetting;
		sliderSFX.value = LocalDatabase.Instance.userPreferences.soundSetting;

		transform.SetAsLastSibling();
	}

	public void SetVolumeMusic(float value)
	{
		LocalDatabase.Instance.SetVolumeMusic(value);
	}

	public void SetVolumeSFX(float value)
	{
		LocalDatabase.Instance.SetVolumeSFX(value);
	}

	private void OnDisable()
	{
		Time.timeScale = 1;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitScene()
	{
		SceneManager.LoadScene(exitDestination);
	}
}
