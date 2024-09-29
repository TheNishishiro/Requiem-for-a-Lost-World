using System;
using DefaultNamespace.Data.Achievements;

namespace DefaultNamespace.Steam.Attributes
{
    public class SteamAchievementAttribute : Attribute
    {
        public string Code { get; private set; }

        public SteamAchievementAttribute(string steamCode)
        {
            Code = steamCode;
        }
    }
    
    public static class SteamAchievementAttributeExtension
    {
        public static string GetSteamCode(this AchievementEnum achievementEnum)
        {
            var fieldInfo = achievementEnum.GetType().GetField(achievementEnum.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(SteamAchievementAttribute), false) as SteamAchievementAttribute[];
            return attributes?.Length > 0 ? attributes[0].Code : null;
        }
    }
}