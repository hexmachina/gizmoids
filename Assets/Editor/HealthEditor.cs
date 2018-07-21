using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
	public static int damage = 20;
	public static int heal = 20;
	public static GUIContent
		damageButtonContent = new GUIContent("Damage"),
	healButtonContent = new GUIContent("Heal");


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal();
		damage = EditorGUILayout.IntField("", damage);

		if (GUILayout.Button(damageButtonContent))
		{
			((Health)target).TakeDamage(damage);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		heal = EditorGUILayout.IntField("", heal);

		if (GUILayout.Button(healButtonContent))
		{
			((Health)target).Heal(heal);
		}
		EditorGUILayout.EndHorizontal();
	}
}
