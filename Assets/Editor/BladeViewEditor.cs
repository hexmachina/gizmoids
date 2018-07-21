using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BladeView))]
[CanEditMultipleObjects]
public class BladeViewEditor : Editor
{
	public static GUIContent
	impactButtonContent = new GUIContent("Impact");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//if (GUILayout.Button(impactButtonContent))
		//{
		//	((BladeView)target).Impact(null);
		//}
	}
}
