using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XenoPanel : MonoBehaviour
{
	public PlayerProgressData playerProgress;

	public Text xenoText;

	// Use this for initialization
	void Start()
	{
		playerProgress.onXenoChanged.AddListener(OnXenoChanged);
		xenoText.text = playerProgress.Xenos.ToString();

	}

	public void OnXenoChanged(int value)
	{
		xenoText.text = value.ToString();
	}

}
