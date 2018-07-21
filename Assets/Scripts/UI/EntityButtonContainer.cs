using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EntityButtonContainer : MonoBehaviour
{
	public GameObject selectClickInterface;

	public GameObject shipInterface;

	public PlayerProgressData playerProgress;
	public List<GizmoidData> gizmoids = new List<GizmoidData>();

	public UnitRoster gizmoidRoster;

	public UnitSelectorButton unitSelectorPrefab;

	public PlacementPreview placementPreview;

	public ToggleDragGroup toggleDragGroup;

	public List<UnitSelectorButton> unitButtons = new List<UnitSelectorButton>();

	public UnityEvent onButtonsPlaced = new UnityEvent();

	[SerializeField]
	private CanvasGroup canvasGroup;

	private Sequence sequence;

	public void BuildTestButtons()
	{
		BuildUnitButtons(gizmoids);
	}

	public void BuildUnitButtons(List<int> loadoutIndexes)
	{

		var data = gizmoidRoster.unitDatas.FindAll(x => loadoutIndexes.Contains((x as GizmoidData).typeId)).Cast<GizmoidData>().OrderBy(x => x.order).ToList();
		BuildUnitButtons(data);
	}

	public void BuildUnitButtons(List<GizmoidData> loadout)
	{
		canvasGroup.interactable = false;
		var newButtons = new List<UnitSelectorButton>();
		foreach (var item in loadout)
		{
			if (unitButtons.Any(x => x.GetUnitData() == item))
			{
				continue;
			}
			var button = BuildUnitButton(item, transform);
			button.transform.localScale = Vector3.zero;
			newButtons.Add(button);
			unitButtons.Add(button);
		}

		sequence = DOTween.Sequence();
		foreach (var item in newButtons)
		{
			sequence.Append(item.transform.DOScale(1, 0.35f).SetEase(DG.Tweening.Ease.OutBack));
		}
		sequence.AppendCallback(OnButtonsPlaced);
		sequence.OnComplete(onButtonsPlaced.Invoke);
	}

	private void OnButtonsPlaced()
	{
		canvasGroup.interactable = true;
	}

	public void SetInteractivity(bool active)
	{
		//for (int i = 0; i < unitButtons.Count; i++)
		//{
		//	unitButtons[i].toggleDrag.interactable = active;
		//}
		canvasGroup.interactable = active;

	}

	private UnitSelectorButton BuildUnitButton(GizmoidData gizmoidData, Transform parent)
	{
		var giz = Instantiate(unitSelectorPrefab, parent);
		giz.transform.localScale = Vector3.one;

		giz.SetUnitData(gizmoidData);
		giz.onSelection.AddListener(placementPreview.OnDragEnabled);
		giz.SetCanAfford(playerProgress.Scraps);
		giz.onPurchased.AddListener(playerProgress.AdjustScrap);
		playerProgress.onScrapChanged.AddListener(giz.SetCanAfford);

		var toggle = giz.GetComponent<ToggleDrag>();
		if (toggle)
		{
			toggle.group = toggleDragGroup;
			toggle.onDrag.AddListener(placementPreview.OnDrag);
			toggle.onDragEnd.AddListener(placementPreview.OnDragEnd);
			toggle.onValueChanged.AddListener(selectClickInterface.SetActive);
			toggle.onValueChangedInverse.AddListener(shipInterface.SetActive);
		}

		return giz;
	}

}
