using Objects.Players.Scripts;
using Weapons;

namespace Objects.Abilities.Lightning_Needle
{
    public class LightningNeedleStrategy : WeaponStatsStrategyBase
    {
        private LightningNeedleWeapon _lightningNeedleWeapon;
        
        public LightningNeedleStrategy(WeaponBase weapon) : base(weapon.weaponStats, weapon.ElementField)
        {
            _lightningNeedleWeapon = (LightningNeedleWeapon)weapon;
        }

        protected override float GetDamageOverTime()
        {
            var damageIncrease = _lightningNeedleWeapon.StormSurge ? 1 - GetCooldownReductionPercentage() : 0;
            return GetDamageOverTime() * (1 + damageIncrease);
        }
    }
}