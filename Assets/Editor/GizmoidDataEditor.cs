using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(GizmoidData))]
public class GizmoidDataEditor : Editor
{
	Sprite sprite;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var texture = AssetPreview.GetAssetPreview(((GizmoidData)target).Icon);
		GUILayout.Label(texture);

	}
}
