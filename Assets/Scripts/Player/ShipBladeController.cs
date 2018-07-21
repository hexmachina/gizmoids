using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;


public class ShipBladeController : MonoBehaviour
{
	//public static ShipBladeController Instance;

	public BladeData bladeData;

	[UnityEngine.Serialization.FormerlySerializedAs("blade")]
	public BladeView bladePrefab;

	public int maxBlades = 8;

	public List<BladeView> blades;

	public UnityEvent onBladesPlaced = new UnityEvent();

	private Sequence sequence;

	//void Awake()
	//{
	//	Instance = this;
	//}

	void AlignBlades()
	{
		foreach (var item in blades)
		{
			item.transform.parent = transform;
			item.transform.localPosition = Vector3.zero;
		}
	}

	public void PlaceBlades(int bladeCount)
	{
		var newBlades = new List<BladeView>();
		bladeCount += blades.Count;
		if (bladeCount > maxBlades)
		{
			bladeCount = maxBlades;
		}

		var indexArray = new int[] { 0, 7, 1, 6, 2, 5, 3, 4 };
		for (int i = blades.Count; i < bladeCount; i++)
		{
			newBlades.Add(AddBlade(indexArray[i]));
		}

		sequence = DOTween.Sequence();
		foreach (var item in newBlades)
		{
			sequence.AppendCallback(() => AudioPlayer.Instance.PlaySoundClip(item.clipPlace));
			sequence.Append(item.transform.DOScaleY(1, 0.6f).SetEase(DG.Tweening.Ease.OutBack));
		}
		sequence.OnComplete(onBladesPlaced.Invoke);
	}

	private BladeView AddBlade(int i)
	{
		float angle = 45 * i;
		var circ = Mathfx.GetPointOnCircle(bladePrefab.radiusOffset, angle + 90, Vector3.zero);
		var b = Instantiate(bladePrefab, transform);
		b.transform.localPosition = circ;
		b.transform.localEulerAngles = new Vector3(0, 0, angle);
		b.name = "Blade" + i;
		b.transform.localScale = new Vector3(1, 0, 1);
		b.bladeData = bladeData;

		if (bladeData.spriteStates.Count > 0)
		{
			b.spriteRenderer.sprite = bladeData.spriteStates[0];
		}

		switch (bladeData.type)
		{
			case GizLib.BladeType.Default:
				var dam = b.gameObject.AddComponent<BladeDamage>();
				b.onImpact.AddListener(dam.HandleImpact);
				break;
		}
		blades.Add(b);
		return b;
	}

	public BladeView GetAvailableBlade(int aisle)
	{
		BladeView selectedBlade = null;
		return selectedBlade;
	}
}
