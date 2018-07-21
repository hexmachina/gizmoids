using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ArenaCollider))]
public class ArenaColliderEditor : Editor
{
	int offset = 0;

	public static GUIContent
	buildButtonContent = new GUIContent("Build Colliders");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("Offset");

		offset = EditorGUILayout.IntSlider(offset, 0, 8);

		if (GUILayout.Button(buildButtonContent))
		{
			((ArenaCollider)target).BuildColliders(offset);
		}
		EditorGUILayout.EndHorizontal();

	}
}
