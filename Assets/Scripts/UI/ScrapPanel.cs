using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrapPanel : MonoBehaviour
{
	public PlayerProgressData playerProgress;

	public Text scrapText;

	// Use this for initialization
	void Start()
	{
		playerProgress.onScrapChanged.AddListener(OnScrapChanged);
		scrapText.text = playerProgress.Scraps.ToString();

	}

	public void OnScrapChanged(int value)
	{
		scrapText.text = value.ToString();
	}
}
