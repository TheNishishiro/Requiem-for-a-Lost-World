using Objects.Players.Scripts;
using Weapons;

namespace Objects.Abilities.Pillar_Of_Despair
{
    public class PillarOfDespairStrategy : WeaponStatsStrategyBase
    {
        private PillarOfDespairWeapon _pillarOfDespairWeapon;
        
        public PillarOfDespairStrategy(WeaponBase weapon) : base(weapon.weaponStats)
        {
            _pillarOfDespairWeapon = (PillarOfDespairWeapon)weapon;
        }

        protected override float GetDamageIncreasePercentage()
        {
            var increase = _pillarOfDespairWeapon.VoidImpact2 ? 1.5f : 1;
            return base.GetDamageIncreasePercentage() + (PlayerStatsScaler.GetScaler().GetMissingHealthPercentage() * increase);
        }

        public override float GetDamage()
        {
            if (_pillarOfDespairWeapon.VoidImpact1)
                return base.GetDamage() + (PlayerStatsScaler.GetScaler().GetMissingHealth() * 0.1f);
            return base.GetDamage();
        }
    }
}