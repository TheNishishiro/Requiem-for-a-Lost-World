using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;

namespace DefaultNamespace.Data.Locale
{
    [CreateAssetMenu(fileName = "Translation Module", menuName = "Language Manager/Translation Module")]
    public class TranslationModule : ScriptableObject
    {
        public List<LocalizedText> LocalizedTexts;
    }
}