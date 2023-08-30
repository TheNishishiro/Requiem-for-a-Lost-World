using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editors
{
	public class ContainerUtility
	{
		public static void CustomizeAvailableItemList(SerializedProperty serializedProperty, string title)
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
}