using System;

namespace DefaultNamespace.Attributes
{
    public class StringValueAttribute : Attribute
    {
        public string Text { get; private set; }
        
        public StringValueAttribute(string text)
        {
            Text = text;
        }
    }

    public static class StringValueExtension
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            return attributes.Length > 0 ? attributes[0].Text : null;
        }
    }
}