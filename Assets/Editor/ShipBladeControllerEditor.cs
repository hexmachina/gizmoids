using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipBladeController))]
public class ShipBladeControllerEditor : Editor
{
	int bladeCount = 0;
	public static GUIContent
	buildButtonContent = new GUIContent("Build Blades");


	ShipBladeController blade;

	private void OnEnable()
	{
		blade = (ShipBladeController)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.BeginHorizontal();
		bladeCount = EditorGUILayout.IntSlider(bladeCount, 0, 8);
		if (GUILayout.Button(buildButtonContent))
		{
			blade.PlaceBlades(bladeCount);
		}
		EditorGUILayout.EndHorizontal();

	}
}
