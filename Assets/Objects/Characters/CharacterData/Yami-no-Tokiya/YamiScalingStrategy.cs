using Managers;

namespace Objects.Characters.Yami_no_Tokiya
{
    public class YamiScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetFollowUpAttackDamageIncrease()
        {
            return base.GetFollowUpAttackDamageIncrease() + SpecialBarManager.instance.GetPercentageFulfillment();
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