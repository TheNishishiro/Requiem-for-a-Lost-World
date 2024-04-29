using System;

namespace Objects.Players.PermUpgrades
{
    public class StatAttribute : Attribute
    {
        public StatCategory StatCategory;
        public string ShortName;
        public string LongName;

        public StatAttribute(StatCategory statCategory, string shortName, string longName)
        {
            StatCategory = statCategory;
            ShortName = shortName;
            LongName = longName;
        }
    }
	
    public static class StatExtension
    {
        public static StatCategory GetStatType(this StatEnum statTypeEnum)
        {
            var fieldInfo = statTypeEnum.GetType().GetField(statTypeEnum.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(StatAttribute), false) as StatAttribute[];
            return attributes?.Length > 0 ? attributes[0].StatCategory : StatCategory.None;
        }
        public static string GetShortName(this StatEnum statTypeEnum)
        {
            var fieldInfo = statTypeEnum.GetType().GetField(statTypeEnum.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(StatAttribute), false) as StatAttribute[];
            return attributes?.Length > 0 ? attributes[0].ShortName : "";
        }
        public static string GetLongName(this StatEnum statTypeEnum)
        {
            var fieldInfo = statTypeEnum.GetType().GetField(statTypeEnum.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(StatAttribute), false) as StatAttribute[];
            return attributes?.Length > 0 ? attributes[0].LongName : "";
        }
    }
}