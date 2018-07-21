using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayInterface : MonoBehaviour
{
	public EntityButtonContainer entityButtonContainer;

	public ScrapPanel scrapPanel;

	public XenoPanel xenoPanel;

	public OverclockPanel overclockPanel;

	public GameObject buttonRemoveGizmoid;

	public SpaceshipInterface spaceshipInterface;

	public Text promptLabel;

	public DialoguePanel dialoguePanelPrefab;

	public LoadoutPanel loadoutPanelPrefab;

	public AlertMessage alertMessage;

	public Toggle toggleSpeed;

	[UnityEngine.Serialization.FormerlySerializedAs("buttonSpeed")]
	public Button buttonPause;

	public HandIndicatorUI handIndicatorPrefab;

	public UnitPreviewPanel previewPanelPrefab;

	public float fastForwardSpeed = 2;

	private DialoguePanel dialoguePanel;

	private HandIndicatorUI handIndicator;

	private List<GameObject> unlockedObjects = new List<GameObject>();

	private Sequence sequence;

	public void ShowPromptLabel(string message)
	{
		promptLabel.text = message;
		ShowPromptLabel();
	}

	public void ShowPromptLabel()
	{
		if (sequence != null && sequence.IsPlaying())
		{
			sequence.Kill();
		}

		promptLabel.gameObject.SetActive(true);
		promptLabel.transform.SetAsLastSibling();

		sequence = DOTween.Sequence();

		sequence.Append(promptLabel.DOFade(0, 0.0f));
		sequence.Append(promptLabel.DOFade(1, 0.5f));
	}

	public void SetInteractive(bool active)
	{
		spaceshipInterface.interactable = active;
		entityButtonContainer.SetInteractivity(active);

		toggleSpeed.interactable = active;
		buttonPause.interactable = active;
	}

	public void SetDisplay(bool active)
	{
		foreach (var item in entityButtonContainer.unitButtons)
		{
			item.gameObject.SetActive(active);
		}
		foreach (var item in unlockedObjects)
		{
			item.SetActive(active);
		}
		toggleSpeed.gameObject.SetActive(active);
		buttonPause.gameObject.SetActive(active);
		scrapPanel.gameObject.SetActive(active);
	}

	public void PauseTime(bool pause)
	{
		if (pause)
		{
			Time.timeScale = 0;
		}
		else
		{
			if (toggleSpeed.isOn)
			{
				Time.timeScale = fastForwardSpeed;
			}
			else
			{
				Time.timeScale = 1;
			}
		}
	}

	public void FastForward(bool fast)
	{
		if (fast)
		{
			Time.timeScale = fastForwardSpeed;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public void HidePromptLabel()
	{
		if (promptLabel.gameObject.activeSelf)
		{
			sequence = DOTween.Sequence();

			sequence.Append(promptLabel.DOFade(0, 0.35f));
			sequence.OnComplete(FadeEndPromptLabel);
		}
	}

	private void FadeEndPromptLabel()
	{
		promptLabel.gameObject.SetActive(false);
	}

	public DialoguePanel GetDialoguePanel()
	{
		if (!dialoguePanel)
		{
			dialoguePanel = Instantiate(dialoguePanelPrefab, transform);
			dialoguePanel.transform.localScale = Vector3.one;
		}
		dialoguePanel.gameObject.SetActive(true);
		return dialoguePanel;
	}

	public HandIndicatorUI GetHandIndicator(bool makeInstant = false)
	{
		if (!handIndicator && makeInstant)
		{
			handIndicator = Instantiate(handIndicatorPrefab, transform);
			handIndicator.transform.localScale = Vector3.one;
		}

		return handIndicator;
	}

	public void CheckUnlockedFeatures(List<string> features)
	{
		UnlockPanel(features, "xeno", xenoPanel.gameObject);
		UnlockPanel(features, "overclock", overclockPanel.gameObject);
		UnlockPanel(features, "remove", buttonRemoveGizmoid.gameObject);
	}

	public void UnlockPanel(List<string> features, string key, GameObject go)
	{
		var found = features.Contains(key);
		go.SetActive(found);
		if (found && !unlockedObjects.Contains(go))
		{
			unlockedObjects.Add(go);
		}
	}

	public void BuildUnitPreview(EntityData data, System.Action callback)
	{
		var unitPreview = Instantiate(previewPanelPrefab, transform);
		unitPreview.transform.localScale = Vector3.one;
		unitPreview.SetPanel(data.unitGraphics, "New Gizmoid: " + data.nameFull + "!", data.description, callback);
	}


	public void BuildLoadoutPanel(System.Action callback)
	{
		var loadout = Instantiate(loadoutPanelPrefab, transform);
		loadout.transform.localScale = Vector3.one;

		loadout.PopulateUnitGrid();
		loadout.buttonContinue.onClick.AddListener(callback.Invoke);
	}
}
