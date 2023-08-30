using DefaultNamespace.Data;
using DefaultNamespace.Editors;
using UnityEditor;

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
				ContainerUtility.CustomizeAvailableItemList(availableItems, "Available Items");
				continue;
			}

			EditorGUILayout.PropertyField(property, true);
		}

		
		serializedObject.ApplyModifiedProperties();
	}
}
