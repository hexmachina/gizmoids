using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	[System.Serializable]
	public class EnemyEvent : UnityEvent<EnemyView> { }

	[System.Serializable]
	public class EnemiesEvent : UnityEvent<List<EnemyView>> { }

	public static EnemySpawner Instance;

	public UnitRoster enemyRoster;

	//Inspector
	public Transform goal;
	public List<Vector3> originPoints;
	public EnemyView enemySheet;

	public ArrangeSortingLayer arrangeSortingLayer;

	public List<EnemyView> enemies = new List<EnemyView>();

	public EnemyEvent onSpawned = new EnemyEvent();

	public EnemiesEvent onSpawnedDestroyed = new EnemiesEvent();

	void Awake()
	{
		Instance = this;
	}

	public EnemyView SpawnEnemy(int enemyId, int spawnLane = -1) //Lane relative to front of ship, -1 indicates random
	{
		var data = enemyRoster.unitDatas.Find(x => x.typeId == enemyId) as EnemyData;
		return SpawnEnemy(data, spawnLane);
	}

	public EnemyView SpawnEnemy(EnemyData data, int spawnLane = -1)
	{
		float jitter = Random.value * 0.2f;
		Vector3 origin = Vector3.zero;
		if (spawnLane == -1)
		{
			spawnLane = Random.Range(0, 7);
		}
		else
		{
			spawnLane = Mathf.Max(0, spawnLane);
		}

		origin = Mathfx.GetPointOnCircle(5.5f + jitter, (spawnLane * 45), Vector3.zero);
		return SpawnEnemy(data, origin, Quaternion.Euler(0, 0, (spawnLane * 45) - 90));
		//var es = Instantiate(enemySheet, origin, Quaternion.Euler(0, 0, (spawnLane * 45) - 90), transform);
		////es.transform.eulerAngles = new Vector3(0, 0, (spawnLane * 45) - 90);
		//es.SetData(data);
		//es.mover.Mobilize(es.mover.transform.position, goal.position, es.enemyData.speed);

		//arrangeSortingLayer.CheckandReorder();

		//onSpawned.Invoke(es);

		//enemies.Add(es);
		//es.onDestroy.AddListener(OnEnemyDestroyed);
		//return es;
	}

	public EnemyView SpawnEnemy(EnemyData data, Vector3 origin, Quaternion rotation)
	{
		var es = Instantiate(enemySheet, origin, rotation, transform);

		es.SetData(data);
		es.mover.Mobilize(es.mover.transform.position, goal.position, es.enemyData.speed);

		arrangeSortingLayer.CheckandReorder();

		onSpawned.Invoke(es);

		enemies.Add(es);
		es.onDestroy.AddListener(OnEnemyDestroyed);
		return es;
	}



	private void OnEnemyDestroyed(EntityView view)
	{
		if (view is EnemyView)
		{
			var enview = view as EnemyView;
			if (enemies.Contains(enview))
			{
				enemies.Remove(enview);
				onSpawnedDestroyed.Invoke(enemies);
			}
		}
	}

	public EnemyData ChooseRandomEnemy(List<EnemyData> possibleEnemies)
	{
		var enemyFrequencies = new Dictionary<float, EnemyData>();
		float sumFrequency = 0;
		foreach (var item in possibleEnemies)
		{
			enemyFrequencies.Add(item.frequency + sumFrequency, item);
			sumFrequency += item.frequency;
		}

		float randomNumber = Random.value * sumFrequency;

		foreach (var item in enemyFrequencies)
		{
			if (randomNumber <= item.Key)
			{
				return item.Value;
			}
		}
		return null;
	}

}
