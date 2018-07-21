using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPanel : MonoBehaviour
{
	public UnitRoster gizmoidRoster;

	public UnitPreviewPanel previewPanel;

	public LoadoutUnitSelector loadoutUnitPrefab;

	public Transform unitContent;

	public Text TextSelectCount;

	public Button buttonContinue;

	public List<GizmoidData> selectedData = new List<GizmoidData>();

	private GizLib.Profile profile;

	public void PopulateUnitGrid()
	{
		profile = LocalDatabase.Instance.activeProfile;
		if (profile != null)
		{
			var data = gizmoidRoster.unitDatas.FindAll(x => profile.gizmoids.Contains((x as GizmoidData).typeId)).Cast<GizmoidData>().OrderBy(x => x.order).ToList();
			foreach (var item in data)
			{
				BuildLoadoutUnit(item);
			}
			TextSelectCount.text = selectedData.Count + "/" + profile.gizmoidSlots;

			var randData = data[Random.Range(0, data.Count)];
			previewPanel.SetPanel(randData.unitGraphics, randData.nameFull, randData.description);

		}
	}

	public void BuildLoadoutUnit(GizmoidData data)
	{
		var unit = Instantiate(loadoutUnitPrefab, unitContent);
		unit.transform.localScale = Vector3.one;

		unit.SetUnitData(data);
		unit.onSelected.AddListener(OnUnitSelected);
	}

	private void OnUnitSelected(bool active, LoadoutUnitSelector unit)
	{
		var data = unit.GetUnitData();
		if (profile != null)
		{
			if (active)
			{
				if (selectedData.Count < profile.gizmoidSlots && !selectedData.Contains(data))
				{
					selectedData.Add(data);
				}
				else
				{
					unit.selectOverride = true;
					unit.toggle.isOn = false;
				}
			}
			else
			{
				if (selectedData.Contains(data))
				{
					selectedData.Remove(data);
				}
			}
			buttonContinue.interactable = selectedData.Count == profile.gizmoidSlots;
			TextSelectCount.text = selectedData.Count + "/" + profile.gizmoidSlots;
		}

		previewPanel.SetPanel(data.unitGraphics, data.nameFull, data.description);
	}

	public void SetLoadout()
	{
		var list = new List<int>();
		foreach (var item in selectedData.OrderBy(x => x.order))
		{
			list.Add(item.typeId);
		}

		SessionManager.Instance.gizmoidLoadout = list;

	}
}
