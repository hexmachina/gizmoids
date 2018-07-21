using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LocalDatabase))]
public class LocalDatabaseEditor : Editor
{

	private LocalDatabase localDatabase;
	public static GUIContent
		saveButtonContent = new GUIContent("Save"),
		loadButtonContent = new GUIContent("Load");


	public void OnEnable()
	{
		localDatabase = (LocalDatabase)target;
		//Debug.Log (localDatabase.masterInfo.enemies[0].properName);
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawDefaultInspector();

		if (GUILayout.Button(loadButtonContent))
		{
			localDatabase.LoadProfilesFromUserDirectory();

		}
		if (GUILayout.Button(saveButtonContent))
		{
			foreach (var profile in localDatabase.userProfiles)
			{
				localDatabase.SaveProfileToUserDirectory(profile);
			}
			var go = ((LocalDatabase)target).gameObject;
			PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
			serializedObject.Update();
		}
		serializedObject.ApplyModifiedProperties();
	}



}
