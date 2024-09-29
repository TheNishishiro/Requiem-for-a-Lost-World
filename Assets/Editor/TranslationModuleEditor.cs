using UnityEditor;
using UnityEngine;
using DefaultNamespace.Data.Locale;
using NaughtyAttributes.Editor;
using System.Collections.Generic;
using System.Reflection;

public class TranslationModuleEditor : EditorWindow
{
    private TranslationModule translationModule;
    private Vector2 scrollPos;
    private SerializedObject serializedTranslationModule;
    private SerializedProperty localizedTextsProperty;
    private HashSet<string> englishTextSet;
    private List<string> languageFields;
    private bool scrollToEnd = false;

    [MenuItem("Window/Translation Module Editor")]
    public static void ShowWindow()
    {
        GetWindow<TranslationModuleEditor>("Translation Module Editor");
    }

    private void OnEnable()
    {
        // Initialize language fields when script is enabled
        languageFields = GetLanguageFields();
    }

    private void OnGUI()
    {
        GUILayout.Label("Translation Module Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        translationModule = (TranslationModule)EditorGUILayout.ObjectField("Translation Module", translationModule, typeof(TranslationModule), false);

        if (translationModule != null)
        {
            if (serializedTranslationModule == null || serializedTranslationModule.targetObject != translationModule)
            {
                serializedTranslationModule = new SerializedObject(translationModule);
                localizedTextsProperty = serializedTranslationModule.FindProperty("LocalizedTexts");
            }

            serializedTranslationModule.Update();
            englishTextSet = new HashSet<string>();

            if (scrollToEnd)
            {
                scrollPos.y = Mathf.Infinity;
                scrollToEnd = false;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < localizedTextsProperty.arraySize; i++)
            {
                SerializedProperty localizedTextProperty = localizedTextsProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginVertical("box");

                string englishText = localizedTextProperty.FindPropertyRelative("englishText").stringValue;
                string labelTitle = GetCleanedAndTrimmedText(englishText, 25);

                EditorGUILayout.LabelField(labelTitle, EditorStyles.boldLabel);

                foreach (var fieldName in languageFields)
                {
                    var languageTextProperty = localizedTextProperty.FindPropertyRelative(fieldName);
                    NaughtyEditorGUI.PropertyField_Layout(languageTextProperty, true);
                }

                if (englishTextSet.Contains(englishText))
                {
                    EditorGUILayout.HelpBox("Duplicate English Text found!", MessageType.Warning);
                }
                else
                {
                    englishTextSet.Add(englishText);
                }

                if (GUILayout.Button("Remove"))
                {
                    localizedTextsProperty.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Add Localized Text"))
            {
                InsertNewLocalizedText();
                scrollToEnd = true;  // Set flag to scroll to end on next GUI update
            }

            serializedTranslationModule.ApplyModifiedProperties();
        }
    }

    private void InsertNewLocalizedText()
    {
        localizedTextsProperty.InsertArrayElementAtIndex(localizedTextsProperty.arraySize);
        SerializedProperty newElement = localizedTextsProperty.GetArrayElementAtIndex(localizedTextsProperty.arraySize - 1);

        foreach (var fieldName in languageFields)
        {
            newElement.FindPropertyRelative(fieldName).stringValue = string.Empty;
        }
    }

    private string GetCleanedAndTrimmedText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "Empty";
        string cleanedText = text.Replace("\r", " ").Replace("\n", " ");
        if (cleanedText.Length <= maxLength) return cleanedText;
        return cleanedText.Substring(0, maxLength) + "...";
    }

    private List<string> GetLanguageFields()
    {
        var fields = new List<string>();
        var localizedTextFields = typeof(LocalizedText).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in localizedTextFields)
        {
            if (field.FieldType == typeof(string))
            {
                fields.Add(field.Name);
            }
        }

        return fields;
    }
}