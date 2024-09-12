using Objects.Stage;

namespace Objects.Characters.Adam
{
    public class AdamScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetDamageIncreasePercentage()
        {
            var perEnemyDamageIncrease = EnemyManager.instance.currentEnemyCount * (GameData.IsCharacterRank(CharacterRank.E4) ? 0.005f : 0.0025f);
            return base.GetDamageIncreasePercentage() + perEnemyDamageIncrease;
        }

        public override float GetDamageTakenIncrease()
        {
            var perEnemyDamageIncrease = EnemyManager.instance.currentEnemyCount * 0.0025f;
            return base.GetDamageTakenIncrease() + perEnemyDamageIncrease;
        }
    }
}