using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;

namespace Objects.Items.Armor
{
	public class ArmorItem : ItemBase
	{
		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Take1000DamageInOneGame);
		}
	}
}