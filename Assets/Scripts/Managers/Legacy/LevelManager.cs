using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GizLib;

public class LevelManager : MonoBehaviour
{
	//Static
	public static LevelManager Instance;

	//Configurable
	public float endLevelDelay = 2F; //Delay in seconds before end level dialog
	public float conditionRotationLength = 2F;
	//public List<int> defaultGizmoidLoadout = new List<int>() {0, 1, 2};
	public float fadeConstant = 10f;
	public float gracePeriod = 1f;

	//Local
	private int currentActIndex = 0;
	private float actProgress = 0;
	private bool actIsInitialized = false;
	private bool actTimerIsRunning = false;
	public LevelData currentLevel;
	private float enemyTimer = 0;
	private int enemyRosterIndex = 0;
	delegate void ActHandler();
	//public Loadout loadout;
	//public List<BurnCard> chosenBurnCards = new List<BurnCard>();

	//Status
	public Profile gameProfile;
	public float levelProgress = 0;
	public float levelDuration;
	public bool levelHasStarted = false;
	public bool hostileActHasBegun = false;
	public float hostileActStartTime;
	public bool allEnemiesHaveSpawned = false;
	public int totalEnemiesSpawned = 0;
	public int totalEnemiesDestroyed = 0;
	public bool levelIsSuspended = false;
	public int itemsCollected = 0;
	public int xenoCollected = 0;
	public int gizmoidsAdded = 0;
	public int enemiesDestroyedInAct = 0;
	public bool doneSpawningEnemies = false;
	public int totalScrapCollected = 0;
	public int blueprintsCollected = 0;
	public int gizmoidsTrashed = 0;

	//Inspector
	public AudioClip winClip;
	//public AudioClip loseClip;

