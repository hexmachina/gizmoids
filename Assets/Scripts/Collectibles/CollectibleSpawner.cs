using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CollectibleSpawner : MonoBehaviour
{
	[System.Serializable]
	public class CollectibleEvent : UnityEvent<CollectibleInteractive> { }

	[System.Serializable]
	public class ScrapEvent : UnityEvent<Scrap> { }

	public static CollectibleSpawner Instance;

	public PlayerProgressData playerProgress;

	[UnityEngine.Serialization.FormerlySerializedAs("scrap")]
	public Scrap scrapPrefab;

	public CollectibleData defaultCollectible;

	public CollectibleInteractive collectiblePrefab;

	public RectTransform collectibleContainer;

	public ArrangeSortingLayer arrangeSortingLayer;

	public bool allowDrops;
	public bool allowScrapDrop;
	public bool allowRareDrop;

	[SerializeField]
	private bool _spawningScrap;
	public bool SpawningScrap
	{
		get
		{
			return _spawningScrap;
		}
		set
		{
			if (value)
			{
				if (!_spawningScrap)
				{
					_spawningScrap = true;
				}
			}
			else
			{
				if (_spawningScrap)
				{
					_spawningScrap = false;
				}
			}
		}
	}

	[SerializeField]
	private bool _spawningCollectibles;
	public bool SpawningCollectibles
	{
		get
		{
			return _spawningCollectibles;
		}
		set
		{
			if (value)
			{
				if (!_spawningCollectibles)
				{
					_spawningCollectibles = true;
				}
			}
			else
			{
				if (_spawningCollectibles)
				{
					_spawningCollectibles = false;
				}
			}
		}
	}

	public float scrapInterval = 10f;
	public int scrapAmount = 5;

	public float initialCollectibleInterval;
	public float collectibleInterval;
	public float collectibleIncrement = 0.1f;

	//Status
	float scrapProgress = 0f;
	float collectibleProgress = 0f;

	public CollectibleEvent onCollectibleAdded = new CollectibleEvent();

	public ScrapEvent onScrapAdded = new ScrapEvent();

	public List<CollectibleInteractive> collectiblePool = new List<CollectibleInteractive>();
	public List<Scrap> scrapPool = new List<Scrap>();

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		//scrapProgress = scrapInterval - 1;
		//collectibleProgress = collectibleInterval - 1;
	}

	// Update is called once per frame
	void Update()
	{
		HandleCollectibleSpawning();
	}

	public void HandleCollectibleSpawning()
	{
		if (_spawningScrap)
		{
			if (scrapProgress >= scrapInterval)
			{
				scrapProgress = 0;
				SpawnScrap(scrapAmount, true);
			}
			else
			{
				scrapProgress += Time.deltaTime;
			}
		}

		if (_spawningCollectibles)
		{
			if (collectibleProgress >= collectibleInterval)
			{
				collectibleProgress = 0;
				collectibleInterval += collectibleIncrement;
				SpawnBigScrap(true);
			}
			else
			{
				collectibleProgress += Time.deltaTime;
			}
		}
	}

	public void SpawnBigScrap(bool phasesOut)
	{
		var coll = GetCollectible(defaultCollectible);
		coll.dropped = false;
		coll.phasesOut = phasesOut;
		coll.BuildButton(collectibleContainer);
		coll.Mobilize(0);
	}

	public List<Scrap> SpawnScrap(int count, bool phasesOut)
	{
		//Debug.Log("Spawned scrap");
		var direction = Random.Range(0, 7);
		var spawningScraps = new List<Scrap>();
		for (var i = 0; i < count; i++)
		{
			var sc = scrapPool.Find(x => !x.gameObject.activeSelf);
			if (sc)
			{
				sc.gameObject.SetActive(true);
			}
			else
			{
				sc = Instantiate(scrapPrefab, transform);
				sc.name = "Scrap";
				scrapPool.Add(sc);
				onScrapAdded.Invoke(sc);
				sc.onCollected.AddListener(playerProgress.OnCollected);
			}
			sc.dropped = false;
			sc.Mobilize(direction);
			sc.phasesOut = phasesOut;
			spawningScraps.Add(sc);
		}
		return spawningScraps;
	}

	public void DropScrap(int amount, bool phaseOut, float degrees, Vector3 startPosition)
	{
		var scraps = SpawnScrap(amount, phaseOut);
		foreach (var item in scraps)
		{
			item.transform.position = startPosition;
			item.mover.SetAngle(degrees);
			item.mover.SetEndPoint(item.mover.endRadius);
			item.dropped = true;
			Vector3 blow = Mathfx.GetPointOnCircle(0.5f, Random.Range(0, 360), startPosition);
			item.mover.StartMovement(MoveType.Detour, 2, blow);
		}
	}


	public CollectibleInteractive GetCollectible(CollectibleData data)
	{
		var coll = collectiblePool.Find(x => x.collectibleData == data && !x.gameObject.activeSelf);

		if (coll)
		{
			coll.gameObject.SetActive(true);
			coll.GetComponent<SpriteRenderer>().color = Color.white;
			var anim = coll.GetComponent<Animator>();
			if (anim)
			{
				anim.enabled = true;
			}
		}
		else
		{
			coll = Instantiate(collectiblePrefab, transform);
			coll.name = data.name;
			collectiblePool.Add(coll);
			onCollectibleAdded.Invoke(coll);
			coll.onCollected.AddListener(playerProgress.OnCollected);
			coll.SetCollectibleData(data);
		}

		return coll;
	}

	public void DropCollectible(CollectibleData data, Vector3 position)
	{
		var coll = GetCollectible(data);
		coll.phasesOut = data.phasing;
		coll.dropped = true;
		coll.transform.position = position;
		coll.BuildButton(collectibleContainer);
		coll.Mobilize(0);
	}

	public void DropCollectible(CollectibleData data, Vector3 position, bool phaseOverride)
	{
		var coll = GetCollectible(data);
		coll.phasesOut = phaseOverride;
		coll.dropped = true;
		coll.transform.position = position;
		coll.BuildButton(collectibleContainer);
		coll.Mobilize(0);
	}

	public void SetAllScrapActivity(bool active)
	{
		foreach (var item in scrapPool)
		{
			if (!item.gameObject.activeSelf)
			{
				continue;
			}
			item.animator.enabled = active;
			item.mover.moving = active;
			var collider = item.GetComponent<Collider2D>();
			if (collider)
			{
				collider.enabled = active;
			}
		}
	}

	public void SetAllCollectibleActivity(bool active)
	{
		foreach (var item in collectiblePool)
		{
			if (item.gameObject.activeSelf)
			{
				var anim = item.GetComponent<Animator>();
				if (anim)
				{
					anim.enabled = active;
				}
				item.mover.moving = active;
				var collider = item.GetComponent<Collider2D>();
				if (collider)
				{
					collider.enabled = active;
				}
			}
		}
	}

	public void OnEnemyAdded(EnemyView enemy)
	{
		enemy.onDestroy.AddListener(OnEntityDestroyed);
	}

	public void OnEntityDestroyed(EntityView view)
	{
		if (!allowDrops)
		{
			return;
		}
		if (view is EnemyView)
		{
			var enemy = view as EnemyView;
			if (allowScrapDrop)
			{
				DropScrap(enemy.enemyData.dropScrapAmount, true, enemy.transform.eulerAngles.z + 90, enemy.transform.position);
			}

			if (allowRareDrop && enemy.enemyData.dropData)
			{
				var drop = enemy.enemyData.dropData.GetDrop();
				if (drop)
				{
					DropCollectible(drop, enemy.transform.position);
				}
			}
		}
	}

}
