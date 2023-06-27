using System;
using System.Collections.Generic;
using DefaultNamespace;
using Managers.StageEvents;
using Objects.Enemies;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var stageData = target as StageData;

		var eventList = serializedObject.FindProperty("stageEvents");

		EditorGUILayout.LabelField("Events:");

		EditorGUI.indentLevel++;

		for (var i = 0; i < eventList.arraySize; i++)
		{
			var eventProperty = eventList.GetArrayElementAtIndex(i);

			var triggerTimeProperty = eventProperty.FindPropertyRelative("triggerTime");
			var eventTimeSpan = Utilities.FloatToTimeString(triggerTimeProperty.floatValue);
			var eventName = $"{eventTimeSpan}";

			EditorGUILayout.PropertyField(eventProperty, new GUIContent(eventName), true);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Delete Event", GUILayout.Width(100)))
			{
				eventList.DeleteArrayElementAtIndex(i);
				serializedObject.ApplyModifiedProperties();
				break;
			}
			GUILayout.EndHorizontal();
		}

		EditorGUI.indentLevel--;

		if (GUILayout.Button("Add Event"))
		{
			eventList.InsertArrayElementAtIndex(eventList.arraySize);
		}

		serializedObject.ApplyModifiedProperties();
	}
}