	public LevelDataSet levelDataSet;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		//Time.timeScale = 0F;
		//currentLevel = Temp.Instance.level0;
		InitializeLevel();
	}

	// Update is called once per frame
	void Update()
	{

		if (!levelIsSuspended)
		{
			ActHandler actHandler = null;
			switch (currentLevel.actRoster[currentActIndex].actType)
			{
				case ActType.None:

					Debug.Log("Empty act: " + currentActIndex);
					IncrementAct();
					break;
				case ActType.Action:
					actHandler = HandleAction;
					break;
				case ActType.Condition:
					actHandler = HandleCondition;
					break;
				case ActType.Message:
					//Debug.Log ("HandleMessage @"+Time.frameCount);
					actHandler = HandleMessage;
					break;
				case ActType.Wave:
					//Debug.Log ("HandleWave @"+Time.frameCount);
					actHandler = HandleWave;
					break;
				default:
					IncrementAct();
					Debug.Log("Invalid act type: " + currentLevel.actRoster[currentActIndex].actType);
					break;
			}

			if (actHandler != null)
			{
				//Debug.Log("Act handler is go.");
				actHandler();
			}

			//						} else {
			//								Debug.LogError ("Invalid act type: " + currentAct.actType);
			//						}
		}
	}

	int GetRandomEnemy()
	{
		//float frequencySum = 0;
		//Dictionary<float, int> probabilityQueue = new Dictionary<float, int>(); //Probability, enemyId
		//List<int> possibleEnemies = currentLevel.actRoster[currentActIndex].wave.possibleEnemies;

		////foreach(Enemy enemy in currentLevel.possibleEnemyTypes)
		////for (int i = 0; i < possibleEnemies.Count; i++)
		////{
		////	Enemy enemy = GameData.Instance.enemyData[possibleEnemies[i]];
		////	if (enemy.frequency > 0)
		////	{
		////		//Debug.Log ("Name, frequency: " + enemy.properName + ", " + enemy.frequency);
		////		probabilityQueue.Add(enemy.frequency + frequencySum, GameData.Instance.enemyData.IndexOf(enemy));
		////		frequencySum += enemy.frequency;
		////	}
		////}

		////Generate a random number between 0 and frequency sum
		//float randomNumber = UnityEngine.Random.value * frequencySum;

		//foreach (KeyValuePair<float, int> entry in probabilityQueue)
		//{
		//	if (randomNumber <= entry.Key)
		//	{
		//		//Debug.Log ("Frequency sum: " + frequencySum + ", random number: " + randomNumber + ", enemy value: " + entry.Key + ", enemy: " + entry.Value.properName);
		//		return entry.Value;
		//	}
		//}

		//Debug.LogError("Null return on GetRandomEnemy");
		return -1;

	}

	IEnumerator EndLevel()
	{
		levelIsSuspended = true;
		actTimerIsRunning = false;
		allEnemiesHaveSpawned = true;
		yield return new WaitForSeconds(endLevelDelay);
		Time.timeScale = 0F;


		//		LevelResult[] currentLevelResults = (from levelResult in gameProfile.completedLevels
		//			where levelResult.worldId == currentLevel.worldId && levelResult.localId == currentLevel.localId
		//				select levelResult).ToArray();
		//		if(currentLevelResults.Length == 0)
		//		{
		//
		//		}

		//Update game profile
		//		if(currentLevel.localId == 3) //Tutorial
		//		{
		//			gameProfile.gameProgress.Add (GameProgression.WorldMap);
		//		}
		//Log level as completed
		LevelResult alreadyCompletedLevel = gameProfile.completedLevels.SingleOrDefault(level => level.localId == currentLevel.localId && level.worldId == currentLevel.worldId);
		if (alreadyCompletedLevel != null)
		{

		}
		else
		{
			LevelResult levelResult = new LevelResult(currentLevel.worldId, currentLevel.localId);
			//levelResult.hasBeenCompleted = true;
			gameProfile.completedLevels.Add(levelResult);
		}


		//Close out game profile
		LocalDatabase.Instance.activeProfile = new Profile(gameProfile);
		LocalDatabase.Instance.CheckInProfile(LocalDatabase.Instance.activeProfile);




		//Assign next state 
		//TODO Debrief
		//ButtonAction buttonAction = ButtonAction.None;
		//if (gameProfile.gameProgress.Contains(GameProgression.WorldMap))
		//{
		//	buttonAction = ButtonAction.GoToWorldMap;
		//}
		//else
		//{
		//	//SessionManager.Instance.levelToLoad = SessionManager.GetIndexFromLevelIds(currentLevel.worldId, currentLevel.localId + 1);
		//	buttonAction = ButtonAction.NextLevel;
		//	//Level nextLevel = GameData.Instance.levelData.Single (level => level.localId == currentLevel.localId + 1 && level.worldId == currentLevel.worldId);
		//	//SessionManager.Instance.levelToLoad = GameData.Instance.levelData.IndexOf(nextLevel);
		//}
		AudioPlayer.Instance.musicSource.Pause();
		AudioPlayer.Instance.PlaySoundClip(winClip, transform);
		//InterfaceController.Instance.OpenAlert("Level Complete!", buttonAction);

		gameProfile = null;
	}

	public void InitializeLevel()
	{
		GetCurrentLevel();

		//Copy user profile
		gameProfile = new Profile(LocalDatabase.Instance.activeProfile);

		//.instance.SetCurrentBackground(currentLevel.worldId);

		//Initialize game UI
		//InterfaceController.Instance.InitializeGameInterface();

		//if (currentLevel.levelType == "Tutorial")
		//{
		//	HandleGizmoidButtons(currentLevel.defaultLoadout);
		//	StartLevelGameplay();
		//	//InterfaceController.Instance.InitializeGameInterface();
		//}
		//else if (gameProfile.gameProgress.Contains(GameProgression.Loadout) && !Debugger.Instance.useSandbox)
		//{
		//	InsertLoadoutScreen();
		//}
		//else
		//{
		//	//Create gizmoid buttons
		//	//if (Debugger.Instance.loadoutScreen)
		//	//{
		//	//	InsertLoadoutScreen();
		//	//}
		//	//else
		//	//{
		//	//	HandleGizmoidButtons(gameProfile.gizmoids);
		//	//	StartLevelGameplay();
		//	//	//InterfaceController.Instance.InitializeGameInterface();

		//	//}

		//}

	}

	public void InsertLoadoutScreen()
	{
		levelIsSuspended = true;

		//var l = Instantiate(loadout) as Loadout;
		//l.transform.parent = InterfaceController.Instance.panelPopup.transform;
		//l.transform.localPosition = Vector3.zero;
		//l.transform.localScale = Vector3.one;
		//InterfaceController.Instance.DisplayForegroundInterface(false);

	}

	public void GetCurrentLevel()
	{
		//if (Debugger.Instance != null && Debugger.Instance.useSandbox)
		//{
		//	//currentLevel = Debugger.Instance.sandbox;
		//	//levelIndex = -1;
		//}
		//else 
		if (SessionManager.Instance.levelToLoad != -1)
		{
			currentLevel = levelDataSet.dataSet[SessionManager.Instance.levelToLoad];
			//currentLevel = GameData.Instance.levelData[SessionManager.Instance.levelToLoad];
		}
		else
		{
			//Determine next level from user data
			Debug.Log(LocalDatabase.Instance.activeProfile.name);
			List<LevelResult> lastCompletedLevelQuery = //((
				(from levelResult in LocalDatabase.Instance.activeProfile.completedLevels
					 //	 where levelResult.hasBeenCompleted == true
				 select levelResult).ToList();
			//                .OrderByDescending(levelResult => levelResult.worldId).ThenByDescending(levelResult => levelResult.localId)).ToList();
			//	objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
			int nextWorldId = -1;
			int nextLocalId = -1;
			if (lastCompletedLevelQuery.Count < 1)
			{
				nextWorldId = 1;
				nextLocalId = 1;
			}
			else if (lastCompletedLevelQuery.Count >= 10)
			{
				nextLocalId = 1;
				nextWorldId = 1;
			}
			else
			{
				// LevelResult lastCompletedLevel = lastCompletedLevelQuery[0];
				nextLocalId = lastCompletedLevelQuery.Count + 1;
				nextWorldId = 1;


				//                if (lastCompletedLevel.localId < 10)
				//                {
				//                    nextLocalId = lastCompletedLevel.localId + 1;
				//                    nextWorldId = lastCompletedLevel.worldId;
				//                }
				//                else
				//                {
				//                    nextLocalId = 1;
				//                    nextWorldId = lastCompletedLevel.worldId + 1;
				//                }
				//                Debug.Log("Last level: " + lastCompletedLevel.worldId + "-" + lastCompletedLevel.localId);
			}
			Debug.Log("Next level: " + nextWorldId + "-" + nextLocalId);
			var nextLevelQuery = (from level in levelDataSet.dataSet
								  where level.worldId == nextWorldId && level.localId == nextLocalId
								  select level).ToArray();
			if (nextLevelQuery.Length == 1)
			{
				currentLevel = nextLevelQuery[0];
			}
			else
			{
				Debug.LogError("Invalid number of matching levels.");
			}
		}
	}

	public void HandleGizmoidButtons(List<int> list)
	{
		//if (Debugger.Instance.useSandbox && !Debugger.Instance.loadoutScreen)
		//{
		//	GameController.Instance.gizmoidLoadout = Debugger.Instance.sandboxLoadout;
		//}
		//else
		//{
		//	GameController.Instance.gizmoidLoadout = list;
		//}
		//InterfaceController.Instance.CreateGizmoidButtons();
	}

	public void StartLevelGameplay()
	{

		levelIsSuspended = false;

		//Set timing
		levelDuration = 0;
		foreach (Act act in currentLevel.actRoster)
		{
			if (act.wave != null)
			{
				levelDuration += act.wave.duration;
			}
		}

		//Give scrap
		//PlayerResources.Instance.SetScrap(currentLevel.initialScraps);

		//Spawn blades
		//ShipBladeController.Instance.PlaceBlades(currentLevel.initialBladeCount);

		//Incorporate mutations
		//foreach (BurnCard burnCard in chosenBurnCards)
		//{
		//	//Switch between burn card types
		//}

		//Start spawning scrap
		//if (currentLevel.bigScrapInterval > 0)
		//{
		//	CollectibleSpawner.Instance.SpawningCollectibles = true;
		//}
		//if (currentLevel.smallScrapInterval > 0)
		//{
		//	CollectibleSpawner.Instance.SpawningScrap = true;
		//}

		//Mark as started
		levelHasStarted = true;
		if (currentLevel.actRoster[currentActIndex].actType == ActType.Wave)
		{
			//StartActTimer ();
			hostileActHasBegun = true;
			hostileActStartTime = 0F;
		}

		//Start time
		Time.timeScale = 1F;

		//Play game track
		//AudioPlayer.Instance.PlayBGM(MusicCue.GameplayWorld1);
	}

	public void IncrementAct()
	{
		Act previousAct = currentLevel.actRoster[currentActIndex];
		currentActIndex++;
		if (currentActIndex < currentLevel.actRoster.Count)
		{

			//Debug.Log ("Incrementing act");

			actProgress = 0;
			enemyTimer = 0;
			enemyRosterIndex = 0;
			actIsInitialized = false;
			enemiesDestroyedInAct = 0;
			//if(previousAct.useGracePeriod)
			//{
			if (previousAct.actType == ActType.Wave)
			{
				Debug.Log("Act is suspended");
				levelIsSuspended = true;
				StartCoroutine("WaitForGracePeriod");
			}

		}
		else
		{
			//End level
			StartCoroutine(EndLevel());
		}
	}

	IEnumerator WaitForGracePeriod()
	{
		Debug.Log("Waiting... Act is initialized: " + actIsInitialized);
		yield return new WaitForSeconds(gracePeriod);
		Debug.Log("Done waiting. Act is initialized: " + actIsInitialized);
		levelIsSuspended = false;
	}

	void HandleMessage()
	{
		//Debug.Log ("Handle message. Initialized? "+actIsInitialized);
		//Act currentAct = currentLevel.actRoster [currentActIndex];
		if (!actIsInitialized)
		{
			Debug.Log("Initiate dialog for act" + currentLevel.actRoster[currentActIndex] + " @" + Time.frameCount);
			//DialogManager.Instance.InitiateDialog(currentLevel.actRoster[currentActIndex].dialog);
		}

		actIsInitialized = true;
	}

	void HandleWave()
	{
		//Act currentAct = currentLevel.actRoster[currentActIndex];

		//if (!actIsInitialized)
		//{
		//	enemyTimer = currentAct.wave.delay;

		//	actTimerIsRunning = true;
		//	doneSpawningEnemies = false;
		//	actIsInitialized = true;
		//}

		//if (actTimerIsRunning)
		//{
		//	levelProgress += Time.deltaTime;

		//	if (currentAct.wave.possibleEnemies.Count >= 1)
		//	{
		//		//If act duration has been reached
		//		if (actProgress >= currentAct.wave.duration)
		//		{
		//			if (!currentAct.wave.waitForEnemies || EnemySpawner.Instance.transform.childCount == 0)
		//			{
		//				IncrementAct();
		//			}
		//		}
		//		else
		//		{
		//			//If act duration has not been reached
		//			actProgress += Time.deltaTime;
		//			if (enemyTimer >= currentAct.wave.delay)
		//			{
		//				enemyTimer = 0;


		//				EnemySpawner.Instance.SpawnEnemy(GetRandomEnemy(), -1);
		//				if (currentAct.wave.delay > currentAct.wave.duration - actProgress)
		//				{
		//					doneSpawningEnemies = true;
		//					if (currentAct.wave.dropItem.collectibleType != CollectibleType.None)
		//					{
		//						StartCoroutine(GiveLastEnemyCollectible());
		//					}
		//				}
		//			}
		//			else
		//			{
		//				enemyTimer += Time.deltaTime;
		//			}
		//		}
		//	}
		//	else if (currentAct.wave.enemyRoster.Count >= 1)
		//	{
		//		if (enemyRosterIndex < currentAct.wave.enemyRoster.Count)
		//		{
		//			//if (enemyTimer >= currentAct.wave.delay)
		//			//{
		//			//	Debug.Log("Spawning enemy index: " + enemyRosterIndex + " @" + Time.frameCount);
		//			//	enemyTimer = 0;

		//			//	EnemySpawner.Instance.SpawnEnemy(currentAct.wave.enemyRoster[enemyRosterIndex], currentAct.wave.spawnLane);

		//			//	if (enemyRosterIndex == currentAct.wave.enemyRoster.Count - 1)
		//			//	{
		//			//		Debug.Log("Last enemy identified @" + Time.frameCount);
		//			//		doneSpawningEnemies = true;
		//			//		if (currentAct.wave.dropItem.collectibleType != CollectibleType.None)
		//			//		{
		//			//			Debug.Log("Starting give last enemy @" + Time.frameCount);
		//			//			StartCoroutine(GiveLastEnemyCollectible());
		//			//		}
		//			//	}
		//			//	enemyRosterIndex++;

		//			//}
		//			//else
		//			//{
		//			//	enemyTimer += Time.deltaTime;
		//			//}
		//		}
		//		else
		//		{
		//			if (!currentAct.wave.waitForEnemies || EnemySpawner.Instance.transform.childCount == 0)
		//			{
		//				Debug.Log("Explicit increment");
		//				IncrementAct();
		//			}
		//		}
		//	}
		//	else
		//	{
		//		Debug.LogError("Enemies not specified for wave");
		//	}

		//}

	}

	IEnumerator GiveLastEnemyCollectible()
	{
		Debug.Log("Started give last enemy @" + Time.frameCount);
		Act currentAct = currentLevel.actRoster[currentActIndex];
		//Collectible collectible = new Collectible();
		//collectible = currentAct.wave.itemDrop;
		//print("collectible values" + collectible.value);
		while (EnemySpawner.Instance.transform.childCount > 1)
		{
			Debug.Log("Running give last enemy loop @" + Time.frameCount);

			yield return null;
		}
		if (EnemySpawner.Instance.transform.childCount == 1)
		{
			Debug.Log("Adding drop. @" + Time.frameCount);
			EnemyView enemySheet = EnemySpawner.Instance.GetComponentInChildren<EnemyView>();
			//enemySheet.AddDrop(currentAct.wave.dropItem);
		}
		else if (EnemySpawner.Instance.transform.childCount == 0)
		{
			Debug.LogError("No enemies to which to give collectible.");
		}
		yield return null;
	}

	void HandleAction()
	{

		Act currentAct = currentLevel.actRoster[currentActIndex];
		switch (currentAct.action.actionType)
		{
			case ActionType.SpawnBigScrap:
				int spawnAmount = int.Parse(currentAct.action.argument);
				if (spawnAmount > 0)
				{

					for (int i = 0; i < spawnAmount; i++)
					{
						CollectibleSpawner.Instance.SpawnBigScrap(false);
					}
				}
				else if (spawnAmount == -1)
				{
					CollectibleSpawner.Instance.SpawnBigScrap(false);
				}
				else
				{
					Debug.LogError("Invalid spawn amount: " + spawnAmount);
				}
				break;
			case ActionType.AddGizmoidButton:
				int gizmoidId = int.Parse(currentAct.action.argument); //Argument is gizmoid id
				if (!gameProfile.gizmoids.Contains(gizmoidId))
				{
					gameProfile.gizmoids.Add(gizmoidId);
				}
				//InterfaceController.Instance.CreateGizmoidButton(gizmoidId);
				//InterfaceController.Instance.gizmoidButtonsScripts[InterfaceController.Instance.gizmoidButtonsScripts.Count - 1].startCooled = false;

				break;
			//case ActionType.StartSpawning:

			//	switch (currentAct.action.argument)
			//	{
			//		case "all scrap":
			//			CollectibleSpawner.Instance.SpawningCollectibles = true;
			//			CollectibleSpawner.Instance.SpawningScrap = true;
			//			break;
			//		case "big scrap":
			//			CollectibleSpawner.Instance.SpawningCollectibles = true;
			//			break;
			//		case "small scrap":
			//			CollectibleSpawner.Instance.SpawningScrap = true;
			//			break;
			//		default:
			//			Debug.LogError("Invalid argument for start spawning: \"" + currentAct.action.argument + "\"");
			//			break;
			//	}
			//	break;
			//case ActionType.StopSpawning:
			//	switch (currentAct.action.argument)
			//	{
			//		case "all scrap":
			//			CollectibleSpawner.Instance.SpawningCollectibles = false;
			//			CollectibleSpawner.Instance.SpawningScrap = false;
			//			break;
			//		case "big scrap":
			//			CollectibleSpawner.Instance.SpawningCollectibles = false;
			//			break;
			//		case "small scrap":
			//			CollectibleSpawner.Instance.SpawningScrap = false;
			//			break;
			//		default:
			//			Debug.LogError("Invalid argument for start spawning: \"" + currentAct.action.argument + "\"");
			//			break;
			//	}
			//	break;
			case ActionType.SpawnSmallScrap:
				CollectibleSpawner.Instance.SpawnScrap(int.Parse(currentAct.action.argument), false);
				break;
			//case ActionType.StartEnemyScrapDrop:
			//	//EnemySpawner.Instance.allowEnemyScrapDrop = true;
			//	break;
			//case ActionType.StopEnemyScrapDrop:
			//	//EnemySpawner.Instance.allowEnemyScrapDrop = false;
			//	break;
			case ActionType.InsertGizmoid:
				string[] argumentStrings = Regex.Split(currentAct.action.argument, ";");
				int[] argumentInts = new int[3];
				for (int i = 0; i < argumentInts.Length; i++)
				{
					argumentInts[i] = int.Parse(argumentStrings[i]);
				}
				//TODO Fix!
				//BladeView targetBlade = PlayerBladeHandler.Instance.blades.Single(blade => blade.arenaTracker.aisle == argumentInts[1]);
				//GizmoidView gizmoidTracker = targetBlade.gizmoidHolder.AddGizmoid(argumentInts[0]);
				//int damage = gizmoidTracker.health.maxHitpoints - argumentInts[2];
				//Debug.Log("Damage: " + damage);
				//gizmoidTracker.health.TakeDamage(damage);
				break;
			case ActionType.Unlock:
				GameProgression gameProgression = (GameProgression)Enum.Parse(typeof(GameProgression), currentAct.action.argument, true);
				gameProfile.gameProgress.Add(gameProgression);
				//InterfaceController.Instance.UnlockPanel(gameProgression);
				break;
			//case ActionType.StartRarityDrops:
			//	//EnemySpawner.Instance.allowEnemyRareDrop = true;
			//	break;
			//case ActionType.StopRarityDrops:
			//	//EnemySpawner.Instance.allowEnemyRareDrop = false;
			//	break;
			default:
				Debug.LogError("Invalid action type: " + currentAct.action.actionType);
				break;
		}
		actIsInitialized = true;

		IncrementAct();
	}


	void HandleCondition()
	{
		if (!actIsInitialized)
		{
			Act currentAct = currentLevel.actRoster[currentActIndex];
			Debug.Log("Condition");
			InitiateCondition(currentAct.condition);
		}



		actIsInitialized = true;
	}

	public void InitiateCondition(Condition condition)
	{
		//		if(dialog.dialogCards.Count > 0)
		//		{
		//			InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive (true);
		//			InterfaceController.Instance.promptLabel.text = dialog.dialogCards [0].text;
		//		} else {
		//			//Do not show prompt 
		//		}

		if (!string.IsNullOrEmpty(condition.promptText))
		{
			//InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive(true);
			//InterfaceController.Instance.promptLabel.text = condition.promptText;
		}

		//Activate listener
		switch (condition.conditionType)
		{
			case ConditionType.None:
				Debug.Log("No condition type specified");
				break;
			case ConditionType.RotateShip:
				StartCoroutine(RotateListener());
				break;
			case ConditionType.PickUpCollectible:
				Debug.Log("Pick up collectible");
				StartCoroutine(PickupListener(
					(CollectibleType)Enum.Parse(typeof(CollectibleType), condition.argument, true)
					));
				break;
			case ConditionType.PlaceGizmoid:
				StartCoroutine(GizmoidAddListener());
				break;
			case ConditionType.DestroyEnemy:
				StartCoroutine("EnemyDestroyListener", int.Parse(condition.argument));
				break;
			case ConditionType.StockpileScrap:
				StartCoroutine("StockpileScrapListener", int.Parse(condition.argument));
				break;
			case ConditionType.WaitForSeconds:
				StartCoroutine("WaitForSeconds", float.Parse(condition.argument));
				break;
			case ConditionType.UseButton:
				if (condition.argument == "trash")
				{
					StartCoroutine(UseTrashListener());
				}
				else
				{
					Debug.LogError("No handling for non-trash use button condition.");
				}
				break;
			default:
				Debug.LogError("Invalid condition type: " + condition.conditionType);
				break;
		}
	}

	//IEnumerator EndPrompt()
	//{
	//	//yield return StartCoroutine(FadeOutPrompt());
	//	//InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive (false);
	//	//yield return new WaitForSeconds(gracePeriod);
	//	//LevelManager.Instance.IncrementAct();
	//}

	//IEnumerator FadeOutPrompt()
	//{
	//	//UISprite promptSprite = InterfaceController.Instance.promptLabel.transform.parent.GetComponent<UISprite>();
	//	if (promptSprite.gameObject.activeSelf)
	//	{
	//		float startingAlpha = promptSprite.alpha;
	//		while (promptSprite.alpha > 0)
	//		{
	//			promptSprite.alpha -= Time.deltaTime * fadeConstant;
	//			yield return 0;
	//		}
	//		yield return new WaitForSeconds(1.25f);
	//		promptSprite.gameObject.SetActive(false);
	//		promptSprite.alpha = startingAlpha;
	//	}
	//}

	#region Listeners

	IEnumerator RotateListener()
	{
		Debug.Log("Listener started.");
		//float lastZRotation = PlayerContolScript.instance.gameObject.transform.eulerAngles.z;
		bool hasSnapped = false;
		bool targetReached = false;
		//yield return null;
		//UISprite promptSprite = InterfaceController.Instance.promptLabel.transform.parent.GetComponent<UISprite>();

		//var tut = Instantiate(InterfaceController.Instance.RotateTutorial) as GameObject;
		//tut.transform.parent = InterfaceController.Instance.gameObject.transform.parent;
		//tut.transform.position = Vector3.zero;
		//tut.transform.localScale = new Vector3(1, 1, 1);

		ArenaHandler.instance.sectors[4].StartPhaseSector();

		//while (!targetReached)
		//{

		//	if (PlayerContolScript.instance.snapping || PlayerContolScript.rotating)
		//	{
		//		tut.SetActive(false);
		//		//promptSprite.alpha = 0;
		//	}
		//	else
		//	{
		//		tut.SetActive(true);
		//		//promptSprite.alpha = 1;
		//	}

		//	if (PlayerContolScript.instance.snapping && !hasSnapped)
		//	{
		//		//print("snapping");
		//		hasSnapped = true;
		//	}

		//	if (!PlayerContolScript.instance.snapping && hasSnapped)
		//	{
		//		//print("snapped " + PlayerContolScript.instance.gameObject.transform.eulerAngles.z);

		//		if (PlayerContolScript.instance.gameObject.transform.eulerAngles.z > 179 && PlayerContolScript.instance.gameObject.transform.eulerAngles.z < 181)
		//		{
		//			print("target reached!");
		//			targetReached = true;
		//		}
		//		else
		//		{
		//			hasSnapped = false;
		//		}
		//	}

		//	yield return null;
		//}

		//ArenaHandler.instance.sectors[4].ClearPhaseSector();
		//Destroy(tut);
		//StartCoroutine(EndPrompt());
		yield return null;
	}


	IEnumerator PickupListener(CollectibleType collectibleType)
	{
		Debug.Log("Pickup listener started @" + Time.frameCount);
		switch (collectibleType)
		{
			case CollectibleType.Blueprint:
				int previousBlueprints = blueprintsCollected;
				while (blueprintsCollected <= previousBlueprints)
				{
					//Debug.Log ("Blueprint loop running @"+Time.frameCount);
					yield return 0;
				}
				break;
			case CollectibleType.BigScrap:
				//int previousScrap = PlayerResources.Instance.Scraps;
				//while (PlayerResources.Instance.Scraps <= previousScrap)
				//{
				//	yield return 0;
				//}
				break;
			case CollectibleType.Xeno:
				print("Pick up Xeno!");
				int previousXenos = gameProfile.xenos;
				while (gameProfile.xenos <= previousXenos)
				{
					yield return 0;
				}
				//yield return new WaitForSeconds(1);

				break;
			default:
				Debug.LogError("Unhandled collectible type: " + collectibleType);
				break;
		}

		//		int previousItemsCollected = LevelManager.Instance.itemsCollected;
		//		while (previousItemsCollected == LevelManager.Instance.itemsCollected) {
		//			yield return 0;
		//		}
		Debug.Log("Ending prompt @" + Time.frameCount);
		//StartCoroutine(EndPrompt());
		yield break;
	}

	IEnumerator GizmoidAddListener()
	{
		//UILabel promptLabel = InterfaceController.Instance.promptLabel.transform.parent.GetComponentInChildren<UILabel>();
		//int previousGizmoidsAdded = gizmoidsAdded;
		//var pointer = Instantiate(InterfaceController.Instance.pointer) as Pointer;
		//pointer.transform.parent = InterfaceController.Instance.cam.transform;
		//pointer.radius = 60;
		//pointer.degree = 315;
		//pointer.size = 0.7f;
		//pointer.point = InterfaceController.Instance.gizmoidButtonPositions[0];
		//while (previousGizmoidsAdded == gizmoidsAdded)
		//{
		//	if (GameController.Instance.interactiveMode == InteractiveMode.Placing)
		//	{
		//		//pointer.gameObject.SetActive(false);
		//		pointer.degree = PlayerContolScript.instance.transform.eulerAngles.z + 90;
		//		pointer.point = Vector3.zero;
		//		pointer.radius = 150;
		//		//InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive(false);
		//		// promptLabel.alpha = 0;

		//	}
		//	else
		//	{
		//		pointer.radius = 60;
		//		pointer.degree = 315;
		//		pointer.point = InterfaceController.Instance.gizmoidButtonPositions[0];
		//		//pointer.gameObject.SetActive(true);
		//		//InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive(true);
		//		promptLabel.alpha = 1;

		//	}
		//	yield return 0;
		//}
		//InterfaceController.Instance.promptLabel.transform.parent.gameObject.SetActive(true);
		//Destroy(pointer.gameObject);
		//StartCoroutine("EndPrompt");
		yield break;
	}

	IEnumerator EnemyDestroyListener(int bodyCount)
	{
		int previousEnemiesDestroyed = totalEnemiesDestroyed;
		while (previousEnemiesDestroyed + bodyCount > totalEnemiesDestroyed)
		{
			yield return 0;
		}


		StartCoroutine("EndPrompt");
		yield break;
	}

	IEnumerator StockpileScrapListener(int targetScrap)
	{
		int initialScraps = totalScrapCollected;
		while (totalScrapCollected - initialScraps < targetScrap)
		{
			yield return 0;
		}
		StartCoroutine("EndPrompt");
		yield break;
	}

	IEnumerator WaitForSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		StartCoroutine("EndPrompt");
	}

	IEnumerator UseTrashListener()
	{
		int initialTrashed = gizmoidsTrashed;
		while (gizmoidsTrashed - initialTrashed < 1)
		{
			//Debug.Log ("now, initial: "+ gizmoidsTrashed + ", "+initialTrashed);
			yield return 0;
		}
		StartCoroutine("EndPrompt");
		yield break;
	}

	#endregion


}
