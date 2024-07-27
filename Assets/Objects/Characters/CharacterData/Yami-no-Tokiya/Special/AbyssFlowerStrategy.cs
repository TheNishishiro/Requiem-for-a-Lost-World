using Objects.Abilities;
using Weapons;

namespace Objects.Characters.Yami_no_Tokiya.Special
{
    public class AbyssFlowerStrategy : WeaponStatsStrategyBase
    {
        public AbyssFlowerStrategy(WeaponBase weapon) : base(weapon.weaponStats, weapon.ElementField)
        {
        }

        protected override float GetProjectileLifeTimeIncreasePercentage()
        {
            return 1f;
        }
    }
}