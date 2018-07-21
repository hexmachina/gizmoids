using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using GizLib;

#if !UNITY_WINRT
using System.Runtime.Serialization.Formatters.Binary;
#endif
using Pathfinding.Serialization.JsonFx;

#region Enums

//public enum BurnCardType
//{
//	None = -1
//}

//public enum MovementBehavior
//{
//	Normal,
//	StayAtRange
//}

//public enum GizmoidUtility
//{
//	None = 0,
//	FireBullet = 1,
//	FireLaser = 2,
//	CollectScrap = 3,
//	RepelOnContact = 4,
//	GenerateScrap = 5,
//	RepairGizmoid = 6,
//	SoundWave = 7,
//	FreezeTime = 8,
//	FireBubble = 9
//}

//public enum EnemyUtilityBehavior
//{
//	None = 0,
//	Ricochet = 4,
//	Debris = 1,
//	FireLaser = 2,
//	Divide = 3
//}

//public enum Ability
//{
//	ImmuneToMelee = 0
//}

//public enum DialogType
//{
//	None = 0,
//	Character = 1,
//	Tooltip = 2,
//	Info = 3
//	//Prompt = 3
//}

//public enum CharacterRoster
//{
//	Joe = 0,
//	Mopey = 1
//}

//public enum Emotions
//{
//	Happy,
//	Angry,
//	Scared,
//	Confused,
//	Explaining,
//	Ooh,
//	Worried,
//	Laugh,
//	Think,
//	Bonk
//}

//public enum ConditionType
//{
//	None = 0,
//	RotateShip = 1,
//	PickUpCollectible = 2,
//	PlaceGizmoid = 3,
//	DestroyEnemy = 4, //Optional arg: Number of enemies to destroy
//	StockpileScrap = 5, //Requires arg: Target scrap
//	WaitForSeconds = 6,
//	UseButton = 7
//}

public enum LevelCategory
{
	None = 0,
	Settings = 1,
	Acts = 2,
	Mutations = 3
}

//public enum ActType
//{
//	None = 0,
//	Wave = 1,
//	Message = 2,
//	Action = 3,
//	Condition = 4
//}

//public enum ActionType
//{
//	None = 0,
//	SpawnBigScrap = 1, //Optional arg: Number of scrap to spawn
//	AddGizmoidButton = 2, //Requires arg: gizmoid Id to create
//	SpawnSmallScrap = 3,
//	StartSpawning = 4,
//	StopSpawning = 5,
//	AddBlade = 6,
//	StopEnemyScrapDrop = 7,
//	StartEnemyScrapDrop = 8,
//	InsertGizmoid = 9,
//	Unlock = 10,
//	StartRarityDrops = 11,
//	StopRarityDrops = 12

//	//SpawnEnemy = 3 //Required args, 1: enemy id, 2: enemy lane relative to player (0 for front, 1 for front-left...), optional third arg: quantity of enemies

//}

//public enum WaveType
//{
//	None = 0,
//	Generated = 1,
//	Explicit = 2
//}

//public enum RowType
//{
//	None = 0,
//	ActTypeHeaders = 1,
//	ActRecords = 2
//}

//public enum CollectibleType
//{
//	None = 0,
//	BigScrap = 1,
//	Blueprint = 2,
//	Overclocker = 3,
//	GizmoidSkin = 4,
//	Xeno = 5,
//	SpaceCrystal = 6,
//	Crate = 7,
//	GizMod = 8,
//	XenoBronze = 9,
//	XenoSilver = 10,
//	XenoGold = 11,
//	Scrap = 12
//}

#endregion

#region Classes

//[System.Serializable]
//public class Collectible
//{

//	public CollectibleType collectibleType = CollectibleType.None;
//	public string collectibleSubtype = "";
//	//public int gizmoidId = -1;
//	public int value = 0;
//	public int tier = -1;
//}

//[System.Serializable]
//public class Utility
//{
//	public GizmoidUtility utilityBehavior;
//	public List<GizmoidUtility> utilityBehaviors = new List<GizmoidUtility>();
//	//public string[] utilityArgs;
//	public List<string> argumentKeys = new List<string>();
//	public List<string> argumentValues = new List<string>();
//}
//[System.Serializable]
//public class EnemyUtility
//{
//	public EnemyUtilityBehavior utilityBehavior = EnemyUtilityBehavior.None;
//	public List<EnemyUtilityBehavior> utilityBehaviors = new List<EnemyUtilityBehavior>();
//	public List<string> argumentKeys = new List<string>();
//	public List<string> argumentValues = new List<string>();

//}

//[System.Serializable]
//public class Gizmoid
//{
//	public int typeId;
//	public int order;
//	public string properName;
//	public string handle;
//	public int maxLife;
//	//public int power;
//	public int price;
//	public float cooldown;
//	//public float initialFire;
//	//public float trigger;
//	public string description;
//	public Utility utility;
//	//public List<Ability> abilities;

//}

//[System.Serializable]
//public class Blade
//{
//	public int typeId;
//	public string properName;
//	public BladeType type;
//}

//[System.Serializable]
//public class Enemy
//{
//	public int typeId;
//	public string properName;
//	public string handle;
//	public int maxLife;
//	public int power;
//	public float speed;
//	public int dropAmount;
//	public float frequency;
//	public float rareChance;
//	public int rareTier;
//	public string description;
//	public string color = "";
//	public MovementBehavior movementBehavior = MovementBehavior.Normal;
//	//public EnemyUtility utility = EnemyUtility.None;
//	public EnemyUtility utility;
//	//public List<EnemyUtility> utilities;
//	//public List<Ability> abilities = new List<Ability>();
//	public float range;
//}

