using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
	int enemyIndex = 0;
	int aisle = -1;
	EnemySpawner enemySpawner;
	public static GUIContent
	buildButtonContent = new GUIContent("Spawn Enemy");

	private void OnEnable()
	{
		enemySpawner = target as EnemySpawner;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (enemySpawner.enemyRoster)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Enemy Type", GUILayout.MaxWidth(100));
			enemyIndex = EditorGUILayout.IntSlider(enemyIndex, 0, enemySpawner.enemyRoster.unitDatas.Count);
			EditorGUILayout.EndHorizontal();

		}

		EditorGUILayout.BeginHorizontal();

		aisle = EditorGUILayout.IntSlider(aisle, -1, 7);
		if (GUILayout.Button(buildButtonContent))
		{
			int index = 0;
			if (enemySpawner.enemyRoster)
			{
				index = enemySpawner.enemyRoster.unitDatas[enemyIndex].typeId;
			}
			enemySpawner.SpawnEnemy(index, aisle);
		}

		EditorGUILayout.EndHorizontal();

	}
}
