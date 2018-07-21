using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GizLib;

public class LevelDirector : MonoBehaviour
{
	public LevelDataSet levelDataSet;

	public PlayerProgressData playerProgress;

	public LevelData testLevel;

	public CollectibleSpawner collectibleSpawner;

	public EnemySpawner enemySpawner;

	public ShipController shipController;

	public ShipBladeController bladeController;

	public Background background;

	public ArenaHandler arenaHandler;

	public GameplayInterface gameplayInterface;

	public GameObject shipRotateTutorialPrefab;

	private GameObject rotateTutorial;

	public float levelDuration;

	LevelData currentLevel;

	[UnityEngine.Serialization.FormerlySerializedAs("winClip")]
	public AudioClip clipWin;

	public AudioClip clipLose;

	public Dictionary<ConditionType, TargetIntValue> conditionValues = new Dictionary<ConditionType, TargetIntValue>();

	public List<CollectibleType> collectibleConditions = new List<CollectibleType>();

	private Dictionary<string, string> currentConditionArguments = new Dictionary<string, string>();

	int actIndex = 0;

	private bool testing;

	private void Start()
	{
		StartLevelGameplay();
	}

	public bool DetermineCurrentLevel()
	{
		if (!levelDataSet || levelDataSet.dataSet.Count == 0)
		{
			return false;
		}

		if (testLevel != null)
		{
			currentLevel = testLevel;
			testing = true;
		}
		else if (SessionManager.Instance.levelData)
		{
			currentLevel = SessionManager.Instance.levelData;
		}
		else if (SessionManager.Instance.levelToLoad != -1)
		{
			currentLevel = levelDataSet.dataSet[SessionManager.Instance.levelToLoad];
		}
		else if (LocalDatabase.Instance.activeProfile != null)
		{
			var lvl = LocalDatabase.Instance.activeProfile.completedLevels.Count;
			if (lvl >= levelDataSet.dataSet.Count)
			{
				lvl = levelDataSet.dataSet.Count - 1;
			}
			currentLevel = levelDataSet.dataSet[lvl];
		}
		else
		{
			currentLevel = levelDataSet.dataSet[0];
		}


		return true;
	}

	public void BackgroundActivity(bool active)
	{
		background.enabled = active;
		collectibleSpawner.enabled = active;
	}

	public void StartLevelGameplay()
	{
		DetermineCurrentLevel();

		SetupPlayerProgress();

		if (currentLevel.levelType == "Tutorial")
		{
			SessionManager.Instance.gizmoidLoadout = currentLevel.defaultLoadout;
			ContinueGameplaySetup();
		}
		else
		{
			Time.timeScale = 0;

			gameplayInterface.BuildLoadoutPanel(ContinueGameplaySetup);
		}
	}

	public void ContinueGameplaySetup()
	{
		Time.timeScale = 1F;

		levelDuration = 0;
		foreach (Act act in currentLevel.actRoster)
		{
			if (act.wave != null)
			{
				levelDuration += act.wave.duration;
			}
		}

		collectibleSpawner.gameObject.SetActive(false);

		gameplayInterface.SetInteractive(false);

		if (bladeController && currentLevel.initialBladeCount > 0)
		{
			bladeController.onBladesPlaced.AddListener(OnInitialBladePlaced);
			bladeController.PlaceBlades(currentLevel.initialBladeCount);
		}
		else
		{
			OnInitialBladePlaced();
		}
	}

	private void SetupPlayerProgress()
	{
		if (playerProgress)
		{
			playerProgress.Reset();
			playerProgress.SetScraps(currentLevel.initialScraps);

			var profile = LocalDatabase.Instance.activeProfile;
			if (profile != null)
			{
				playerProgress.SetUnlockedFeatures(profile.featuresUnlocked);
				playerProgress.SetXeno(profile.xenos);
			}
		}
	}

