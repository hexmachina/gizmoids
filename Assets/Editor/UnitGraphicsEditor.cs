using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UnitGraphics))]
public class UnitGraphicsEditor : Editor
{
	Color matColor = Color.red;

	public static GUIContent
	startButtonContent = new GUIContent("Start Rainbow"),
	endButtonContent = new GUIContent("End Rainbow"),
	startPhaseButtonContent = new GUIContent("Start Phase"),
	endPhaseButtonContent = new GUIContent("End Phase");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button(startButtonContent))
		{
			((UnitGraphics)target).StartRainbow();
		}
		if (GUILayout.Button(endButtonContent))
		{
			((UnitGraphics)target).EndRainbow();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		matColor = EditorGUILayout.ColorField("Phase Color", matColor);

		if (GUILayout.Button(startPhaseButtonContent))
		{
			((UnitGraphics)target).StartColorPhase(0.5f, matColor);
		}
		if (GUILayout.Button(endPhaseButtonContent))
		{
			((UnitGraphics)target).EndColorPhase();
		}
		EditorGUILayout.EndHorizontal();

	}
}
