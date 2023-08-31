using DefaultNamespace.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainer))]
public class ItemContainerEditor : Editor
{
	private SerializedProperty availableItems;

	private void OnEnable()
	{
		availableItems = serializedObject.FindProperty("availableItems");
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
