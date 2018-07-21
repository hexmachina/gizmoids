using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelDirector))]
public class LevelDirectorEditor : Editor
{
	public static GUIContent
	winButtonContent = new GUIContent("Win Level");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button(winButtonContent))
		{
			((LevelDirector)target).ForceVictory();
		}
	}

}
