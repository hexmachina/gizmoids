using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Graphic))]
public class RainbowColorFadeImage : MonoBehaviour
{
	Sequence sequence;

	Graphic graphic;

	List<Color> rainbow = new List<Color>();

	int index = 0;

	// Use this for initialization
	void Start()
	{
		graphic = GetComponent<Graphic>();
		MakeRainbow();
		sequence = DOTween.Sequence();
		sequence.Append(graphic.DOColor(rainbow[index], 0.5f))
			.OnComplete(LoopColorFade);
	}

	private void LoopColorFade()
	{
		sequence = DOTween.Sequence();
		index++;
		if (index >= rainbow.Count)
		{
			index = 0;
		}
		sequence.Append(graphic.DOColor(rainbow[index], 0.5f))
			.OnComplete(LoopColorFade);
	}

	public void MakeRainbow()
	{
		rainbow = new List<Color>
		{
			Color.red,
			Color.yellow,
			Color.green,
			Color.cyan,
			Color.magenta
		};
	}
}
