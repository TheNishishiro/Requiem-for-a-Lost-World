using Interfaces;
using Managers;
using Objects.Players.PermUpgrades;
using Objects.Stage;

namespace Objects.Characters.David
{
    public class DavidScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetDamageIncreasePercentage()
        {
            var multiplier = GameData.IsCharacterRank(CharacterRank.E3) ? 0.01f : 0.02f;
            return 1 + (PlayerStats?.DamagePercentageIncrease ?? 0) + ((GetMaxHealth() - GetHealth())*multiplier);
        }
    }
}