using Managers;
using Objects.Stage;

namespace Objects.Characters.Yami_no_Tokiya
{
    public class YamiScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetFollowUpAttackDamageIncrease()
        {
            var increase = GameData.IsCharacterRank(CharacterRank.E5) ? 2 : 1f;
            return base.GetFollowUpAttackDamageIncrease() + SpecialBarManager.instance.GetPercentageFulfillment() * increase;
        }

        public override float GetDamageIncreasePercentage()
        {
            if (GameData.IsCharacterRank(CharacterRank.E5))
                return base.GetDamageIncreasePercentage() + SpecialBarManager.instance.GetPercentageFulfillment();
            
            return base.GetDamageIncreasePercentage();
        }

        public override void TakeDamage(float amount, bool isPreventDeath = false)
        {
            var isShieldInsufficient = GetSpecialValue() < amount;
            var damageToTake = isShieldInsufficient ? amount - GetSpecialValue() : 0;
            base.TakeDamage(damageToTake, isPreventDeath);
            PlayerStats.SpecialValue = isShieldInsufficient ? 0 : PlayerStats.SpecialValue - amount;
        }
    }
}