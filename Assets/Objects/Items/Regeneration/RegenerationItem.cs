using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;

namespace Objects.Items.Regeneration
{
	public class RegenerationItem : ItemBase
	{
		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Heal1000HealthInOneGame);
		}
	}
}