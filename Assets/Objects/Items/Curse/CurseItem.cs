using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;

namespace Objects.Items.Curse
{
	public class CurseItem : ItemBase
	{
		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Kill100000Enemies);
		}
	}
}