	private void OnInitialBladePlaced()
	{
		bladeController.onBladesPlaced.RemoveListener(OnInitialBladePlaced);
		BuildLoadoutButtons();
	}

	private void BuildLoadoutButtons()
	{
		if (SessionManager.Instance.gizmoidLoadout.Count > 0)
		{
			gameplayInterface.entityButtonContainer.onButtonsPlaced.AddListener(OnLoadoutButtonsPlaced);
			gameplayInterface.entityButtonContainer.BuildUnitButtons(SessionManager.Instance.gizmoidLoadout);
		}
		else
		{
			OnLoadoutButtonsPlaced();
		}
	}

	private void OnLoadoutButtonsPlaced()
	{
		gameplayInterface.entityButtonContainer.onButtonsPlaced.RemoveListener(OnLoadoutButtonsPlaced);

		playerProgress.onFeatureChanged.AddListener(gameplayInterface.CheckUnlockedFeatures);
		gameplayInterface.CheckUnlockedFeatures(playerProgress.GetUnlockedFeatures());
		gameplayInterface.SetInteractive(true);
		EstablishCollectibleSpawner();
		BeginActRoster();
	}

	public void BeginActRoster()
	{
		Time.timeScale = 1F;

		if (currentLevel.actRoster.Count > 0)
		{
			actIndex = 0;
			ProcessAct(currentLevel.actRoster[actIndex]);
		}
	}

	public void EstablishCollectibleSpawner()
	{
		if (collectibleSpawner)
		{
			collectibleSpawner.gameObject.SetActive(true);
			collectibleSpawner.collectibleInterval = currentLevel.bigScrapInterval;
			collectibleSpawner.SpawningCollectibles = collectibleSpawner.collectibleInterval > 0;

			collectibleSpawner.scrapInterval = currentLevel.smallScrapInterval;
			collectibleSpawner.SpawningScrap = collectibleSpawner.scrapInterval > 0;
		}
	}

	public void IncrementAct()
	{
		actIndex++;
		if (actIndex < currentLevel.actRoster.Count)
		{
			ProcessAct(currentLevel.actRoster[actIndex]);
		}
		else
		{
			Debug.Log("End Level");
			gameplayInterface.SetInteractive(false);
			StartCoroutine(Wait(2, LevelCompleted));
		}
	}

	public void ProcessAct(Act act)
	{
		switch (act.actType)
		{
			case ActType.None:
				break;
			case ActType.Wave:
				//StartCoroutine(ProcessWave(act.wave));
				StartCoroutine(ProcessNewWave(act.wave));
				break;
			case ActType.Message:
				StartCoroutine(StartDialogue(act.dialog));
				break;
			case ActType.Action:
				ProcessAction(act.action);
				break;
			case ActType.Condition:
				ProcessCondition(act.condition);
				break;
			default:
				break;
		}
	}

	private IEnumerator StartDialogue(Dialog dialog)
	{
		gameplayInterface.SetInteractive(false);
		yield return new WaitForSeconds(1);
		var dp = gameplayInterface.GetDialoguePanel();
		dp.onComplete.AddListener(OnDialogueComplete);
		Time.timeScale = 0;
		dp.StartDialogue(dialog);
	}

