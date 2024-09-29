using System;
using System.Globalization;
using NaughtyAttributes;

namespace DefaultNamespace.Data.Locale
{
    [Serializable]
    public class LocalizedText
    {
        [ResizableTextArea]
        public string englishText;
        [ResizableTextArea]
        public string polishText;

        public string GetByLocale()
        {
            return GetByLocale(CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
        }
        
        public string GetByLocale(string locale)
        {
            if (locale == "pl" && !string.IsNullOrWhiteSpace(polishText))
                return polishText.Trim();
            return englishText.Trim();
        }
    }
}