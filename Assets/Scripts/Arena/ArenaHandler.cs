using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArenaHandler : MonoBehaviour
{
	public static ArenaHandler instance;
	public List<SectorHandler> sectors;

	public SectorDataList sectorDataList;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{
		foreach (var item in sectors)
		{
			item.name = "Sector" + sectors.IndexOf(item);
		}
	}

	public void GetArenaData()
	{
		sectorDataList.sectorData.Clear();
		foreach (var sector in sectors)
		{
			var rows = sector.GetRows();
			foreach (var row in rows)
			{
				var data = new GizLib.SectorData();
				data.name = "Sector " + sectors.IndexOf(sector) + " - Row " + rows.IndexOf(row);
				data.aisle = sectors.IndexOf(sector);
				data.place = rows.IndexOf(row);
				data.angle = sector.transform.eulerAngles.z;
				var polyColl = row.GetComponent<PolygonCollider2D>();
				if (polyColl)
				{
					for (int i = 0; i < polyColl.pathCount; i++)
					{
						var path = polyColl.GetPath(i);
						data.paths.Add(new GizLib.Path(path));
					}
				}
				sectorDataList.sectorData.Add(data);
			}
		}
	}

	public SectorHandler GetSectorByNearestRotation(float rotation)
	{
		return sectors.Aggregate((x, y) => Mathf.Abs(x.transform.eulerAngles.z - rotation) < Mathf.Abs(y.transform.eulerAngles.z - rotation) ? x : y);
	}

}
