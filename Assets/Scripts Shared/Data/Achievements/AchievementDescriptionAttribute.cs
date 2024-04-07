using System;
using System.Collections.Generic;
using Objects.Characters;

namespace DefaultNamespace.Data.Achievements
{
	public class AchievementValueAttribute : Attribute
	{
		public string Description { get; protected set; }
		public string Title { get; protected set; }
		public AchievementSection Section { get; private set; }
		public CharactersEnum Character { get; private set; }
		public Rarity Rarity { get; private set; }
		public KeyValuePair<RewardType, float> Reward { get; private set; }
		public KeyValuePair<RequirementType, float> Requirement { get; private set; }
		
		public AchievementValueAttribute(string title, string description, AchievementSection section, Rarity rarity)
		{
			Title = title;
			Description = description;
			Section = section;
			Rarity = rarity;
			Reward = new KeyValuePair<RewardType, float>(RewardType.None, 200);
			Requirement = new KeyValuePair<RequirementType, float>(RequirementType.None, 1);
		}		
		
		public AchievementValueAttribute(string title, string description, AchievementSection section, Rarity rarity, RequirementType requirementType, float requirementQuantity)
		{
			Title = title;
			Description = description;
			Section = section;
			Rarity = rarity;
			Reward = new KeyValuePair<RewardType, float>(RewardType.None, 200);
			Requirement = new KeyValuePair<RequirementType, float>(requirementType, requirementQuantity);
		}			
		
		public AchievementValueAttribute(string title, string description, AchievementSection section, Rarity rarity, CharactersEnum character, RequirementType requirementType, float requirementQuantity)
		{
			Title = title;
			Description = description;
			Section = section;
			Character = character;
			Rarity = rarity;
			Reward = new KeyValuePair<RewardType, float>(RewardType.None, 200);
			Requirement = new KeyValuePair<RequirementType, float>(requirementType, requirementQuantity);
		}	
		
		public AchievementValueAttribute(string title, string description, AchievementSection section, Rarity rarity, RewardType rewardType, float rewardQuantity, RequirementType requirementType, float requirementQuantity)
		{
			Title = title;
			Description = description;
			Section = section;
			Rarity = rarity;
			Reward = new KeyValuePair<RewardType, float>(rewardType, rewardQuantity);
			Requirement = new KeyValuePair<RequirementType, float>(requirementType, requirementQuantity);
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
		
		public static string GetTitle(this AchievementEnum achievementEnum)
		{
			var fieldInfo = achievementEnum.GetType().GetField(achievementEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(AchievementValueAttribute), false) as AchievementValueAttribute[];
			return attributes?.Length > 0 ? attributes[0].Title : null;
		}
		
		public static AchievementValueAttribute GetAchievementValue(this AchievementEnum achievementEnum)
		{
			var fieldInfo = achievementEnum.GetType().GetField(achievementEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(AchievementValueAttribute), false) as AchievementValueAttribute[];
			return attributes?.Length > 0 ? attributes[0] : null;
		}
	}
}