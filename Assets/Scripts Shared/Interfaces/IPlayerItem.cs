using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Weapons;
using Objects.Items;
using UnityEngine;

namespace Interfaces
{
	public interface IPlayerItem
	{
		string NameField { get; }
		string DescriptionField { get; }
		Sprite IconField { get; }
		int LevelField { get; }
		Element ElementField { get; }
		AttackType AttackTypeField { get; }
		bool IsItem { get; }
		AchievementEnum? RequiredAchievementField { get; }
		bool IsUnlocked(SaveFile saveFile);
		bool ReliesOnAchievement(AchievementEnum achievement);
		string GetDescription(int rarity);
	}
}