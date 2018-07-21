using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{

	private static LevelDataManager _instance;
	public static LevelDataManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<LevelDataManager>();
				if (_instance == null)
				{
					var go = new GameObject("Level Data Manager");
					_instance = go.AddComponent<LevelDataManager>();
				}
			}
			return _instance;
		}
	}

	public LevelDataSet levelDataSet;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			if (this != _instance)
			{
				Destroy(gameObject);
			}
		}
		if (!levelDataSet)
		{
			levelDataSet = Resources.Load<LevelDataSet>("Level Data Set");
		}
	}

	public int GetLevelDataByIndex(int worldId, int localId)
	{
		var outputLevel = levelDataSet.dataSet.Find(level => level.localId == localId && level.worldId == worldId);
		return levelDataSet.dataSet.IndexOf(outputLevel);
	}
}