//[System.Serializable]
//public class Act
//{
//	public ActType actType = ActType.None;
//	public Dialog dialog;
//	public LevelAction action;
//	public Wave wave;
//	public Condition condition;
//	//public bool useGracePeriod = true;
//	//public float duration;
//	//public bool routine;
//	//public float delay;

//	//public int cluster;
//	//public List<Enemy> enemyWave;
//	//public List<Enemy> waveEnemies;
//	//public Trigger endTrigger;
//}

//[System.Serializable]
//public class Condition
//{
//	public ConditionType conditionType;
//	public bool requiredForProgression = true;
//	public string promptText;
//	public string argument;

//}

//[System.Serializable]
//public class Wave
//{
//	//public WaveType waveType = WaveType.None;
//	public float duration = 0;
//	public Collectible itemDrop = null;
//	public CollectibleData dropItem;

//	//Generated wave
//	public List<int> possibleEnemies = new List<int>();
//	public List<string> possibleEnemyNames = new List<string>();
//	public bool waitForEnemies = true; //Whether the act will wait for all enemies to be destroyed before triggering next act
//	public float delay = 0;

//	//Explicit wave
//	public List<int> enemyRoster = new List<int>();
//	public List<string> enemyNames = new List<string>();
//	public int spawnLane = -1; //Spawn lane relative to player. -1 is random, 0 is in front of player, 1 is one row left, etc.
//	public bool randomizedRoster = false;
//	public int overclockDrops = 0;

//}

//[System.Serializable]
//public class Level
//{
//	public int localId = -1;
//	public int worldId = -1;
//	//public int localId = -1;
//	//public int worldId = -1;
//	public int initialParts;
//	public int bronzeMinimum;
//	public int silverMinimum;
//	public int goldMinimum;
//	//public int collect;
//	//public int scrap;
//	public int initialBladeCount;
//	public float bigScrapInterval;
//	public float smallScrapInterval;
//	//public List<int> possibleEnemyTypes = new List<int>(); //List of enemy ids
//	//public List<Dialog> dialogRoster = new List<Dialog>();
//	public string levelType;
//	public string mutations;
//	public List<int> defaultLoadout = new List<int>();
//	public List<Act> actRoster = new List<Act>();

//	public Level()
//	{
//	}
//}

//[System.Serializable]
//public class Dialog
//{
//	public DialogType dialogType = DialogType.None;
//	public List<DialogCard> dialogCards = new List<DialogCard>();
//}

//[System.Serializable]
//public class DialogCard
//{
//	public string text = "";
//	public string header;
//	public Emotions emotion;
//	public CharacterRoster cast;
//	public bool onRight;
//}


//[System.Serializable]
//public class LevelAction
//{
//	public ActionType actionType = ActionType.None;
//	public string argument;
//}

//[System.Serializable]
//public class BurnCard
//{
//	public BurnCardType burnCardType;

//	public string name;
//	public string description;
//	public float argument;
//}


#endregion

public class GameData : MonoBehaviour
{

	//public static GameData Instance;

	//public MasterInfo masterInfo = new MasterInfo();

	//public string saveTextFileName = "MasterInfo.txt";
	//public string loadTextFileName = "MasterInfo.txt";
	//private string legacyFileName = "session-manager.txt";
	//static string binaryFileName = "MasterInfo.dat";
	//static string spreadsheetName = "GizmoidsMasterInfo";
	//private string dataPath;
	//public string levelFileName = "GizmoidsMasterInfo - Level 1-1.csv";

	//public static float defaultBigScrapInterval = 7f;
	//public static float defaultSmallScrapInterval = 8f;

	//public MasterInfo masterInfo = new MasterInfo();

	//public List<Level> levelData = new List<Level>();
	//public List<Gizmoid> gizmoidData = new List<Gizmoid>();
	//public List<Enemy> enemyData = new List<Enemy>();
	//public List<Blade> bladeData = new List<Blade>();
	//public List<Collectible> collectibleData = new List<Collectible>();

	//public Level testLevel;


	void Awake()
	{
		//Instance = this;


		//levelDatabase = LoadLevelsFromCsv();
		//dataPath = Application.dataPath + "/Data/";
		//LoadAndDeserializeFromText();
	}

	// Use this for initialization
	//void Start ()
	//{

	//}

	// Update is called once per frame
	//void Update ()
	//{

	//		if(Input.GetKey(KeyCode.Alpha3))
	//		{
	//			SerializeAndSaveToBinary();
	//		}
	//		if(Input.GetKey(KeyCode.Alpha4))
	//		{
	//			LoadAndDeserializeFromBinary();
	//		}
	//		if(Input.GetKeyUp(KeyCode.Alpha5))
	//		{
	//			Debug.Log ("GetKeyUp5");
	//			LoadAndDeserializeLevelFromCsv();
	//		}

	//		if(Input.GetKeyUp(KeyCode.Alpha6))
	//		{
	//			string inputString = "blah,,mopey,test2,\"Joe, kid, it’s job.\",,,,,";
	//			List<string> stringTest = SophistacatedSplit(inputString);
	//			Debug.Log (inputString);
	//			string testString = "";
	//			//Debug.Log (stringTest[0]);
	//			foreach(string s in stringTest)
	//			{
	//				testString += s + "|";
	//			}
	//			Debug.Log (testString);
	//		}


	//}

