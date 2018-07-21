using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ArenaHandler))]
public class ArenaHandlerEditor : Editor
{
	public static GUIContent
	sectorButtonContent = new GUIContent("Get Sector Dara");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button(sectorButtonContent))
		{
			((ArenaHandler)target).GetArenaData();
		}
	}

}
