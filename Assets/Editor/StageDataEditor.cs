using System;
                                                   using System.Collections.Generic;
using DefaultNamespace;
using Managers.StageEvents;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        var itemsProperty = serializedObject.FindProperty("stageEvents");

        reorderableList = new ReorderableList(serializedObject, itemsProperty, true, true, true, true)
            {
                drawHeaderCallback = rect => {
                    EditorGUI.LabelField(rect, "Events:");
                },
                drawElementCallback = (rect, index, isActive, isFocused) => {
                    var elementProperty = itemsProperty.GetArrayElementAtIndex(index);

                    var triggerTimeProperty = elementProperty.FindPropertyRelative("triggerTime");
                    var eventTimeSpan = Utilities.FloatToTimeString(triggerTimeProperty.floatValue);
                    var eventName = $"{eventTimeSpan}";

                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width - 100, EditorGUIUtility.singleLineHeight),
                        elementProperty, new GUIContent(eventName), true);
        
                    if (GUI.Button(
                            new Rect(rect.x + rect.width - 100, rect.y, 100, EditorGUIUtility.singleLineHeight),
                            "Delete Event"))
                    {
                        itemsProperty.DeleteArrayElementAtIndex(index);
                    }
                },
                elementHeightCallback = (index) => EditorGUI.GetPropertyHeight(itemsProperty.GetArrayElementAtIndex(index)) + 5
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        reorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}