	//	public void SerializeAndSaveToText()
	//	{
	//		string dataPath = Application.dataPath + "/Data/";
	//		//JsonWriter jsonWriter = new JsonWriter();
	//		//jsonWriter.
	//		string data = Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(masterInfo);
	//			//JsonWriter.Serialize(masterInfo);
	//		//string data = JsonMapper.ToJson(masterInfo);
	//		//Debug.Log (data);
	//
	//		if(!Directory.Exists(dataPath))
	//		{
	//			Directory.CreateDirectory(dataPath);
	//		}
	//
	//		using (StreamWriter streamWriter = new StreamWriter(dataPath + saveTextFileName))
	//		{
	//			streamWriter.Write (data);
	//		}
	//		Debug.Log ("Master info saved to text file:"+saveTextFileName);
	//	}
	//
	//	private void SerializeAndSaveToBinary()
	//	{
	//#if !UNITY_WINRT
	//		BinaryFormatter binaryFormatter = new BinaryFormatter();
	//		//Debug.Log (dataPath + binaryFileName);
	//		FileStream fileStream = File.Create (Application.dataPath+"/Data/" + binaryFileName);
	//
	//		binaryFormatter.Serialize(fileStream, masterInfo);
	//		fileStream.Close ();
	//
	//		Debug.Log ("Saved to binary @"+Time.frameCount);
	//#endif
	//	}

	//	private void LoadAndDeserializeFromBinary()
	//    {
	//#if !UNITY_WINRT
	//		string dataPath = Application.dataPath + "/Data/";
	//        if(File.Exists (dataPath + binaryFileName))
	//		{
	//			BinaryFormatter binaryFormatter = new BinaryFormatter();
	//			FileStream fileStream = File.Open (dataPath + binaryFileName, FileMode.Open);
	//			masterInfo = (MasterInfo)binaryFormatter.Deserialize(fileStream);
	//			fileStream.Close ();
	//			Debug.Log ("Master info loaded from binary");
	//		} else {
	//			Debug.LogError ("Path not found: " + dataPath + binaryFileName);
	//		}
	//#endif
	//    }

	//	public void LoadAndDeserializeFromText()
	//	{
	//		string dataPath = Application.dataPath + "/Data/";
	//		string data = "";
	//		using (StreamReader streamReader = new StreamReader(dataPath + loadTextFileName))
	//		{
	//			data = streamReader.ReadToEnd();
	//		}
	//
	//		masterInfo = Pathfinding.Serialization.JsonFx.JsonReader.Deserialize<MasterInfo>(data);
	//		Debug.Log ("Master info loaded from text file: "+loadTextFileName);
	//	}

	//public static List<Level> LoadLevelsFromCsv()
	//{
	//	List<Level> output = new List<Level>();
	//	DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Data/Levels/");
	//	FileInfo[] info = dir.GetFiles("*.csv");
	//	foreach (FileInfo file in info)
	//	{
	//		output.Add((LoadLevelFromCsv(file.FullName)));
	//	}
	//	return output;
	//}

	//public static Level LoadLevelFromCsv(string fullFileName)
	//{
	//	//string filePath = Application.dataPath+"/Data/"+levelFileName;
	//	Debug.Log("Loading level from csv from " + fullFileName);
	//	string[] dataLines = File.ReadAllLines(fullFileName);
	//	Dictionary<int, string> fieldLookup = new Dictionary<int, string>();
	//	Level level = new Level();
	//	Act activeAct = null;

	//	//Debug
	//	//Debug.Log (dataLines[0]);


	//	for (int row = 0; row < dataLines.Length; row++)
	//	{
	//		switch (row)
	//		{
	//			case 0:
	//				//Parse top headers
	//				string[] fieldStrings = dataLines[row].Split(',');

	//				for (int column = 0; column < fieldStrings.Length; column++)
	//				{
	//					fieldLookup.Add(column, fieldStrings[column]);
	//				}
	//				break;
	//			case 1:
	//				//Parse top fields
	//				string[] topFieldStrings = dataLines[row].Split(',');
	//				for (int column = 0; column < fieldLookup.Count; column++)
	//				{
	//					switch (fieldLookup[column])
	//					{
	//						case "":
	//							//Do nothing
	//							break;
	//						case "World Number":
	//							level.worldId = int.Parse(topFieldStrings[column]);
	//							break;
	//						case "Level Number":
	//							level.localId = int.Parse(topFieldStrings[column]);
	//							break;
	//						case "Initial Scrap":
	//							if (topFieldStrings[column] == "")
	//							{
	//								level.initialParts = 0;
	//							}
	//							else
	//							{
	//								level.initialParts = int.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "Bronze Min.":
	//							if (topFieldStrings[column] != "")
	//							{
	//								level.bronzeMinimum = int.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "Silver Min.":
	//							if (topFieldStrings[column] != "")
	//							{
	//								level.silverMinimum = int.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "Gold Min.":
	//							if (topFieldStrings[column] != "")
	//							{
	//								level.goldMinimum = int.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "Initial Blades":
	//							level.initialBladeCount = int.Parse(topFieldStrings[column]);
	//							break;
	//						case "Big Scrap Interval":
	//							if (topFieldStrings[column] == "default")
	//							{
	//								level.bigScrapInterval = defaultBigScrapInterval;
	//							}
	//							else
	//							{
	//								level.bigScrapInterval = float.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "Small Scrap Interval":
	//							if (topFieldStrings[column] == "default")
	//							{
	//								level.smallScrapInterval = defaultSmallScrapInterval;
	//							}
	//							else
	//							{
	//								level.smallScrapInterval = float.Parse(topFieldStrings[column]);
	//							}
	//							break;
	//						case "LevelType":
	//							level.levelType = topFieldStrings[column];
	//							break;
	//						case "Mutations":
	//							level.mutations = topFieldStrings[column];
	//							break;
	//						case "Default Loadout":
	//							if (string.IsNullOrEmpty(topFieldStrings[column]))
	//							{
	//								level.defaultLoadout = new List<int>();
	//							}
	//							else
	//							{
	//								string[] loadoutStrings = Regex.Split(topFieldStrings[column], "; ");
	//								List<int> outputLoadout = new List<int>();
	//								foreach (string s in loadoutStrings)
	//								{
	//									outputLoadout.Add(int.Parse(s));
	//								}
	//								level.defaultLoadout = outputLoadout;
	//							}
	//							break;
	//						default:
	//							Debug.LogError("Unhandled field: " + fieldLookup[column] + " on " + level.worldId + "-" + level.localId);
	//							break;

