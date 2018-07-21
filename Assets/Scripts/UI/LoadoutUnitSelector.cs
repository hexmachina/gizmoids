using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadoutUnitSelector : MonoBehaviour
{
	[System.Serializable]
	public class LoadoutUnitEvent : UnityEvent<bool, LoadoutUnitSelector>
	{ }

	[SerializeField]
	private GizmoidData unitData;

	public Image icon;

	public Text textPrice;

	public Toggle toggle;

	public LoadoutUnitEvent onSelected = new LoadoutUnitEvent();

	public bool selectOverride = false;

	public void SetUnitData(GizmoidData data)
	{
		name = "Button " + data.nameFull;
		unitData = data;
		textPrice.text = data.Price.ToString();
		icon.sprite = data.Icon;
	}

	public GizmoidData GetUnitData()
	{
		return unitData;
	}

	public void OnToggle(bool active)
	{
		if (!selectOverride)
		{
			onSelected.Invoke(active, this);
		}
		selectOverride = false;
	}
}
