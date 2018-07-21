using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverclockPanel : MonoBehaviour
{
	public PlayerProgressData playerProgress;

	public ToggleDrag toggleDrag;

	public List<Image> beacons = new List<Image>();

	private void Start()
	{
		playerProgress.onOverclockChanged.AddListener(OnOverclockChange);
		OnOverclockChange(playerProgress.OverclockTokens);
	}

	public void OnOverclockChange(int value)
	{
		for (int i = 0; i < beacons.Count; i++)
		{
			beacons[i].gameObject.SetActive(i + 1 <= value);
		}

		toggleDrag.interactable = value != 0;
	}
}