	//					}
	//				}
	//				break;
	//			case 2:
	//				//Empty row: do nothing
	//				break;
	//			case 3:
	//				//Act headers; do nothing
	//				break;
	//			default:
	//				//Handle acts

	//				List<string> actCells = SophisticatedSplit(dataLines[row]);

	//				RowType rowType = RowType.None;
	//				DialogCard tempCard = null;
	//				if (actCells[0] != "")
	//				{
	//					rowType = RowType.ActTypeHeaders;
	//					if (activeAct == null)
	//					{
	//						activeAct = new Act();
	//					}
	//					else
	//					{
	//						level.actRoster.Add(activeAct);
	//						activeAct = new Act();
	//					}
	//				}
	//				else
	//				{
	//					rowType = RowType.ActRecords;
	//				}
	//				for (int column = 0; column < actCells.Count; column++)
	//				{
	//					//Debug.Log (column);
	//					if (actCells[column] != "")
	//					{
	//						if (rowType == RowType.ActTypeHeaders)
	//						{
	//							if (column == 1)
	//							{ //Second column, indicating act type
	//								switch (actCells[column])
	//								{
	//									case "dialog":
	//										activeAct.actType = ActType.Message;

	//										break;

	//									case "condition":
	//										activeAct.actType = ActType.Condition;
	//										activeAct.condition = new Condition();
	//										break;
	//									case "wave":
	//										activeAct.actType = ActType.Wave;
	//										activeAct.wave = new Wave();
	//										break;
	//									case "action":
	//										activeAct.actType = ActType.Action;
	//										activeAct.action = new LevelAction();
	//										break;
	//									default:
	//										Debug.LogError("Invalid act type: " + actCells[column]);
	//										break;
	//								}
	//							}
	//							else if (column == 2)
	//							{
	//								switch (actCells[column])
	//								{
	//									case "Character":
	//										//print("Found Character!");
	//										activeAct.dialog = new Dialog();
	//										activeAct.dialog.dialogType = DialogType.Character;
	//										break;
	//									case "Info":
	//										activeAct.dialog = new Dialog();
	//										activeAct.dialog.dialogType = DialogType.Info;

	//										break;
	//									default:
	//										break;
	//								}
	//							}
	//						}
	//						else if (rowType == RowType.ActRecords)
	//						{
	//							switch (activeAct.actType)
	//							{
	//								case ActType.Message:
	//									if (activeAct.dialog.dialogType == DialogType.Character)
	//									{
	//										if (column == 2)
	//										{ //Character
	//										  //Debug.Log ("2");

	//											tempCard = new DialogCard();
	//											tempCard.header = actCells[column].ToUpper();
	//											switch (actCells[column])
	//											{
	//												case "joe":
	//													tempCard.cast = CharacterRoster.Joe;
	//													break;
	//												default:
	//													break;
	//											}
	//										}
	//										else if (column == 3)
	//										{ //Emotion
	//										  //Debug.Log ("3"+tempCard.header);
	//										  //Debug.Log ("Location: "+column+", "+row);
	//											switch (actCells[column])
	//											{
	//												case "happy":
	//													tempCard.emotion = Emotions.Happy;
	//													break;
	//												case "explaining":
	//													tempCard.emotion = Emotions.Explaining;
	//													break;
	//												case "ooo":
	//													tempCard.emotion = Emotions.Ooh;
	//													break;
	//												default:
	//													tempCard.emotion = Emotions.Happy;
	//													break;
	//											}
	//											//tempCard.emotion = actCells [column];
	//										}
	//										else if (column == 4)
	//										{ //Text
	//										  //Debug.Log ("4"+tempCard.header);
	//										  //Debug.Log (activeAct.dialog);
	//											tempCard.text = actCells[column];
	//											activeAct.dialog.dialogCards.Add(tempCard);
	//										}
	//										else if (column == 5)
	//										{
	//											if (actCells[column] == "left")
	//											{
	//												tempCard.onRight = false;
	//											}
	//											else if (actCells[column] == "right")
	//											{
	//												tempCard.onRight = true;
	//											}
	//										}
	//									}
	//									else if (activeAct.dialog.dialogType == DialogType.Info)
	//									{

	//									}

