using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UnitGraphics : MonoBehaviour, IOverclockable
{

	public int highestOrder;
	public string animSortingLayer;
	public int level;

	public DamageData damageData;

	private Color sustainedColor;
	private bool sustained;

	public List<SpriteRenderer> renderers;
	public List<int> initialSort = new List<int>();

	public Animator anim;

	private bool priding;

	private bool colorPhasing;

	private List<Color> rainbow = new List<Color>();

	private int rainbowIndex;

	private List<Color> lastColorValues = new List<Color>();

	private Sequence colorSequence;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		if (!anim)
		{
			anim = GetComponentInChildren<Animator>();
		}

		renderers = new List<SpriteRenderer>();

		foreach (var item in GetComponentsInChildren<SpriteRenderer>(true))
		{
			renderers.Add(item);
		}

		foreach (var item in renderers)
		{
			initialSort.Add(item.sortingOrder);
			if (highestOrder < item.sortingOrder)
			{
				highestOrder = item.sortingOrder;
			}
		}
	}

	public int GetHighestSortingOrder()
	{
		int order = 0;
		foreach (var rend in renderers)
		{
			if (rend.sortingOrder > order)
			{
				order = rend.sortingOrder;
			}
		}

		return order;
	}

	public void MakeRainbow()
	{
		rainbow = new List<Color>
		{
			Color.white,
			Color.red,
			Color.yellow,
			Color.green,
			Color.cyan,
			Color.magenta
		};
	}

	public void OnActivateBool(bool active)
	{
		anim.SetBool("Active", active);
	}

	public void OnActivateTrigger(bool active)
	{
		if (!active)
		{
			anim.SetTrigger("Active");
		}
	}

	public void OnActivateTrigger()
	{
		anim.SetTrigger("Active");
	}

	public void OnActivePlay()
	{
		anim.Play("Active", 0, 0);
	}

	public void OnPrepPlay()
	{
		anim.Play("Prep", 0, 0);
	}

	public void OnReadyPlay()
	{
		anim.Play("Ready", 0, 0);
	}

	public void OnRecoverTrigger()
	{
		anim.SetTrigger("Recover");
	}

	public void AdjustSortingOrder(int sort)
	{
		foreach (var item in renderers)
		{
			item.sortingOrder = sort + initialSort[renderers.IndexOf(item)];
		}
	}

	public void SetSortingLayer(string s)
	{
		animSortingLayer = s;
		foreach (var item in renderers)
		{
			item.sortingLayerName = animSortingLayer;
		}
	}

	public void ShowDamage()
	{
		if (renderers != null)
		{
			foreach (var item in renderers)
			{

				item.material = damageData.materialHit;
			}
		}
	}

	public void ShowNormal()
	{
		if (renderers != null)
		{
			foreach (var item in renderers)
			{
				item.material = damageData.materialDefault;
				if (sustained)
				{
					SetRenderColors(sustainedColor);
				}
			}
		}
	}

	public IEnumerator DamagePhase(float seconds)
	{
		ShowDamage();
		yield return new WaitForSeconds(seconds);
		ShowNormal();
		yield return null;
	}

	public void SetRenderColors(Color colors)
	{
		if (renderers != null)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].material.color = colors;
			}
		}
	}

	public void ShowSustainedColor(Color color)
	{
		sustained = true;
		sustainedColor = color;
		SetRenderColors(sustainedColor);
	}

	public void HideSusainedColor()
	{
		sustained = false;
		ShowNormal();
	}

	public virtual void OnHealthChanged(GizLib.IntValueChange change)
	{
		if (change.value < change.lastValue)
		{
			StartCoroutine(DamagePhase(damageData.hitDuration));
		}
	}

	public void StartRainbow()
	{
		priding = true;
		lastColorValues.Clear();
		foreach (var item in renderers)
		{
			lastColorValues.Add(item.color);
		}
		if (rainbow.Count == 0)
		{
			MakeRainbow();
		}
		rainbowIndex = Random.Range(0, rainbow.Count);

		colorSequence = DOTween.Sequence();
		for (int i = 0; i < renderers.Count; i++)
		{
			colorSequence.Join(renderers[i].DOColor(rainbow[rainbowIndex], 0.25f));
		}
		colorSequence.OnComplete(ContinueRainbow);
	}

	private void ContinueRainbow()
	{
		var nextColor = Color.white;
		colorSequence = DOTween.Sequence();
		if (priding)
		{
			colorSequence.OnComplete(ContinueRainbow);
			rainbowIndex++;
			if (rainbowIndex >= rainbow.Count)
			{
				rainbowIndex = 0;
			}
			nextColor = rainbow[rainbowIndex];
			for (int i = 0; i < renderers.Count; i++)
			{
				colorSequence.Join(renderers[i].DOColor(nextColor, 0.5f));
			}
		}
		else
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				colorSequence.Join(renderers[i].DOColor(lastColorValues[i], 0.5f));
			}
		}
	}



	public void EndRainbow()
	{
		priding = false;
	}

	public void StartColorPhase(float duration, Color phaseColor)
	{
		colorPhasing = true;
		SetRenderColors(Color.white);
		colorSequence = DOTween.Sequence();
		for (int i = 0; i < renderers.Count; i++)
		{
			colorSequence.Join(renderers[i].DOColor(phaseColor, duration));
		}
		colorSequence.OnComplete(() => ContinueColorPhase(duration, phaseColor, true));
	}

	private void ContinueColorPhase(float duration, Color phaseColor, bool tick)
	{
		var nextColor = Color.white;
		colorSequence = DOTween.Sequence();
		if (colorPhasing)
		{
			tick = !tick;
			if (tick)
			{
				nextColor = phaseColor;
			}
			colorSequence.OnComplete(() => ContinueColorPhase(duration, phaseColor, tick));
		}
		for (int i = 0; i < renderers.Count; i++)
		{
			colorSequence.Join(renderers[i].DOColor(nextColor, duration));
		}
	}

	public void EndColorPhase()
	{
		colorPhasing = false;
	}

	public void SetOverclock(bool active)
	{
		if (active)
		{
			StartRainbow();
		}
		else
		{
			EndRainbow();
		}
	}

	public void Overclocking(float duration)
	{

	}
}
