using System.Collections.Generic;
using Data.ToggleableEntries;
using Objects.Items;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor : Editor
{
	private SerializedProperty availableItems;
	private SerializedProperty availableWeapons;

	private void OnEnable()
	{
		availableItems = serializedObject.FindProperty("availableItems");
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
			if (property.name == "availableItems")
			{
				CustomizeAvailableItemList(availableItems, "Available Items");
				continue;
			}
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
			for (int i = 0; i < serializedProperty.arraySize; i++)
			{
				var item = serializedProperty.GetArrayElementAtIndex(i);
				var isEnabled = item.FindPropertyRelative("isEnabled");
				isEnabled.boolValue = true;
			}
		}

		if (GUILayout.Button("Disable All"))
		{
			for (int i = 0; i < serializedProperty.arraySize; i++)
			{
				var item = serializedProperty.GetArrayElementAtIndex(i);
				var isEnabled = item.FindPropertyRelative("isEnabled");
				isEnabled.boolValue = false;
			}
		}
	}
}