	//									break;
	//								case ActType.Condition:
	//									if (column == 2)
	//									{ //Condition Name
	//										switch (actCells[column])
	//										{
	//											case "pick up collectible":
	//												activeAct.condition.conditionType = ConditionType.PickUpCollectible;
	//												break;
	//											case "place gizmoid":
	//												activeAct.condition.conditionType = ConditionType.PlaceGizmoid;
	//												break;
	//											case "stockpile scrap":
	//												activeAct.condition.conditionType = ConditionType.StockpileScrap;
	//												break;
	//											case "wait for seconds":
	//												activeAct.condition.conditionType = ConditionType.WaitForSeconds;
	//												break;
	//											case "rotate ship":
	//												activeAct.condition.conditionType = ConditionType.RotateShip;
	//												break;
	//											case "use button":
	//												activeAct.condition.conditionType = ConditionType.UseButton;
	//												break;
	//											default:
	//												Debug.LogError("Unhandled condition name: " + actCells[column]);
	//												break;
	//										}
	//									}
	//									else if (column == 3)
	//									{ //Argument
	//										activeAct.condition.argument = actCells[column];
	//									}
	//									else if (column == 4)
	//									{ //Prompt
	//										activeAct.condition.promptText = actCells[column];
	//									}
	//									break;
	//								case ActType.Wave:
	//									if (column == 2)
	//									{ //Duration
	//										activeAct.wave.duration = float.Parse(actCells[column]);
	//									}
	//									else if (column == 3)
	//									{ //Possible enemies
	//										foreach (string s in actCells[column].Split(';'))
	//										{
	//											activeAct.wave.possibleEnemies.Add(int.Parse(s));
	//										}
	//										//activeAct.wave.possibleEnemies = actCells[column].Split(';');
	//									}
	//									else if (column == 4)
	//									{ //Wait for enemies?
	//										activeAct.wave.waitForEnemies = actCells[column] == "yes" ? true : false;
	//									}
	//									else if (column == 5)
	//									{ //Delay
	//										activeAct.wave.delay = float.Parse(actCells[column]);
	//									}
	//									else if (column == 6)
	//									{ // Enemy roster
	//										foreach (string s in actCells[column].Split(';'))
	//										{
	//											activeAct.wave.enemyRoster.Add(int.Parse(s));
	//										}


	//										//					activeAct.wave.enemyRoster = actCells[column].Split(';');;
	//									}
	//									else if (column == 7)
	//									{ //Spawn lane
	//										activeAct.wave.spawnLane = int.Parse(actCells[column]);
	//									}
	//									else if (column == 8)
	//									{ //Randomize roster?
	//										activeAct.wave.randomizedRoster = actCells[column] == "yes" ? true : false;
	//									}
	//									else if (column == 9)
	//									{ //Item drop
	//										Collectible waveCollectible = new Collectible();
	//										string[] dropStings = Regex.Split(actCells[column], "- ");
	//										waveCollectible.collectibleType = (CollectibleType)System.Enum.Parse(typeof(CollectibleType), dropStings[0], true);
	//										if (waveCollectible.collectibleType == CollectibleType.Blueprint)
	//										{
	//											waveCollectible.value = int.Parse(dropStings[1]);
	//										}
	//										else if (waveCollectible.collectibleType == CollectibleType.Xeno)
	//										{
	//											waveCollectible.collectibleSubtype = dropStings[1];
	//											//Do nothing
	//										}
	//										else
	//										{
	//											Debug.LogError("No handling for " + waveCollectible.collectibleType);
	//										}
	//										activeAct.wave.itemDrop = waveCollectible;

	//									}
	//									else if (column == 10)
	//									{
	//										activeAct.wave.overclockDrops = int.Parse(actCells[column]);
	//									}
	//									else
	//									{
	//										Debug.LogError("Invalid column on Wave act: " + column);
	//									}


	//									break;
	//								case ActType.Action:
	//									if (column == 2)
	//									{ //Action name
	//										switch (actCells[column])
	//										{
	//											case "spawn big scrap":
	//												activeAct.action.actionType = ActionType.SpawnBigScrap;
	//												break;
	//											case "unlock gizmoid":
	//												activeAct.action.actionType = ActionType.AddGizmoidButton;
	//												break;
	//											case "spawn small scrap":
	//												activeAct.action.actionType = ActionType.SpawnSmallScrap;
	//												break;
	//											case "start spawning":
	//												activeAct.action.actionType = ActionType.StartSpawning;
	//												break;
	//											case "stop spawning":
	//												activeAct.action.actionType = ActionType.StopSpawning;
	//												break;
	//											case "add blade":
	//												activeAct.action.actionType = ActionType.AddBlade;
	//												break;
	//											case "stop enemy scrap drop":
	//												activeAct.action.actionType = ActionType.StopEnemyScrapDrop;
	//												break;
	//											case "start enemy scrap drop":
	//												activeAct.action.actionType = ActionType.StartEnemyScrapDrop;
	//												break;
	//											case "insert gizmoid":
	//												activeAct.action.actionType = ActionType.InsertGizmoid;
	//												break;
	//											case "unlock":
	//												activeAct.action.actionType = ActionType.Unlock;
	//												break;
	//											case "start rarity drops":
	//												activeAct.action.actionType = ActionType.StartRarityDrops;
	//												break;
	//											case "stop rarity drops":
	//												activeAct.action.actionType = ActionType.StopRarityDrops;
	//												break;
	//											default:
	//												Debug.LogError("Unhandled action type: " + actCells[column] + " on " + level.worldId + "-" + level.localId);
	//												break;
	//										}
	//									}
	//									else if (column == 3)
	//									{ //Action argument
	//										activeAct.action.argument = actCells[column];
	//									}
	//									break;
	//								default:
	//									Debug.LogError("Invalid act type: " + activeAct.actType);
	//									break;

	//							}
	//						}
	//						else
	//						{
	//							Debug.LogError("Unexpected row type: " + rowType);
	//						}
	//					}

	//				}
	//				break;
	//		}
	//	}
	//	if (activeAct != null)
	//	{
	//		level.actRoster.Add(activeAct);
	//	}
	//	return level;
	//	//attributeData.Add (recordAttribute);



	//}

