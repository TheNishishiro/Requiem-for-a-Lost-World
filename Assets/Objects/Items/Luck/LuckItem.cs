using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;

namespace Objects.Items.Luck
{
	public class LuckItem : ItemBase
	{
		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Obtain10HighRarityItems);
		}
	}
}