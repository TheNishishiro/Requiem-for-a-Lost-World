using Interfaces;
using Objects.Players;

namespace Objects.Characters.Corina_Alter
{
	public class CorinaStrategy : ICharacterStrategy
	{
		public void Apply(PlayerStats stats, CharacterRank characterRank)
		{
			if (characterRank >= CharacterRank.E1)
			{
				stats.MagnetSize += 0.2f;
				stats.DamageOverTime += stats.DamageOverTime * 0.3f;
			}
			if (characterRank >= CharacterRank.E3)
			{
				stats.CritDamage += 0.5f;
				stats.HealthMax -= 20;
			}
			if (characterRank >= CharacterRank.E4)
			{
				stats.Scale += 0.2f;
			}
			if (characterRank >= CharacterRank.E5)
			{
				stats.DamagePercentageIncrease += 0.5f;
				stats.MagnetSize += 0.2f;
			}
		}
	}
}