	//static List<string> SophisticatedSplit(string inputString)
	//{
	//	//Debug.Log (inputString);
	//	List<string> returnList = new List<string>();
	//	string hotString = inputString;
	//	char[] commaChar = new char[] { ',' };
	//	//char[] quoteCommaChar = new char[]{'"',','};
	//	//int i = 0;
	//	//bool looping = true;
	//	while (true)
	//	{
	//		string[] tempArray = new string[2];
	//		if (hotString[0] == '"')
	//		{
	//			hotString = hotString.Substring(1);
	//			//Debug.Log ("Double quote is first character at "+i);
	//			tempArray = hotString.Split(new string[] { "\"," }, 2, System.StringSplitOptions.None);
	//		}
	//		else
	//		{
	//			tempArray = hotString.Split(commaChar, 2);
	//		}
	//		returnList.Add(tempArray[0]);
	//		//Debug.Log (hotString + ", count:"+tempArray.Length);
	//		if (tempArray.Length <= 1 || string.IsNullOrEmpty(tempArray[1]))
	//		{
	//			break;
	//		}
	//		hotString = tempArray[1];
	//		//Debug.Log (hotString + " @"+i);

	//		//			if(hotString == "")
	//		//			{
	//		//				break;
	//		//			}
	//	}
	//	//		string debugString = "";
	//	//		foreach (string s in returnList)
	//	//		{
	//	//			debugString += s + "|";
	//	//		}
	//	//		Debug.Log (debugString+", Count ="+returnList.Count);
	//	return returnList;
	//}

	//public static List<Gizmoid> LoadGizmoidsFromCsv()
	//{
	//	string fullFileName = Application.dataPath + "/Data/Units/" + spreadsheetName + " - " + "Gizmoids.csv";
	//	Debug.Log("Loading gizmoids from csv from " + fullFileName);
	//	string[] dataLines = File.ReadAllLines(fullFileName);
	//	Dictionary<int, string> fieldLookup = new Dictionary<int, string>();

	//	//Non-Generic Construct object of return type
	//	List<Gizmoid> gizmoidOutput = new List<Gizmoid>();

	//	for (int row = 0; row < dataLines.Length; row++)
	//	{
	//		//Non-generic: Create record object
	//		Gizmoid gizmoid = new Gizmoid();

	//		switch (row)
	//		{
	//			case 0:
	//				//Parse headers
	//				string[] fieldStrings = dataLines[row].Split(',');

	//				for (int column = 0; column < fieldStrings.Length; column++)
	//				{
	//					fieldLookup.Add(column, fieldStrings[column]);
	//				}
	//				break;
	//			default:
	//				//Non-Generic Parse records
	//				//string [] recordStrings = dataLines [row].Split (',');
	//				List<string> recordStrings = SophisticatedSplit(dataLines[row]);
	//				for (int column = 0; column < recordStrings.Count; column++)
	//				{
	//					Debug.Log(column);
	//					if (recordStrings[column] != "")
	//					{
	//						switch (fieldLookup[column])
	//						{
	//							//Given table field, write data to object
	//							case "":
	//								//Ignore all data in this column
	//								break;
	//							case "ID":
	//								gizmoid.typeId = int.Parse(recordStrings[column]);
	//								break;
	//							case "Order":
	//								gizmoid.order = int.Parse(recordStrings[column]);
	//								break;
	//							case "Proper Name":
	//								gizmoid.properName = recordStrings[column];
	//								break;
	//							case "Handle":
	//								gizmoid.handle = recordStrings[column];
	//								break;
	//							case "Max Life":
	//								gizmoid.maxLife = int.Parse(recordStrings[column]);
	//								break;
	//							case "Price":
	//								gizmoid.price = int.Parse(recordStrings[column]);
	//								break;
	//							case "Cooldown":
	//								gizmoid.cooldown = float.Parse(recordStrings[column]);
	//								break;
	//							case "Description":
	//								gizmoid.description = recordStrings[column];
	//								break;
	//							case "Utility":
	//								Utility utility = new Utility();
	//								utility.utilityBehavior = (GizmoidUtility)System.Enum.Parse(typeof(GizmoidUtility), recordStrings[column], true);
	//								gizmoid.utility = utility;
	//								break;
	//							case "Utility Args":
	//								string[] pairs = recordStrings[column].Split(';');

	//								foreach (string pair in pairs)
	//								{
	//									string[] keyValueArray = pair.Split(':');
	//									gizmoid.utility.argumentKeys.Add(keyValueArray[0]);
	//									gizmoid.utility.argumentValues.Add(keyValueArray[1]);
	//									print("hello");
	//								}
	//								//gizmoid.utility.utilityArgs = recordStrings[column].Split(';');
	//								break;
	//							case "Ability Array":
	//								string[] abilityStrings = recordStrings[column].Split(';');
	//								//Ability[] abilities = new Ability[abilityStrings.Length];
	//								//for (int i = 0; i < abilityStrings.Length; i++)
	//								//{
	//								//	abilities[i] = (Ability)System.Enum.Parse(typeof(Ability), abilityStrings[i], true);
	//								//}
	//								break;
	//							default:
	//								Debug.LogError("Unhandled field: " + fieldLookup[column]);
	//								break;
	//						}
	//					}
	//				}
	//				//Non-generic: add record to output
	//				gizmoidOutput.Add(gizmoid);
	//				break;
	//		}

	//	}

	//	//Non-generic: return output
	//	return gizmoidOutput;
	//}

	//public static List<Enemy> LoadEnemiesFromCsv()
	//{
	//	string fullFileName = Application.dataPath + "/Data/Units/" + spreadsheetName + " - " + "Enemies.csv";
	//	Debug.Log("Loading enemies from csv from " + fullFileName);
	//	string[] dataLines = File.ReadAllLines(fullFileName);
	//	Dictionary<int, string> fieldLookup = new Dictionary<int, string>();

