using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UnitSelectorButton : MonoBehaviour
{
	[System.Serializable]
	public class AnimEvent : UnityEvent<bool, GizmoidData>
	{ }

	[System.Serializable]
	public class PurchaseEvent : UnityEvent<int> { }

	[SerializeField]
	private GizmoidData unitData;

	public Image icon;

	public Image cooldownImage;

	public Image affordImage;

	public Text textPrice;

	public ToggleDrag toggleDrag;

	public AnimEvent onSelection = new AnimEvent();

	public PurchaseEvent onPurchased = new PurchaseEvent();

	bool coolingDown;

	bool canAfford;

	Sequence sequence;

	private void Start()
	{
	}

	public void SetUnitData(GizmoidData data)
	{
		name = "Button " + data.nameFull;
		unitData = data;
		textPrice.text = data.Price.ToString();
		SetIcon(data.Icon);
	}

	public GizmoidData GetUnitData()
	{
		return unitData;
	}

	public void OnDragEnabled(bool enabled, PointerEventData eventData)
	{
		onSelection.Invoke(enabled, unitData);
	}

	public void SetIcon(Sprite sprite)
	{
		icon.sprite = affordImage.sprite = cooldownImage.sprite = sprite;
	}

	public void SetCanAfford(int value)
	{
		canAfford = value >= unitData.Price;

		toggleDrag.interactable = !coolingDown && canAfford;

		if (canAfford)
		{
			textPrice.color = Color.white;
			affordImage.gameObject.SetActive(false);
		}
		else
		{
			textPrice.color = Color.red;
			affordImage.gameObject.SetActive(true);
		}
	}

	public void Acquire()
	{
		onPurchased.Invoke(unitData.Price * -1);
		StartCooldown();
	}

	public void StartCooldown()
	{
		toggleDrag.interactable = false;
		coolingDown = true;
		cooldownImage.fillAmount = 1;
		cooldownImage.DOFillAmount(0, unitData.Cooldown).OnComplete(CooldownCompleted);
	}

	public void CooldownCompleted()
	{
		coolingDown = false;
		toggleDrag.interactable = !coolingDown && canAfford;
	}
}
