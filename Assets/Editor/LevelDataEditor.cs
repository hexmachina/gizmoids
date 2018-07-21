using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
	private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

	private static GUIContent
		moveUpButtonContent = new GUIContent("\u2191", "move up"),
		moveDownButtonContent = new GUIContent("\u2193", "move down"),
		duplicateButtonContent = new GUIContent("+", "duplicate"),
		deleteButtonContent = new GUIContent("-", "delete"),
		addButtonContent = new GUIContent("+", "add element");

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var iterator = serializedObject.GetIterator();
		bool next = iterator.NextVisible(true);
		while (next)
		{
			if (iterator.name != "actRoster")
			{
				EditorGUILayout.PropertyField(iterator, true);
			}
			next = iterator.NextVisible(false);
		}
		var roster = serializedObject.FindProperty("actRoster");
		EditorGUILayout.PropertyField(roster);
		ShowElements(roster);
		serializedObject.ApplyModifiedProperties();
	}

	private static void ShowElements(SerializedProperty list)
	{
		if (list.isExpanded)
		{
			for (int i = 0; i < list.arraySize; i++)
			{
				ShowElementChildren(list, i);
			}
			if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
			{
				list.arraySize += 1;
			}
		}
	}

	private static void ShowElementChildren(SerializedProperty list, int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Act " + index);

		var element = list.GetArrayElementAtIndex(index);
		var enumProp = element.FindPropertyRelative("actType");
		if (enumProp != null)
		{
			EditorGUILayout.PropertyField(enumProp, new GUIContent());
			ShowButtons(list, index);
			EditorGUILayout.EndHorizontal();
			var actType = enumProp.enumNames[enumProp.enumValueIndex];
			if (actType != "None")
			{
				EditorGUI.indentLevel += 1;
				if (actType == "Message")
				{
					EditorGUILayout.PropertyField(element.FindPropertyRelative("dialog"), true);
				}
				else
				{
					EditorGUILayout.PropertyField(element.FindPropertyRelative(actType.ToLower()), true);
				}
				EditorGUI.indentLevel -= 1;
			}
		}
	}

	private static void ShowButtons(SerializedProperty list, int index)
	{
		if (GUILayout.Button(moveUpButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
		{
			if (index - 1 >= 0)
			{
				list.MoveArrayElement(index, index - 1);
			}
		}
		if (GUILayout.Button(moveDownButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
		{
			list.MoveArrayElement(index, index + 1);
		}
		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
		{
			list.InsertArrayElementAtIndex(index);
		}
		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
		{
			int oldSize = list.arraySize;
			list.DeleteArrayElementAtIndex(index);
			if (list.arraySize == oldSize)
			{
				list.DeleteArrayElementAtIndex(index);
			}
		}
	}
}
