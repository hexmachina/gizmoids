using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListeningRow
{
	public bool[] rows;
}

[System.Serializable]
public class ListeningGrid
{
	public bool topDown;
	public bool isMirrored;
	public List<ListeningRow> aisles;
}

public class CheckArenaLegacy : MonoBehaviour
{

	public bool hasCompany;
	public List<string> whiteList;
	//public ArenaHandler arena;
	//public ArenaPlacement placement;

	//public List<RowHandler> rows;

	public ListeningGrid listeningGrid;
	//public int currentAisle;
	//public int currentRow;

	// Use this for initialization
	void Start()
	{
		//currentAisle = placement.aisle;
		//currentRow = placement.row;
		//CollectRows();
	}

	// Update is called once per frame
	//void Update () {
	//       if (placement.aisle != currentAisle || placement.row != currentRow)
	//       {
	//           CollectRows();
	//           currentAisle = placement.aisle;
	//           currentRow = placement.row;
	//       }
	//       //CheckAisle();
	//}

	//void FixedUpdate()
	//{

	//}

	public void SetArenaHandler(ArenaHandler ar)
	{
		//arena = ar;
	}

	public bool CheckAisle()
	{
		bool found = false;
		//foreach (var item in GetListeningRows())
		//{
		//	if (item.occupied)
		//	{
		//		foreach (var white in whiteList)
		//		{
		//			if (item.IsOccupiedBy(white))
		//			{
		//				found = true;
		//				break;
		//			}
		//			else
		//			{
		//				found = false;
		//			}
		//		}
		//	}
		//	if (found)
		//	{
		//		break;
		//	}
		//}
		hasCompany = found;
		return hasCompany;
		//SendMessage("SetActivation", hasCompany, SendMessageOptions.DontRequireReceiver);

	}

	//public List<RowHandler> GetListeningRows()
	//{
	//	var rows = new List<RowHandler>();
	//	foreach (var item in listeningGrid.aisles)
	//	{
	//		for (var i = 0; i < item.rows.Length; i++)
	//		{
	//			if (item.rows[i])
	//			{
	//				int row;
	//				if (listeningGrid.topDown)
	//				{
	//					row = placement.row - i;
	//				}
	//				else
	//				{
	//					row = placement.row + i;
	//				}
	//				if (row < 8 && row > -1)
	//				{
	//					int sector = placement.aisle + listeningGrid.aisles.IndexOf(item);
	//					if (sector > 7)
	//					{
	//						sector -= 8;
	//					}
	//					rows.Add(ArenaHandler.instance.sectors[sector].rows[row]);
	//				}
	//			}
	//		}
	//	}
	//	return rows;
	//}
}
