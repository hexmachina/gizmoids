using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.IO;
using GizLib;

public class LoadLevelData
{
	public static float defaultBigScrapInterval = 7f;
	public static float defaultSmallScrapInterval = 8f;

	[MenuItem("Tools/LoadLevelData")]
	private static void LoadCSVFolder()
	{

		var path = EditorUtility.OpenFolderPanel("Select a Folder Containing CSVs", "", "");

		DirectoryInfo dir = new DirectoryInfo(path);
		FileInfo[] info = dir.GetFiles("*.csv");
		if (info == null || info.Length == 0)
		{
			EditorUtility.DisplayDialog("No CVSs Found", "You must select a folder containing CSV files", "OK");
		}
		else
		{
			var outputPath = EditorUtility.OpenFolderPanel("Select a Level Data Output Folder", "", "");
			foreach (var item in info)
			{
				var lvl = LoadLevelFromCsv(item.FullName);
				SaveLevelData(lvl, outputPath);
			}
		}
	}

	private static void SaveLevelData(LevelData level, string path)
	{
		var fileName = "Level" + level.worldId + "-" + level.localId;
		if (path.Contains("Assets"))
		{
			var index = path.IndexOf("Assets");
			path = path.Substring(index);
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

		AssetDatabase.CreateAsset(level, assetPathAndName);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = level;

	}

	public static LevelData LoadLevelFromCsv(string fullFileName)
	{
		//string filePath = Application.dataPath+"/Data/"+levelFileName;
		Debug.Log("Loading level from csv from " + fullFileName);
		string[] dataLines = File.ReadAllLines(fullFileName);
		Dictionary<int, string> fieldLookup = new Dictionary<int, string>();
		var level = ScriptableObject.CreateInstance<LevelData>();
		Act activeAct = null;

		//Debug
		//Debug.Log (dataLines[0]);


		for (int row = 0; row < dataLines.Length; row++)
		{
			switch (row)
			{
				case 0:
					//Parse top headers
					string[] fieldStrings = dataLines[row].Split(',');

					for (int column = 0; column < fieldStrings.Length; column++)
					{
						fieldLookup.Add(column, fieldStrings[column]);
					}
					break;
				case 1:
					//Parse top fields
					string[] topFieldStrings = dataLines[row].Split(',');
					for (int column = 0; column < fieldLookup.Count; column++)
					{
						switch (fieldLookup[column])
						{
							case "":
								//Do nothing
								break;
							case "World Number":
								level.worldId = int.Parse(topFieldStrings[column]);
								break;
							case "Level Number":
								level.localId = int.Parse(topFieldStrings[column]);
								break;
							case "Initial Scrap":
								if (topFieldStrings[column] == "")
								{
									level.initialScraps = 0;
								}
								else
								{
									level.initialScraps = int.Parse(topFieldStrings[column]);
								}
								break;
							case "Bronze Min.":
								if (topFieldStrings[column] != "")
								{
									level.bronzeMinimum = int.Parse(topFieldStrings[column]);
								}
								break;
							case "Silver Min.":
								if (topFieldStrings[column] != "")
								{
									level.silverMinimum = int.Parse(topFieldStrings[column]);
								}
								break;
							case "Gold Min.":
								if (topFieldStrings[column] != "")
								{
									level.goldMinimum = int.Parse(topFieldStrings[column]);
								}
								break;
							case "Initial Blades":
								level.initialBladeCount = int.Parse(topFieldStrings[column]);
								break;
							case "Big Scrap Interval":
								if (topFieldStrings[column] == "default")
								{
									level.bigScrapInterval = defaultBigScrapInterval;
								}
								else
								{
									level.bigScrapInterval = float.Parse(topFieldStrings[column]);
								}
								break;
							case "Small Scrap Interval":
								if (topFieldStrings[column] == "default")
								{
									level.smallScrapInterval = defaultSmallScrapInterval;
								}
								else
								{
									level.smallScrapInterval = float.Parse(topFieldStrings[column]);
								}
								break;
							case "LevelType":
								level.levelType = topFieldStrings[column];
								break;
							case "Mutations":
								level.mutations = topFieldStrings[column];
								break;
							case "Default Loadout":
								if (string.IsNullOrEmpty(topFieldStrings[column]))
								{
									level.defaultLoadout = new List<int>();
								}
								else
								{
									string[] loadoutStrings = Regex.Split(topFieldStrings[column], "; ");
									List<int> outputLoadout = new List<int>();
									foreach (string s in loadoutStrings)
									{
										outputLoadout.Add(int.Parse(s));
									}
									level.defaultLoadout = outputLoadout;
								}
								break;
							default:
								Debug.LogError("Unhandled field: " + fieldLookup[column] + " on " + level.worldId + "-" + level.localId);
								break;

						}
					}
					break;
				case 2:
					//Empty row: do nothing
					break;
				case 3:
					//Act headers; do nothing
					break;
				default:
					//Handle acts

					//List<string> actCells = SophisticatedSplit(dataLines[row]);

					//RowType rowType = RowType.None;
					//DialogCard tempCard = null;
					//if (actCells[0] != "")
					//{
					//	rowType = RowType.ActTypeHeaders;
					//	if (activeAct == null)
					//	{
					//		activeAct = new Act();
					//	}
					//	else
					//	{
					//		level.actRoster.Add(activeAct);
					//		activeAct = new Act();
					//	}
					//}
					//else
					//{
					//	rowType = RowType.ActRecords;
					//}
					//for (int column = 0; column < actCells.Count; column++)
					//{
					//	//Debug.Log (column);
					//	if (actCells[column] != "")
					//	{
					//		if (rowType == RowType.ActTypeHeaders)
					//		{
					//			if (column == 1)
					//			{ //Second column, indicating act type
					//				switch (actCells[column])
					//				{
					//					case "dialog":
					//						activeAct.actType = ActType.Message;

					//						break;

					//					case "condition":
					//						activeAct.actType = ActType.Condition;
					//						activeAct.condition = new Condition();
					//						break;
					//					case "wave":
					//						activeAct.actType = ActType.Wave;
					//						activeAct.wave = new Wave();
					//						break;
					//					case "action":
					//						activeAct.actType = ActType.Action;
					//						activeAct.action = new LevelAction();
					//						break;
					//					default:
					//						Debug.LogError("Invalid act type: " + actCells[column]);
					//						break;
					//				}
					//			}
					//			else if (column == 2)
					//			{
					//				switch (actCells[column])
					//				{
					//					case "Character":
					//						//print("Found Character!");
					//						activeAct.dialog = new Dialog();
					//						activeAct.dialog.dialogType = DialogType.Character;
					//						break;
					//					case "Info":
					//						activeAct.dialog = new Dialog();
					//						activeAct.dialog.dialogType = DialogType.Info;

					//						break;
					//					default:
					//						break;
					//				}
					//			}
					//		}
					//		else if (rowType == RowType.ActRecords)
					//		{
					//			switch (activeAct.actType)
					//			{
					//				case ActType.Message:
					//					if (activeAct.dialog.dialogType == DialogType.Character)
					//					{
					//						if (column == 2)
					//						{ //Character
					//						  //Debug.Log ("2");

					//							tempCard = new DialogCard();
					//							tempCard.header = actCells[column].ToUpper();
					//							switch (actCells[column])
					//							{
					//								case "joe":
					//									tempCard.cast = CharacterRoster.Joe;
					//									break;
					//								default:
					//									break;
					//							}
					//						}
					//						else if (column == 3)
					//						{ //Emotion
					//						  //Debug.Log ("3"+tempCard.header);
					//						  //Debug.Log ("Location: "+column+", "+row);
					//							switch (actCells[column])
					//							{
					//								case "happy":
					//									tempCard.emotion = Emotions.Happy;
					//									break;
					//								case "explaining":
					//									tempCard.emotion = Emotions.Explaining;
					//									break;
					//								case "ooo":
					//									tempCard.emotion = Emotions.Ooh;
					//									break;
					//								default:
					//									tempCard.emotion = Emotions.Happy;
					//									break;
					//							}
					//							//tempCard.emotion = actCells [column];
					//						}
					//						else if (column == 4)
					//						{ //Text
					//						  //Debug.Log ("4"+tempCard.header);
					//						  //Debug.Log (activeAct.dialog);
					//							tempCard.text = actCells[column];
					//							activeAct.dialog.dialogCards.Add(tempCard);
					//						}
					//						else if (column == 5)
					//						{
					//							if (actCells[column] == "left")
					//							{
					//								tempCard.onRight = false;
					//							}
					//							else if (actCells[column] == "right")
					//							{
					//								tempCard.onRight = true;
					//							}
					//						}
					//					}
					//					else if (activeAct.dialog.dialogType == DialogType.Info)
					//					{

					//					}

					//					break;
					//				case ActType.Condition:
					//					if (column == 2)
					//					{ //Condition Name
					//						switch (actCells[column])
					//						{
					//							case "pick up collectible":
					//								activeAct.condition.conditionType = ConditionType.PickUpCollectible;
					//								break;
					//							case "place gizmoid":
					//								activeAct.condition.conditionType = ConditionType.PlaceGizmoid;
					//								break;
					//							case "stockpile scrap":
					//								activeAct.condition.conditionType = ConditionType.StockpileScrap;
					//								break;
					//							case "wait for seconds":
					//								activeAct.condition.conditionType = ConditionType.WaitForSeconds;
					//								break;
					//							case "rotate ship":
					//								activeAct.condition.conditionType = ConditionType.RotateShip;
					//								break;
					//							case "use button":
					//								activeAct.condition.conditionType = ConditionType.UseButton;
					//								break;
					//							default:
					//								Debug.LogError("Unhandled condition name: " + actCells[column]);
					//								break;
					//						}
					//					}
					//					else if (column == 3)
					//					{ //Argument
					//						activeAct.condition.argument = actCells[column];
					//					}
					//					else if (column == 4)
					//					{ //Prompt
					//						activeAct.condition.promptText = actCells[column];
					//					}
					//					break;
					//				case ActType.Wave:
					//					if (column == 2)
					//					{ //Duration
					//						activeAct.wave.duration = float.Parse(actCells[column]);
					//					}
					//					else if (column == 3)
					//					{ //Possible enemies
					//						foreach (string s in actCells[column].Split(';'))
					//						{
					//							activeAct.wave.possibleEnemies.Add(int.Parse(s));
					//						}
					//						//activeAct.wave.possibleEnemies = actCells[column].Split(';');
					//					}
					//					else if (column == 4)
					//					{ //Wait for enemies?
					//						activeAct.wave.waitForEnemies = actCells[column] == "yes" ? true : false;
					//					}
					//					else if (column == 5)
					//					{ //Delay
					//						activeAct.wave.delay = float.Parse(actCells[column]);
					//					}
					//					else if (column == 6)
					//					{ // Enemy roster
					//						foreach (string s in actCells[column].Split(';'))
					//						{
					//							activeAct.wave.enemyRoster.Add(int.Parse(s));
					//						}


					//						//					activeAct.wave.enemyRoster = actCells[column].Split(';');;
					//					}
					//					else if (column == 7)
					//					{ //Spawn lane
					//						activeAct.wave.spawnLane = int.Parse(actCells[column]);
					//					}
					//					else if (column == 8)
					//					{ //Randomize roster?
					//						activeAct.wave.randomizedRoster = actCells[column] == "yes" ? true : false;
					//					}
					//					else if (column == 9)
					//					{ //Item drop
					//					  //Collectible waveCollectible = new Collectible();
					//					  //string[] dropStings = Regex.Split(actCells[column], "- ");
					//					  //waveCollectible.collectibleType = (CollectibleType)System.Enum.Parse(typeof(CollectibleType), dropStings[0], true);
					//					  //if (waveCollectible.collectibleType == CollectibleType.Blueprint)
					//					  //{
					//					  //	waveCollectible.value = int.Parse(dropStings[1]);
					//					  //}
					//					  //else if (waveCollectible.collectibleType == CollectibleType.Xeno)
					//					  //{
					//					  //	waveCollectible.collectibleSubtype = dropStings[1];
					//					  //	//Do nothing
					//					  //}
					//					  //else
					//					  //{
					//					  //	Debug.LogError("No handling for " + waveCollectible.collectibleType);
					//					  //}
					//					  //activeAct.wave.itemDrop = waveCollectible;

					//					}
					//					else if (column == 10)
					//					{
					//						//activeAct.wave.overclockDrops = int.Parse(actCells[column]);
					//					}
					//					else
					//					{
					//						Debug.LogError("Invalid column on Wave act: " + column);
					//					}


					//					break;
					//				case ActType.Action:
					//					if (column == 2)
					//					{ //Action name
					//						switch (actCells[column])
					//						{
					//							case "spawn big scrap":
					//								activeAct.action.actionType = ActionType.SpawnBigScrap;
					//								break;
					//							case "unlock gizmoid":
					//								activeAct.action.actionType = ActionType.AddGizmoidButton;
					//								break;
					//							case "spawn small scrap":
					//								activeAct.action.actionType = ActionType.SpawnSmallScrap;
					//								break;
					//							//case "start spawning":
					//							//	activeAct.action.actionType = ActionType.StartSpawning;
					//							//	break;
					//							//case "stop spawning":
					//							//	activeAct.action.actionType = ActionType.StopSpawning;
					//							//	break;
					//							case "add blade":
					//								activeAct.action.actionType = ActionType.AddBlade;
					//								break;
					//							//case "stop enemy scrap drop":
					//							//	activeAct.action.actionType = ActionType.StopEnemyScrapDrop;
					//							//	break;
					//							//case "start enemy scrap drop":
					//							//	activeAct.action.actionType = ActionType.StartEnemyScrapDrop;
					//							//	break;
					//							case "insert gizmoid":
					//								activeAct.action.actionType = ActionType.InsertGizmoid;
					//								break;
					//							case "unlock":
					//								activeAct.action.actionType = ActionType.Unlock;
					//								break;
					//							//case "start rarity drops":
					//							//	activeAct.action.actionType = ActionType.StartRarityDrops;
					//							//	break;
					//							//case "stop rarity drops":
					//							//	activeAct.action.actionType = ActionType.StopRarityDrops;
					//							//	break;
					//							default:
					//								Debug.LogError("Unhandled action type: " + actCells[column] + " on " + level.worldId + "-" + level.localId);
					//								break;
					//						}
					//					}
					//					else if (column == 3)
					//					{ //Action argument
					//						activeAct.action.argument = actCells[column];
					//					}
					//					break;
					//				default:
					//					Debug.LogError("Invalid act type: " + activeAct.actType);
					//					break;

					//			}
					//		}
					//		else
					//		{
					//			Debug.LogError("Unexpected row type: " + rowType);
					//		}
					//	}

					//}
					break;
			}
		}
		if (activeAct != null)
		{
			level.actRoster.Add(activeAct);
		}
		return level;
		//attributeData.Add (recordAttribute);

	}

	static List<string> SophisticatedSplit(string inputString)
	{
		//Debug.Log (inputString);
		List<string> returnList = new List<string>();
		string hotString = inputString;
		char[] commaChar = new char[] { ',' };
		//char[] quoteCommaChar = new char[]{'"',','};
		//int i = 0;
		//bool looping = true;
		while (true)
		{
			string[] tempArray = new string[2];
			if (hotString[0] == '"')
			{
				hotString = hotString.Substring(1);
				//Debug.Log ("Double quote is first character at "+i);
				tempArray = hotString.Split(new string[] { "\"," }, 2, System.StringSplitOptions.None);
			}
			else
			{
				tempArray = hotString.Split(commaChar, 2);
			}
			returnList.Add(tempArray[0]);
			//Debug.Log (hotString + ", count:"+tempArray.Length);
			if (tempArray.Length <= 1 || string.IsNullOrEmpty(tempArray[1]))
			{
				break;
			}
			hotString = tempArray[1];
			//Debug.Log (hotString + " @"+i);

			//			if(hotString == "")
			//			{
			//				break;
			//			}
		}
		//		string debugString = "";
		//		foreach (string s in returnList)
		//		{
		//			debugString += s + "|";
		//		}
		//		Debug.Log (debugString+", Count ="+returnList.Count);
		return returnList;
	}
}
