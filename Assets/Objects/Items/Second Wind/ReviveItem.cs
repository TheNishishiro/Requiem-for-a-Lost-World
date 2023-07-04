using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;

namespace Objects.Items.Second_Wind
{
	public class ReviveItem : ItemBase
	{
		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Die20Times);
		}
	}
}