	//	//Non-Generic Construct object of return type
	//	List<Enemy> enemyOutput = new List<Enemy>();

	//	for (int row = 0; row < dataLines.Length; row++)
	//	{
	//		//Non-generic: Create record object
	//		Enemy enemy = new Enemy();

	//		switch (row)
	//		{
	//			case 0:
	//				//Parse headers
	//				string[] fieldStrings = dataLines[row].Split(',');

	//				for (int column = 0; column < fieldStrings.Length; column++)
	//				{
	//					fieldLookup.Add(column, fieldStrings[column]);
	//				}
	//				break;
	//			default:
	//				//Non-Generic Parse records
	//				//string [] recordStrings = dataLines [row].Split (',');
	//				List<string> recordStrings = SophisticatedSplit(dataLines[row]);
	//				for (int column = 0; column < recordStrings.Count; column++)
	//				{
	//					Debug.Log(column);
	//					if (recordStrings[column] != "")
	//					{
	//						switch (fieldLookup[column])
	//						{
	//							//Given table field, write data to object
	//							case "":
	//								//Ignore all data in this column
	//								break;
	//							case "ID":
	//								enemy.typeId = int.Parse(recordStrings[column]);
	//								break;
	//							case "Proper Name":
	//								enemy.properName = recordStrings[column];
	//								break;
	//							case "Handle":
	//								enemy.handle = recordStrings[column];
	//								break;
	//							case "Max Life":
	//								enemy.maxLife = int.Parse(recordStrings[column]);
	//								break;
	//							case "Power":
	//								enemy.power = int.Parse(recordStrings[column]);
	//								break;
	//							case "Speed":
	//								enemy.speed = float.Parse(recordStrings[column]);
	//								break;
	//							case "Range":
	//								enemy.range = float.Parse(recordStrings[column]);
	//								break;
	//							case "Scrap Drop Amount":
	//								enemy.dropAmount = int.Parse(recordStrings[column]);
	//								break;
	//							case "Frequency":
	//								enemy.frequency = float.Parse(recordStrings[column]);
	//								break;
	//							case "Rare Chance":
	//								enemy.rareChance = float.Parse(recordStrings[column]);
	//								break;
	//							case "Rare Tier":
	//								enemy.rareTier = int.Parse(recordStrings[column]);
	//								//Debug.Log("Rare Tier " + enemy.rareTier);
	//								break;
	//							case "Description":
	//								enemy.description = recordStrings[column];
	//								break;
	//							case "Color":
	//								enemy.color = recordStrings[column];
	//								break;
	//							case "Movement Behavior":
	//								enemy.movementBehavior = (MovementBehavior)System.Enum.Parse(typeof(MovementBehavior), recordStrings[column], true);
	//								break;
	//							case "Utility":
	//								//enemy.utility = new EnemyUtility();

	//								string[] utlist = recordStrings[column].Split(';');
	//								foreach (var item in utlist)
	//								{
	//									enemy.utility.utilityBehaviors.Add((EnemyUtilityBehavior)System.Enum.Parse(typeof(EnemyUtilityBehavior), item, true));
	//								}
	//								//enemy.utility.utilityBehavior = (EnemyUtilityBehavior)System.Enum.Parse (typeof(EnemyUtilityBehavior), recordStrings [column], true);
	//								break;
	//							case "Utility Args":
	//								//Debug.Log ("No handling for utility args");
	//								string[] pairs = recordStrings[column].Split(';');

	//								foreach (string pair in pairs)
	//								{
	//									string[] keyValueArray = pair.Split(':');
	//									enemy.utility.argumentKeys.Add(keyValueArray[0]);
	//									enemy.utility.argumentValues.Add(keyValueArray[1]);
	//									print("hello");
	//								}
	//								break;
	//							case "Abilities":
	//								string[] abilityStrings = recordStrings[column].Split(';');

	//								//foreach (string s in abilityStrings)
	//								//{
	//								//	enemy.abilities.Add((Ability)System.Enum.Parse(typeof(Ability), recordStrings[column], true));
	//								//}
	//								break;
	//							default:
	//								Debug.LogError("Unhandled field: " + fieldLookup[column]);
	//								break;
	//						}
	//					}
	//				}
	//				//Non-generic: add record to output
	//				enemyOutput.Add(enemy);
	//				break;
	//		}

	//	}
	//	return enemyOutput;
	//}

	//public static List<Blade> LoadBladesFromCsv()
	//{
	//	string fullFileName = Application.dataPath + "/Data/Units/" + spreadsheetName + " - " + "Blades.csv";
	//	Debug.Log("Loading blades from csv from " + fullFileName);
	//	string[] dataLines = File.ReadAllLines(fullFileName);
	//	Dictionary<int, string> fieldLookup = new Dictionary<int, string>();

	//	//Non-Generic Construct object of return type
	//	List<Blade> bladeOutput = new List<Blade>();

	//	for (int row = 0; row < dataLines.Length; row++)
	//	{
	//		//Non-generic: Create record object
	//		Blade blade = new Blade();

	//		switch (row)
	//		{
	//			case 0:
	//				//Parse headers
	//				string[] fieldStrings = dataLines[row].Split(',');

