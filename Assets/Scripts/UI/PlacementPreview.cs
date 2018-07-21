using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementPreview : MonoBehaviour
{
	public SpriteRenderer sectorSelector;

	public GizmoidData gizmoidData;

	private BladeView selectedBlade;

	private UnitGraphics preview;

	public void OnDragEnabled(bool enabled, GizmoidData data)
	{
		gameObject.SetActive(enabled);
		if (enabled)
		{
			gizmoidData = data;
			if (data.unitGraphics)
			{
				ChangePreview(data.unitGraphics);
			}
		}
		else
		{
			sectorSelector.gameObject.SetActive(false);
		}
	}

	public void OnDragEnableEvent(bool enabled, PointerEventData eventData)
	{
		gameObject.SetActive(enabled);
	}

	public void OnDrag(PointerEventData eventData)
	{
		var pointerPosition = GizLib.Helper.GetWorldPoint();
		transform.position = new Vector3(pointerPosition.x, pointerPosition.y, transform.position.z);
		CheckSector();
	}

	public void ChangePreview(UnitGraphics animationManager)
	{
		//RemoveGraphics();
		if (preview)
		{
			Destroy(preview.gameObject);
		}
		preview = Instantiate(animationManager, transform);
		preview.transform.localScale = Vector3.one;
		preview.transform.localPosition = Vector3.zero;
		preview.SetSortingLayer("Preview");
	}

	public void RemoveGraphics()
	{
		for (var i = 0; i < transform.childCount; i++)
		{
			var trans = transform.GetChild(i);
			Destroy(trans.gameObject);
		}
	}

	public void CheckSector()
	{
		var collision = GetComponent<Collider2D>();
		var points = new Collider2D[50];
		var count = collision.GetContacts(points);
		if (count < points.Length)
		{
			System.Array.Resize(ref points, count);
		}
		for (int i = 0; i < points.Length; i++)
		{
			Debug.Log(points[i].name);
			var sector = points[i].GetComponent<SectorHandler>();
			if (sector)
			{
				var index = i;
				OnEnterSector(points[index]);

				break;
			}
		}
	}

	public void OnEnterSector(Collider2D collision)
	{
		var points = new Collider2D[50];
		var count = collision.GetContacts(points);
		if (count < points.Length)
		{
			System.Array.Resize(ref points, count);
		}
		bool foundBlade = false;
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i].gameObject != gameObject && points[i].gameObject.tag == "Blade")
			{
				selectedBlade = points[i].GetComponent<BladeView>();
				if (!selectedBlade)
				{
					selectedBlade = points[i].GetComponentInParent<BladeView>();
				}

				if (selectedBlade)
				{
					foundBlade = selectedBlade.gizmoidHolder.gizmoids.Count < 6;
					break;
				}
			}
		}
		if (!foundBlade)
		{
			selectedBlade = null;
		}
		DisplaySectorSelector(collision.transform.eulerAngles.z, foundBlade);
	}

	public void OnDragEnd(PointerEventData eventData)
	{
		CheckSector();
		if (selectedBlade && selectedBlade.gizmoidHolder.gizmoids.Count < 6 && gizmoidData != null)
		{
			selectedBlade.gizmoidHolder.PlacePreview(gizmoidData, preview);
			if (eventData.pointerDrag)
			{
				var unitSelect = eventData.pointerDrag.GetComponent<UnitSelectorButton>();
				if (unitSelect)
				{
					unitSelect.Acquire();
				}
			}
			preview = null;
			selectedBlade = null;
		}
		sectorSelector.gameObject.SetActive(false);
		transform.position = new Vector3(10, 0, 0);
		gameObject.SetActive(false);
	}

	public void OnDragEndRemove(PointerEventData eventData)
	{
		var points = new Collider2D[50];

		var count = GetComponent<Collider2D>().GetContacts(points);
		if (count < points.Length)
		{
			System.Array.Resize(ref points, count);
		}

		for (int i = 0; i < points.Length; i++)
		{
			var giz = points[i].GetComponent<GizmoidView>();
			if (giz)
			{
				giz.SelfDestruct();
				break;
			}
		}
		sectorSelector.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	public void OnDragEndOverclock(PointerEventData eventData)
	{
		OverclockPanel ocp = null;
		if (eventData.pointerDrag)
		{
			ocp = eventData.pointerDrag.GetComponent<OverclockPanel>();
		}

		if (!ocp)
		{
			return;
		}

		var points = new Collider2D[50];

		var count = GetComponent<Collider2D>().GetContacts(points);
		if (count < points.Length)
		{
			System.Array.Resize(ref points, count);
		}

		for (int i = 0; i < points.Length; i++)
		{
			var giz = points[i].GetComponent<GizmoidView>();
			if (giz)
			{
				ocp.playerProgress.AdjustOverclock(-1);
				giz.Overclocking(ocp.playerProgress.overclockDuration);
				break;
			}
		}
		sectorSelector.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	public void DisplaySectorSelector(float angle, bool active)
	{
		if (!sectorSelector.gameObject.activeSelf)
		{
			sectorSelector.gameObject.SetActive(true);
		}
		sectorSelector.transform.position = Vector3.zero;
		sectorSelector.transform.eulerAngles = new Vector3(0, 0, angle);
		if (active)
		{
			sectorSelector.color = Color.cyan;
		}
		else
		{
			sectorSelector.color = Color.red;
		}
	}

}
