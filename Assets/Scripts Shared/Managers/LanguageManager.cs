using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DefaultNamespace.Data.Locale;
using Events.Scripts;
using UnityEngine;

namespace Managers
{
    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager instance;
        [SerializeField] private List<TranslationModule> translationModules;
        private Dictionary<string, LocalizedText> translationModulesDictionary;

        public void Start()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            translationModulesDictionary = translationModules.SelectMany(x => x.LocalizedTexts).ToDictionary(x => x.englishText.Trim(), x => x);
            instance = this;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            ChangeLanguage();
        }

        private void ChangeLanguage()
        {
            TranslationChangedEvent.Invoke();
        }

        public string Translate(string text)
        {
            var searchText = text.Trim();
            if (translationModulesDictionary.ContainsKey(searchText))
                return translationModulesDictionary[searchText].GetByLocale();
            
            Debug.Log($"Could not find translation for: '{searchText}'");
            return text;
        }
    }
}