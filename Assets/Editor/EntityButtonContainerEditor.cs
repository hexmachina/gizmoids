using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityButtonContainer))]
public class EntityButtonContainerEditor : Editor
{
	public static GUIContent
	placeButtonContent = new GUIContent("Add Buttons");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button(placeButtonContent))
		{
			((EntityButtonContainer)target).BuildTestButtons();
		}
	}

}
