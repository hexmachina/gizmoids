using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitSelectorButton))]
public class UnitSelectorButtonEditor : Editor
{
	public static GUIContent
	coolButtonContent = new GUIContent("Cooldown");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button(coolButtonContent))
		{
			((UnitSelectorButton)target).StartCooldown();
		}
	}
}
