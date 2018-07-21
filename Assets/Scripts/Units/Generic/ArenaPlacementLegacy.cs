using UnityEngine;
using System.Collections;

public class ArenaPlacementLegacy : MonoBehaviour
{

	public int row = -1;
	public int aisle = -1;
	//public RowHandler currentRow;
	//private RowHandler lastRow;
	public SectorHandler currentAisle;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


	public void SetRow(int r)
	{
		row = r;
	}

	//public int GetRow()
	//{
	//	return row;
	//}

	public int GetAisle()
	{
		return aisle;
	}

	public void SetAisle(int a)
	{
		aisle = a;

	}

	public void SetCurrentAisle(SectorHandler s)
	{
		//if (tag == "Blade Point")
		//{
		//	if (currentAisle != null)
		//	{
		//		currentAisle.currentBlade = null;
		//	}
		//	var blade = GetComponentInParent<BladeBehaviourScript>();
		//	s.currentBlade = blade;
		//}
		currentAisle = s;
		//aisle = currentAisle.place;
	}

	public void SetCurrentRow(RowHandler r)
	{
		//currentRow = r;
	}

	public void UpdatePlacement(RowHandler r)
	{

		//lastRow = currentRow;
		//currentRow = r;
		//currentAisle = r.sector;
		row = r.place;
		aisle = r.aisle;
		//if (lastRow)
		//{
		//	lastRow.RemoveOccupier(gameObject);

		//}

		//currentRow.AddGroupDictionary(this);
	}

	//public void ClearFromRow()
	//{
	//	if (currentRow != null)
	//		currentRow.RemoveOccupier(gameObject);
	//}

	//public void ClearFromRow(GameObject go)
	//{
	//	if (currentRow != null)
	//		currentRow.RemoveOccupier(go);
	//}

	public void Reset()
	{
		//ClearFromRow();
		currentAisle = null;
		//currentRow = null;
		row = -1;
		aisle = -1;
	}
}
