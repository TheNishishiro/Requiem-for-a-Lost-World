using Objects.Stage;

namespace Objects.Characters.Truzi
{
    public class TruziScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetDamageIncreasePercentage()
        {
            var scaling = GameData.IsCharacterRank(CharacterRank.E2) ? 3f : 1f;
            
            return base.GetDamageIncreasePercentage() + (PlayerStats.CooldownReductionPercentage * scaling);
        }

        public override float GetCooldownReduction()
        {
            var cdr = base.GetCooldownReduction();
            return cdr > 0.75f && GameData.IsCharacterRank(CharacterRank.E5) ? 0.75f : cdr;
        }

        public override double GetCritDamage()
        {
            var cdrDiff = base.GetCooldownReduction() - 0.75f;
            if (cdrDiff > 0 && GameData.IsCharacterRank(CharacterRank.E5))
                return base.GetCritDamage() + cdrDiff;
            
            return base.GetCritDamage();
        }
    }
}