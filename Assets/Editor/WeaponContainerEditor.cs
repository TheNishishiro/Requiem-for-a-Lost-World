using System.Collections.Generic;
using Data.ToggleableEntries;
using DefaultNamespace.Data;
using DefaultNamespace.Editors;
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
				ContainerUtility.CustomizeAvailableItemList(availableWeapons, "Available Weapons");
				continue;
			}

			EditorGUILayout.PropertyField(property, true);
		}

		
		serializedObject.ApplyModifiedProperties();
	}
}
