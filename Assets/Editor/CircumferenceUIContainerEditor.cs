using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircumferenceUIContainer))]
public class CircumferenceUIContainerEditor : Editor
{

	//public static GUIContent
	//placeButtonContent = new GUIContent("Place Items");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		//if (GUILayout.Button(placeButtonContent))
		//{
		//	((CircumferenceUIContainer)target).PlaceItems();
		//}
	}
}
