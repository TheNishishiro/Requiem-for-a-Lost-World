using DefaultNamespace;
using Managers.StageEvents;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private ReorderableList reorderableList;
    private int _indexToDelete = -1;

    private void OnEnable()
    {
        SerializedProperty itemsProperty = serializedObject.FindProperty("stageEvents");

        reorderableList = new ReorderableList(serializedObject, itemsProperty, true, true, true, true);

        reorderableList.drawHeaderCallback = (rect) => {
            EditorGUI.LabelField(rect, "TwitchEvent:");
        };

        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => 
        {
            if (GUI.Button(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Delete Event"))
            {
                _indexToDelete = index;
            }

            SerializedProperty elementProperty = itemsProperty.GetArrayElementAtIndex(index);
            var triggerTimeProperty = elementProperty.FindPropertyRelative("triggerTime");
            var eventTimeSpan = Utilities.FloatToTimeString(triggerTimeProperty.floatValue);
            var eventName = $"{eventTimeSpan}";

            EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y, rect.width - 100, EditorGUIUtility.singleLineHeight), 
                                    elementProperty, new GUIContent(eventName), true);
        
        };

        reorderableList.elementHeightCallback = index => 
        {
            if (_indexToDelete == index) {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(itemsProperty.GetArrayElementAtIndex(index)) + 4;
        };

        reorderableList.onReorderCallbackWithDetails = (_, __, ___) => 
        {
            if (_indexToDelete != -1)
            {
                itemsProperty.DeleteArrayElementAtIndex(_indexToDelete);
                _indexToDelete = -1;
                serializedObject.ApplyModifiedProperties();
                Repaint();
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();

        if (_indexToDelete != -1)
        {
            SerializedProperty itemsProperty = serializedObject.FindProperty("stageEvents");
            itemsProperty.DeleteArrayElementAtIndex(_indexToDelete);
            _indexToDelete = -1;
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }
}