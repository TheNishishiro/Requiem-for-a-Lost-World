using Data.Elements;
using Weapons;

namespace Objects.Abilities.Reality_Crack
{
    public class RealityShatterStrategy : WeaponStatsStrategyBase
    {
        public RealityShatterStrategy(WeaponBase weapon) : base(weapon.weaponStats, weapon.ElementField)
        {
        }

        public override float GetTotalTimeToLive()
        {
            return 2f;
        }
    }
}