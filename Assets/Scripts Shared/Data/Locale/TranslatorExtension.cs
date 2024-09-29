using Managers;

namespace DefaultNamespace.Data.Locale
{
    public static class TranslatorExtension
    {
        private static bool IsLanguageManagerActive;
        
        public static string Translate(this string text)
        {
            return text;
            
            if (string.IsNullOrWhiteSpace(text))
                return text;
            
            if (!IsLanguageManagerActive)
                IsLanguageManagerActive = LanguageManager.instance != null;
            return IsLanguageManagerActive ? LanguageManager.instance.Translate(text) : text;
        }
    }
}