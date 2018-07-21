using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArenaColliderGridData))]
public class ArenaColliderGridDataEditor : Editor
{
	bool[][] grid =
	{
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
		new bool[] {false, false, false, false, false, false, false, false},
	};

	ArenaColliderGridData arena;

	private void OnEnable()
	{
		arena = (ArenaColliderGridData)target;
		for (int i = 7; i > -1; i--)
		{
			for (int j = 0; j < 8; j++)
			{
				var sector = arena.zoneSelections.Find(x => x.zone == j);
				if (sector != null && sector.sections.Contains(i))
				{
					grid[j][i] = true;
				}
			}
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		for (int i = 7; i > -1; i--)
		{
			EditorGUILayout.BeginHorizontal();
			for (int j = 0; j < 8; j++)
			{
				grid[j][i] = EditorGUILayout.Toggle(grid[j][i]);
				if (grid[j][i])
				{
					var sector = arena.zoneSelections.Find(x => x.zone == j);
					if (sector == null)
					{
						arena.zoneSelections.Add(new GizLib.ZoneSelection());
						sector = arena.zoneSelections[arena.zoneSelections.Count - 1];
						sector.zone = j;
					}
					if (!sector.sections.Contains(i))
					{
						sector.sections.Add(i);
					}
				}
				else
				{
					var sector = arena.zoneSelections.Find(x => x.zone == j);
					if (sector != null)
					{
						if (sector.sections.Contains(i))
						{
							sector.sections.Remove(i);
							if (sector.sections.Count == 0)
							{
								arena.zoneSelections.Remove(sector);
							}
						}

					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Space();
		base.OnInspectorGUI();

		serializedObject.ApplyModifiedProperties();
	}
}