	public void OnDialogueComplete()
	{
		gameplayInterface.SetInteractive(true);
		gameplayInterface.GetDialoguePanel().onComplete.RemoveListener(OnDialogueComplete);
		IncrementAct();
		if (gameplayInterface.toggleSpeed.isOn)
		{
			Time.timeScale = gameplayInterface.fastForwardSpeed;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	IEnumerator ProcessNewWave(Wave wave)
	{
		float spawnTime = wave.delay;
		float actProgress = 0;
		bool waving = true;
		int rosterIndex = 0;

		while (waving)
		{
			if (spawnTime >= wave.delay)
			{
				enemySpawner.SpawnEnemy(wave.waveCandidates[rosterIndex].PickRandomCandidate(), wave.waveCandidates[rosterIndex].spawnLane);
				rosterIndex++;
				if (rosterIndex >= wave.waveCandidates.Count)
				{
					if (!wave.timeDriven)
					{
						waving = false;
					}
					rosterIndex = 0;
				}
				spawnTime = 0;
			}
			else
			{
				spawnTime += Time.deltaTime;
				if (wave.timeDriven)
				{
					actProgress += Time.deltaTime;
					if (actProgress >= wave.duration)
					{
						waving = false;
					}
				}
			}

			if (!waving && wave.dropItem != null)
			{
				foreach (var item in enemySpawner.enemies)
				{
					item.onDestroy.AddListener(OnEnemyDrop);
				}
			}

			yield return null;
		}

		while (wave.waitForEnemies && enemySpawner.enemies.Count > 0)
		{
			yield return null;
		}

		IncrementAct();
	}

	private void AddEnemyDropEvent(EnemyView view)
	{
		view.onDestroy.AddListener(OnEnemyDrop);
	}

	public void OnEnemyDrop(EntityView view)
	{
		if (view is EnemyView)
		{
			var enemy = view as EnemyView;
			if (enemySpawner.enemies.Count == 0)
			{
				var collectible = currentLevel.actRoster[actIndex].wave.dropItem;
				if (collectible)
				{
					collectibleSpawner.DropCollectible(collectible, enemy.transform.position, false);
					enemySpawner.onSpawned.RemoveListener(AddEnemyDropEvent);
				}
			}
		}
	}

	public void ProcessAction(LevelAction levelAction)
	{
		bool canIncrement = true;
		switch (levelAction.actionType)
		{
			case ActionType.SpawnBigScrap:
				int spawnAmount = int.Parse(levelAction.argument);
				SpawnBigScrap(spawnAmount);
				break;
			case ActionType.AddGizmoidButton:
				canIncrement = false;
				var gizmoidId = int.Parse(levelAction.argument);
				AddGizmoidButtons(gizmoidId);
				break;
			case ActionType.SpawnSmallScrap:
				collectibleSpawner.SpawnScrap(int.Parse(levelAction.argument), false);
				break;
			case ActionType.AutoSpawnAll:
				try
				{
					var can = Convert.ToBoolean(levelAction.argument);
					collectibleSpawner.SpawningScrap = can;
					collectibleSpawner.SpawningCollectibles = can;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				break;
			case ActionType.AutoSpawnScrap:
				try
				{
					collectibleSpawner.SpawningScrap = Convert.ToBoolean(levelAction.argument);
				}
				catch (Exception)
				{
					throw;
				}
				break;
			case ActionType.AutoSpawnCollectible:
				try
				{
					collectibleSpawner.SpawningCollectibles = Convert.ToBoolean(levelAction.argument);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				break;
			case ActionType.AllowDropCollectible:
				try
				{
					collectibleSpawner.allowRareDrop = Convert.ToBoolean(levelAction.argument);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				break;
			case ActionType.AllowDropScrap:
				try
				{
					collectibleSpawner.allowScrapDrop = Convert.ToBoolean(levelAction.argument);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				break;
			case ActionType.RotateShip:
				float target = 0;
				float.TryParse(levelAction.argument, out target);
				shipController.ForceRotate(target, 0.5f, true);
				break;
			case ActionType.AddBlade:
				canIncrement = false;
				int bladeAmount = int.Parse(levelAction.argument);
				bladeController.onBladesPlaced.AddListener(OnBladePlaced);
				bladeController.PlaceBlades(bladeAmount);
				break;
			case ActionType.InsertGizmoid:
				var argumentStrings = levelAction.argument.Split(new string[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);
				int[] argumentInts = new int[argumentStrings.Length];
				for (int i = 0; i < argumentInts.Length; i++)
				{
					argumentInts[i] = int.Parse(argumentStrings[i]);
				}
				InjectGizmoid(argumentInts[0], argumentInts[2]);
				break;
			case ActionType.Unlock:
				playerProgress.AddFeatures(levelAction.argument);
				break;
			case ActionType.GizmoidButtonInteractive:
				bool active = true;
				try
				{
					active = Convert.ToBoolean(levelAction.argument);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				gameplayInterface.entityButtonContainer.SetInteractivity(active);
				break;
			case ActionType.None:
			default:
				break;
		}
		if (canIncrement)
		{
			IncrementAct();
		}
	}

	public void ProcessCondition(Condition condition)
	{
		currentConditionArguments = condition.GetAuguementsDictionary();

		if (!string.IsNullOrEmpty(condition.promptText))
		{
			gameplayInterface.ShowPromptLabel(condition.promptText);
			if (currentConditionArguments.ContainsKey("PromptAlignment"))
			{
				try
				{
					gameplayInterface.promptLabel.alignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), currentConditionArguments["PromptAlignment"], true);

				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			else
			{
				gameplayInterface.promptLabel.alignment = TextAnchor.UpperCenter;
			}
		}

		int count;
		switch (condition.conditionType)
		{
			case ConditionType.None:
				break;
			case ConditionType.RotateShip:
				RotateShipCondition(condition.argument, currentConditionArguments);
				break;
			case ConditionType.PickUpCollectible:
				var type = (CollectibleType)Enum.Parse(typeof(CollectibleType), condition.argument, true);
				PickupConditions(type);
				break;
			case ConditionType.PlaceGizmoid:
				PlaceGizmoids();
				break;
			case ConditionType.DestroyEnemy:
				count = int.Parse(condition.argument);
				conditionValues.Add(condition.conditionType, new TargetIntValue(count));
				AddDestroyEnemyListeners();
				break;
			case ConditionType.StockpileScrap:
				CollectScrapCondition(condition);
				break;
			case ConditionType.WaitForSeconds:
				StartCoroutine(Wait(float.Parse(condition.argument), ConditionCompleted));
				break;
			case ConditionType.DestroyGizmoid:
				AddDestroyGizmoidListeners();
				break;
			default:
				break;
		}
	}

	private void ConditionCompleted()
	{
		gameplayInterface.HidePromptLabel();
		IncrementAct();
	}

	public void ForceVictory()
	{
		if (currentLevel && currentLevel.actRoster.Count > 0)
		{
			for (int i = 0; i < currentLevel.actRoster.Count; i++)
			{
				if (currentLevel.actRoster[i].actType == ActType.Action)
				{
					if (currentLevel.actRoster[i].action != null && currentLevel.actRoster[i].action.actionType == ActionType.Unlock)
					{
						playerProgress.AddFeatures(currentLevel.actRoster[i].action.argument, false);
					}
				}
				if (currentLevel.actRoster[i].actType == ActType.Wave)
				{
					var lastWave = currentLevel.actRoster[i].wave;
					if (lastWave != null && lastWave.dropItem)
					{
						playerProgress.OnCollected(lastWave.dropItem);
					}
				}
			}
		}
		LevelCompleted();
	}

	public void LevelCompleted()
	{
		gameplayInterface.SetDisplay(false);

		AudioPlayer.Instance.musicSource.Pause();
		AudioPlayer.Instance.PlaySoundClip(clipWin, transform);

		Action action = null;

		if (playerProgress.unlockedGizmoids.Count > 0)
		{
			var gizData = (GizmoidData)gameplayInterface.entityButtonContainer.gizmoidRoster.unitDatas.Find(x => ((GizmoidData)x).typeId == playerProgress.unlockedGizmoids[0]);
			action = () => gameplayInterface.BuildUnitPreview(gizData, PostLevelAction().Invoke);
		}
		else
		{
			action = PostLevelAction();
		}

		StorePlayerProgress(true);
		Time.timeScale = 0;
		gameplayInterface.alertMessage.ShowMessage("SUCCESS!", "Congratulations, you completed Mission " + currentLevel.localId + "!", action);
	}

	private Action PostLevelAction()
	{
		var hasWorldMap = LocalDatabase.Instance.activeProfile.featuresUnlocked.Contains("worldmap") || playerProgress.GetUnlockedFeatures().Contains("worldmap");

		Action action = null;
		if (currentLevel.levelType == "Tutorial" && !hasWorldMap)
		{
			action = IncrementAndReloadLevel;
		}
		else
		{
			action = GotoWorldMap;
		}
		return action;
	}

	public void LevelFailed()
	{
		gameplayInterface.SetDisplay(false);
		if (gameObject.activeSelf)
		{
			StartCoroutine(Wait(1, Failure));
		}
		else
		{
			Failure();
		}
	}

	private void Failure()
	{
		StorePlayerProgress(false);

		Time.timeScale = 0;

		AudioPlayer.Instance.musicSource.Pause();
		AudioPlayer.Instance.PlaySoundClip(clipLose, transform);

		var hasWorldMap = LocalDatabase.Instance.activeProfile.featuresUnlocked.Contains("worldmap") || playerProgress.GetUnlockedFeatures().Contains("worldmap");


		Action action = null;
		if (currentLevel.levelType == "Tutorial" && !hasWorldMap)
		{
			action = ReloadLevel;
		}
		else
		{
			action = GotoWorldMap;
		}
		gameplayInterface.alertMessage.ShowMessage("Mission Failed!", "Oh No! Better luck next time. Please try again.", action);

	}

	public void GotoWorldMap()
	{
		SceneManager.LoadScene("WorldMapNew");
	}

	public void ReloadLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void IncrementAndReloadLevel()
	{
		if (testing && testLevel)
		{
			SessionManager.Instance.levelData = testLevel;
		}
		else
		{
			if (levelDataSet.dataSet.Contains(currentLevel))
			{
				var index = levelDataSet.dataSet.IndexOf(currentLevel);
				if (index + 1 < levelDataSet.dataSet.Count)
				{
					SessionManager.Instance.levelData = levelDataSet.dataSet[index + 1];
					SessionManager.Instance.levelToLoad = index + 1;
				}
			}
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StorePlayerProgress(bool victory)
	{
		var profile = LocalDatabase.Instance.activeProfile;
		if (profile != null)
		{
			profile.xenos = playerProgress.Xenos;
			if (victory)
			{
				if (!profile.completedLevels.Exists(x => x.localId == currentLevel.localId && x.worldId == currentLevel.worldId))
				{
					profile.completedLevels.Add(new LevelResult(currentLevel.worldId, currentLevel.localId));
				}
			}

			foreach (var item in playerProgress.GetUnlockedFeatures())
			{
				if (!profile.featuresUnlocked.Contains(item))
				{
					profile.featuresUnlocked.Add(item);
				}
			}

			for (int i = 0; i < playerProgress.unlockedGizmoids.Count; i++)
			{
				if (!profile.gizmoids.Contains(playerProgress.unlockedGizmoids[i]))
				{
					profile.gizmoids.Add(playerProgress.unlockedGizmoids[i]);
				}
			}
			LocalDatabase.Instance.CheckInProfile(profile);
		}

		playerProgress.Reset();
	}

	private IEnumerator Wait(float duration, Action callback)
	{
		yield return new WaitForSeconds(duration);
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	private void OnBladePlaced()
	{
		bladeController.onBladesPlaced.RemoveListener(OnBladePlaced);
		IncrementAct();
	}

	private void CollectScrapCondition(Condition condition)
	{
		int amount = 5;
		if (currentConditionArguments.ContainsKey("Amount"))
		{
			int.TryParse(currentConditionArguments["Amount"], out amount);
		}
		else
		{
			int.TryParse(condition.argument, out amount);
		}

		if (currentConditionArguments.ContainsKey("Exact"))
		{
			if (playerProgress.Scraps < amount)
			{
				conditionValues.Add(condition.conditionType, new TargetIntValue(amount));
				playerProgress.onScrapChanged.AddListener(OnScrapValueChanged);
			}
			else
			{
				ConditionCompleted();
			}
		}
		else
		{
			conditionValues.Add(condition.conditionType, new TargetIntValue(amount + playerProgress.Scraps));
			playerProgress.onScrapChanged.AddListener(OnScrapValueChanged);
		}
	}

	private void OnScrapValueChanged(int value)
	{
		if (conditionValues.ContainsKey(ConditionType.StockpileScrap) && value >= conditionValues[ConditionType.StockpileScrap].targetValue)
		{
			playerProgress.onScrapChanged.RemoveListener(OnScrapValueChanged);
			Debug.Log("Scrap Target Reached!");

			ConditionCompleted();
		}
	}

	#region DestroyGizmoid
	private void AddDestroyGizmoidListeners()
	{
		foreach (var blade in bladeController.blades)
		{
			foreach (var giz in blade.gizmoidHolder.gizmoids)
			{
				giz.onDestroy.AddListener(OnGizDestroy);
			}
		}
	}

	private void RemoveDestroyGizmoidListeners()
	{
		foreach (var blade in bladeController.blades)
		{
			foreach (var giz in blade.gizmoidHolder.gizmoids)
			{
				giz.onDestroy.RemoveListener(OnGizDestroy);
			}
		}
	}

	private void OnGizDestroy(EntityView view)
	{
		ConditionCompleted();
		RemoveDestroyGizmoidListeners();
	}
	#endregion

	#region DestroyEnemy
	private void AddDestroyEnemyListeners()
	{
		enemySpawner.onSpawned.AddListener(OnEnemySpawned);
		foreach (var item in enemySpawner.enemies)
		{
			item.onDestroy.AddListener(OnEnemyDestroyed);
		}
	}
	private void RemoveDestroyEnemyListeners()
	{
		enemySpawner.onSpawned.RemoveListener(OnEnemySpawned);
		foreach (var item in enemySpawner.enemies)
		{
			item.onDestroy.RemoveListener(OnEnemyDestroyed);
		}
	}

	private void OnEnemySpawned(EnemyView view)
	{
		view.onDestroy.AddListener(OnEnemyDestroyed);
	}

	private void OnEnemyDestroyed(EntityView view)
	{
		if (view is EnemyView)
		{
			view.onDestroy.RemoveListener(OnEnemyDestroyed);
			if (conditionValues.ContainsKey(ConditionType.DestroyEnemy))
			{
				conditionValues[ConditionType.DestroyEnemy].value++;
				if (conditionValues[ConditionType.DestroyEnemy].value >= conditionValues[ConditionType.DestroyEnemy].targetValue)
				{
					RemoveDestroyEnemyListeners();
					conditionValues.Remove(ConditionType.DestroyEnemy);
					Debug.Log("Destroyed Enemy Target Reached");

					ConditionCompleted();

				}
			}
		}
	}
	#endregion

	#region PlaceGizmoid
	public void PlaceGizmoids()
	{
		if (currentConditionArguments.ContainsKey("Tutorial"))
		{
			var hand = gameplayInterface.GetHandIndicator(true);
			var start = Vector2.zero;
			if (gameplayInterface.entityButtonContainer.unitButtons.Count > 0)
			{
				start = (gameplayInterface.entityButtonContainer.unitButtons[0].transform as RectTransform).anchoredPosition;
			}
			var end = Vector2.zero;
			if (bladeController.blades.Count > 0)
			{
				hand.SetupMotion(start, bladeController.blades[0].transform, 1.5f, true, 3);
			}
		}

		foreach (var item in bladeController.blades)
		{
			item.gizmoidHolder.onGizmoidAdded.AddListener(OnGizmoidAdded);
		}
		foreach (var item in gameplayInterface.entityButtonContainer.unitButtons)
		{
			item.toggleDrag.onDragEnabled.AddListener(TogglePromptOnDrag);
			item.toggleDrag.onDragEnd.AddListener(ShowPromptOnDragEnd);
		}
	}

	private void ShowPromptOnDragEnd(PointerEventData pointerEventData)
	{
		gameplayInterface.ShowPromptLabel();
		var hand = gameplayInterface.GetHandIndicator(false);
		if (hand)
		{
			hand.gameObject.SetActive(true);
		}

	}

	private void TogglePromptOnDrag(bool acive, PointerEventData pointerEventData)
	{
		var hand = gameplayInterface.GetHandIndicator(false);
		if (hand)
		{
			hand.gameObject.SetActive(!acive);
		}

		if (acive)
		{
			gameplayInterface.HidePromptLabel();
		}
		else
		{
			gameplayInterface.ShowPromptLabel();
		}
	}

	private void OnGizmoidAdded(GizmoidView gizmoid)
	{
		foreach (var item in bladeController.blades)
		{
			item.gizmoidHolder.onGizmoidAdded.RemoveListener(OnGizmoidAdded);
		}
		foreach (var item in gameplayInterface.entityButtonContainer.unitButtons)
		{
			item.toggleDrag.onDragEnabled.RemoveListener(TogglePromptOnDrag);
			item.toggleDrag.onDragEnd.RemoveListener(ShowPromptOnDragEnd);

		}
		Debug.Log("Gizmoid " + gizmoid.name + " added!");

		var hand = gameplayInterface.GetHandIndicator(false);
		if (hand)
		{
			hand.gameObject.SetActive(false);
		}

		ConditionCompleted();
	}
	#endregion

	#region PickUpCollectible

	private void PickupConditions(CollectibleType collectibleType)
	{
		if (!collectibleConditions.Contains(collectibleType))
		{
			collectibleConditions.Add(collectibleType);
		}
		collectibleSpawner.onCollectibleAdded.AddListener(OnCollectibleAdded);
		var collectibles = collectibleSpawner.collectiblePool.FindAll(x => x.collectibleData.collectibleType == collectibleType);
		foreach (var item in collectibles)
		{
			item.onCollected.AddListener(OnCollected);
		}
	}

	private void OnCollectibleAdded(CollectibleInteractive collectible)
	{
		collectible.onCollected.AddListener(OnCollected);
	}

	private void RemovePickupConditions(CollectibleType collectibleType)
	{
		if (collectibleConditions.Contains(collectibleType))
		{
			collectibleConditions.Remove(collectibleType);
		}
		collectibleSpawner.onCollectibleAdded.RemoveListener(OnCollectibleAdded);
		var collectibles = collectibleSpawner.collectiblePool.FindAll(x => x.collectibleData.collectibleType == collectibleType);
		foreach (var item in collectibles)
		{
			item.onCollected.RemoveListener(OnCollected);
		}
	}

	public void OnCollected(CollectibleData data)
	{
		if (collectibleConditions.Contains(data.collectibleType))
		{
			RemovePickupConditions(data.collectibleType);
			Debug.Log(data.collectibleType.ToString() + " Collected!");

			ConditionCompleted();
		}
	}
	#endregion

	#region RotateShipCondition
	public void RotateShipCondition(string arg, Dictionary<string, string> args)
	{
		float target = 180f;

		if (args.ContainsKey("TargetRotation"))
		{
			float.TryParse(args["TargetRotation"], out target);
		}
		else
		{
			float.TryParse(arg, out target);
		}

		bool showTutorial = false;
		if (args.ContainsKey("Tutorial"))
		{
			try
			{
				showTutorial = Convert.ToBoolean(args["Tutorial"]);
			}
			catch (Exception)
			{

				throw;
			}
		}

		if (showTutorial)
		{
			if (!rotateTutorial)
			{
				rotateTutorial = Instantiate(shipRotateTutorialPrefab);
			}
			rotateTutorial.gameObject.SetActive(true);
		}


		arenaHandler.GetSectorByNearestRotation(target).StartPhaseSector();
		shipController.onEndRotate.AddListener(OnShipSnap);
		gameplayInterface.spaceshipInterface.onBeginDrag.AddListener(OnShipRotating);
	}

	public void OnShipRotating(PointerEventData eventData)
	{
		gameplayInterface.HidePromptLabel();

		if (rotateTutorial)
		{
			rotateTutorial.SetActive(false);
		}
	}

	public void OnShipSnap(float rotation)
	{

		var target = 180f;
		if (currentConditionArguments.ContainsKey("TargetRotation"))
		{
			float.TryParse(currentConditionArguments["TargetRotation"], out target);
		}
		else
		{
			float.TryParse(currentLevel.actRoster[actIndex].condition.argument, out target);
		}
		if (rotation == target)
		{
			arenaHandler.GetSectorByNearestRotation(target).ClearPhaseSector();
			shipController.onEndRotate.RemoveListener(OnShipSnap);
			gameplayInterface.spaceshipInterface.onBeginDrag.RemoveListener(OnShipRotating);

			if (rotateTutorial)
			{
				rotateTutorial.SetActive(false);
			}

			ConditionCompleted();
		}
		else
		{
			if (rotateTutorial)
			{
				rotateTutorial.SetActive(true);
			}

			gameplayInterface.ShowPromptLabel();
		}
	}
	#endregion

	public void Spawning(string argument, bool active)
	{
		switch (argument)
		{
			case "all scrap":
				collectibleSpawner.SpawningCollectibles = active;
				collectibleSpawner.SpawningScrap = active;
				break;
			case "big scrap":
				collectibleSpawner.SpawningCollectibles = active;
				break;
			case "small scrap":
				collectibleSpawner.SpawningScrap = active;
				break;
			default:
				Debug.LogError("Invalid argument for start spawning: \"" + argument + "\"");
				break;
		}
	}

	public void AddGizmoidButtons(params int[] indexes)
	{
		var gizies = new List<GizmoidData>();
		foreach (var item in indexes)
		{
			var giz = (GizmoidData)gameplayInterface.entityButtonContainer.gizmoidRoster.unitDatas.Find(x => ((GizmoidData)x).typeId == item);
			if (giz)
			{
				gizies.Add(giz);
			}
		}
		gameplayInterface.entityButtonContainer.onButtonsPlaced.AddListener(OnGizmoidButtonsInjected);
		gameplayInterface.entityButtonContainer.BuildUnitButtons(gizies);
	}

	private void OnGizmoidButtonsInjected()
	{
		gameplayInterface.entityButtonContainer.onButtonsPlaced.RemoveListener(OnGizmoidButtonsInjected);
		IncrementAct();
	}

	public void SpawnBigScrap(int amount)
	{
		if (amount > 0)
		{
			for (int i = 0; i < amount; i++)
			{
				collectibleSpawner.SpawnBigScrap(false);
			}
		}
		else if (amount == -1)
		{
			collectibleSpawner.SpawnBigScrap(false);
		}
		else
		{
			Debug.LogError("Invalid spawn amount: " + amount);
		}
	}

	public void InjectGizmoid(int gizmoidIndex, int damage = 0)
	{
		var bladeIndex = UnityEngine.Random.Range(0, bladeController.blades.Count);
		var gizData = (GizmoidData)gameplayInterface.entityButtonContainer.gizmoidRoster.unitDatas.Find(x => ((GizmoidData)x).typeId == gizmoidIndex);

		var giz = bladeController.blades[bladeIndex].gizmoidHolder.BuildGizmoid(gizData);
		giz.health.TakeDamage(giz.health.maxHitpoints - damage);
	}
}
