using System;

namespace DefaultNamespace.Data.Achievements
{
	public class AchievementValueAttribute : Attribute
	{
		public string Description { get; protected set; }
		
		public AchievementValueAttribute(string description)
		{
			Description = description;
		}
	}
	
	public static class AchievementValueExtension
	{
		public static string GetDescription(this AchievementEnum achievementEnum)
		{
			var fieldInfo = achievementEnum.GetType().GetField(achievementEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(AchievementValueAttribute), false) as AchievementValueAttribute[];
			return attributes?.Length > 0 ? attributes[0].Description : null;
		}
	}
}