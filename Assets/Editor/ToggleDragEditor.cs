using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(ToggleDrag), true)]
[CanEditMultipleObjects]
public class ToggleDragEditor : SelectableEditor
{
	SerializedProperty m_OnValueChangedProperty;
	SerializedProperty m_OnValueChangedInverseProperty;
	SerializedProperty m_OnDragBeginProperty;
	SerializedProperty m_OnDragProperty;
	SerializedProperty m_OnDragEndProperty;
	SerializedProperty m_TransitionProperty;
	SerializedProperty m_GraphicProperty;
	SerializedProperty m_GroupProperty;
	SerializedProperty m_IsOnProperty;

	protected override void OnEnable()
	{
		base.OnEnable();

		m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
		m_GraphicProperty = serializedObject.FindProperty("graphic");
		m_GroupProperty = serializedObject.FindProperty("m_Group");
		m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
		m_OnValueChangedProperty = serializedObject.FindProperty("onValueChanged");
		m_OnValueChangedInverseProperty = serializedObject.FindProperty("onValueChangedInverse");
		m_OnDragBeginProperty = serializedObject.FindProperty("onDragEnabled");
		m_OnDragProperty = serializedObject.FindProperty("onDrag");
		m_OnDragEndProperty = serializedObject.FindProperty("onDragEnd");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.PropertyField(m_IsOnProperty);
		EditorGUILayout.PropertyField(m_TransitionProperty);
		EditorGUILayout.PropertyField(m_GraphicProperty);
		EditorGUILayout.PropertyField(m_GroupProperty);

		EditorGUILayout.Space();

		// Draw the event notification options
		EditorGUILayout.PropertyField(m_OnValueChangedProperty);

		EditorGUILayout.PropertyField(m_OnValueChangedInverseProperty);

		EditorGUILayout.PropertyField(m_OnDragBeginProperty);

		EditorGUILayout.PropertyField(m_OnDragProperty);

		EditorGUILayout.PropertyField(m_OnDragEndProperty);


		serializedObject.ApplyModifiedProperties();
	}

}
