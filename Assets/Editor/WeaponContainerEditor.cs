using System.Collections.Generic;
using Data.ToggleableEntries;
using DefaultNamespace.Data;
using Objects.Items;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(WeaponContainer))]
public class WeaponContainerEditor : Editor
{
	private SerializedProperty availableWeapons;

	private void OnEnable()
	{
		availableWeapons = serializedObject.FindProperty("availableWeapons");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var property = serializedObject.GetIterator();
		var enterChildren = true;
		while (property.NextVisible(enterChildren))
		{
			enterChildren = false;
			if (property.name == "availableWeapons")
			{
				CustomizeAvailableItemList(availableWeapons, "Available Weapons");
				continue;
			}

			EditorGUILayout.PropertyField(property, true);
		}

		
		serializedObject.ApplyModifiedProperties();
	}
	
	private void CustomizeAvailableItemList(SerializedProperty serializedProperty, string title)
	{
		EditorGUILayout.PropertyField(serializedProperty, new GUIContent(title), true);

		if (!serializedProperty.isExpanded)
			return;
	
		// Add Enable/Disable buttons.
		if (GUILayout.Button("Enable All"))
		{
			for (var i = 0; i < serializedProperty.arraySize; i++)
			{
				var item = serializedProperty.GetArrayElementAtIndex(i);
				var isEnabled = item.FindPropertyRelative("isEnabled");
				isEnabled.boolValue = true;
			}
		}

		if (GUILayout.Button("Disable All"))
		{
			for (var i = 0; i < serializedProperty.arraySize; i++)
			{
				var item = serializedProperty.GetArrayElementAtIndex(i);
				var isEnabled = item.FindPropertyRelative("isEnabled");
				isEnabled.boolValue = false;
			}
		}
	}
}