	//				for (int column = 0; column < fieldStrings.Length; column++)
	//				{
	//					fieldLookup.Add(column, fieldStrings[column]);
	//				}
	//				break;
	//			default:
	//				//Non-Generic Parse records
	//				//string [] recordStrings = dataLines [row].Split (',');
	//				List<string> recordStrings = SophisticatedSplit(dataLines[row]);
	//				for (int column = 0; column < recordStrings.Count; column++)
	//				{
	//					Debug.Log(column);
	//					if (recordStrings[column] != "")
	//					{
	//						switch (fieldLookup[column])
	//						{
	//							//Given table field, write data to object
	//							case "":
	//								//Ignore all data in this column
	//								break;
	//							case "ID":
	//								blade.typeId = int.Parse(recordStrings[column]);
	//								break;
	//							case "Proper Name":
	//								blade.properName = recordStrings[column];
	//								break;
	//							case "Type":
	//								blade.type = (BladeType)System.Enum.Parse(typeof(BladeType), recordStrings[column], true);
	//								break;
	//							default:
	//								Debug.LogError("Unhandled field: " + fieldLookup[column]);
	//								break;
	//						}
	//					}
	//				}
	//				//Non-generic: add record to output
	//				bladeOutput.Add(blade);
	//				break;
	//		}

	//	}

	//	//Non-generic: return output
	//	return bladeOutput;
	//}

	//public static List<Collectible> LoadCollectiblesFromCsv()
	//{
	//	string fullFileName = Application.dataPath + "/Data/Items/" + spreadsheetName + " - " + "Collectibles.csv";
	//	Debug.Log("Loading colletibles from csv from " + fullFileName);
	//	string[] dataLines = File.ReadAllLines(fullFileName);
	//	Dictionary<int, string> fieldLookup = new Dictionary<int, string>();

	//	//Non-Generic Construct object of return type
	//	List<Collectible> collectibleOutput = new List<Collectible>();

	//	for (int row = 0; row < dataLines.Length; row++)
	//	{
	//		//Non-generic: Create record object
	//		Collectible collectible = new Collectible();

	//		switch (row)
	//		{
	//			case 0:
	//				//Parse headers
	//				string[] fieldStrings = dataLines[row].Split(',');

	//				for (int column = 0; column < fieldStrings.Length; column++)
	//				{
	//					fieldLookup.Add(column, fieldStrings[column]);
	//				}
	//				break;
	//			default:
	//				//Non-Generic Parse records
	//				List<string> recordStrings = SophisticatedSplit(dataLines[row]);
	//				for (int column = 0; column < recordStrings.Count; column++)
	//				{
	//					Debug.Log(column);
	//					if (recordStrings[column] != "")
	//					{
	//						switch (fieldLookup[column])
	//						{
	//							case "":
	//								//Ignore all data in this column
	//								break;
	//							case "Type":
	//								collectible.collectibleType = (CollectibleType)System.Enum.Parse(typeof(CollectibleType), recordStrings[column], true);
	//								break;
	//							case "Subtype":
	//								collectible.collectibleSubtype = recordStrings[column];
	//								break;
	//							default:
	//								Debug.LogError("Unhandled field: " + fieldLookup[column]);
	//								break;
	//						}
	//					}
	//				}
	//				//Non-generic: add record to output
	//				collectibleOutput.Add(collectible);
	//				break;
	//		}

	//	}

	//	//Non-generic: return output
	//	return collectibleOutput;
	//}

	//	public static List<T> LoadFromCSV<T>(string fullFileName) where T : new()
	//	{
	//		//string fullFileName = Application.dataPath+"/Data/Units/"+spreadsheetName + " - " + "Blades.csv";
	//		Debug.Log ("Loading csv from " + fullFileName);
	//		string[] dataLines = File.ReadAllLines (fullFileName);
	//		Dictionary<int, string> fieldLookup = new Dictionary<int, string> ();
	//		
	//		//Non-Generic Construct object of return type
	//		//List<Blade> bladeOutput = new List<Blade> ();
	//		List<T> output = new List<T> ();
	//
	//		for (int row = 0; row < dataLines.Length; row++) 
	//		{
	//			//Non-generic: Create record object
	//			//Blade blade = new Blade();
	//			T item = new T ();
	//			
	//			switch (row) 
	//			{
	//			case 0:
	//				//Parse headers
	//				string [] fieldStrings = dataLines [row].Split (',');
	//				
	//				for (int column = 0; column < fieldStrings.Length; column++) 
	//				{
	//					fieldLookup.Add (column, fieldStrings [column]);
	//				}
	//				break;
	//			default:
	//				//Non-Generic Parse records
	//				List<string> recordStrings = SophisticatedSplit(dataLines[row]);
	//				for (int column = 0; column < recordStrings.Count; column++) 
	//				{
	//					if(recordStrings[column] != "")
	//					{
	//						//Write data to item
	//						WriteCellToItem(item, fieldLookup[column], recordStrings[column]);
	//
	//
	//					}
	//				}
	//				//Non-generic: add record to output
	//				output.Add (item);
	//				break;
	//			}
	//			
	//		}
	//		
	//		//Non-generic: return output
	//		return output;
	//	}

	//	static T WriteCellToItem<T> (T item, string fieldName, string cellData)
	//	{
	//		Blade blade = (Blade)item;
	//		switch(typeof(T).ToString())
	//		{
	//		case "Blade":
	//			switch (fieldName) 
	//										{
	//										case "":
	//											//Ignore all data in this column
	//											break;
	//										case "ID":
	//											item.typeId = int.Parse(cellData);
	//											break;
	//										case "Proper Name":
	//				item.properName = recordStrings[cellData];
	//				break;
	//										case "Type":
	//				item.type = (BladeType)System.Enum.Parse(typeof(BladeType), cellData, true);
	//											break;
	//										default:
	//											Debug.LogError ("Unhandled field: " + fieldName);
	//											break;
	//			}
	//			break;
	//		}
	//	}
}
