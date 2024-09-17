using DefaultNamespace.Data.Weapons;
using Interfaces;
using Objects.Abilities;
using UnityEngine;

namespace Objects.Enemies.EnemyWeapons
{
    public class EnemyWeaponStrategy : IWeaponStatsStrategy
    {
        private readonly WeaponStats _weaponStats;

        public EnemyWeaponStrategy(WeaponStats weaponStats)
        {
            _weaponStats = weaponStats;
        }
        
        public DamageResult GetDamageDealt(float damageIncrease = 0, float flatDamageIncrease = 0, bool isFollowUp = false)
        {
            var damageResult = new DamageResult
            {
                IsCriticalHit = IsCrit()
            };
            
            var nonCritDamage = GetDamage() + flatDamageIncrease;
            damageResult.Damage = damageResult.IsCriticalHit ? nonCritDamage * (1f + _weaponStats.CritDamage)  : nonCritDamage;

            return damageResult;
        }

        protected virtual bool IsCrit()
        { 
            return Random.value < _weaponStats.CritRate;
        }

        public float GetTotalCooldown()
        {
            return _weaponStats.Cooldown;
        }

        public float GetDuplicateSpawnDelay()
        {
            return _weaponStats.DuplicateSpawnDelay;
        }

        public int GetAttackCount()
        {
            return _weaponStats.AttackCount;
        }

        public float GetDamageCooldown()
        {
            return _weaponStats.DamageCooldown;
        }

        public float GetDamageOverTime(float damageIncrease = 0)
        {
            return _weaponStats.DamageOverTime * (1 + damageIncrease);
        }

        public float GetDamageOverTimeFrequency()
        {
            return _weaponStats.DamageOverTimeFrequency;
        }

        public float GetDamageOverTimeDuration()
        {
            return _weaponStats.DamageOverTimeDuration;
        }

        public float GetWeakness()
        {
            return _weaponStats.Weakness;
        }

        public float GetSpeed()
        {
            return _weaponStats.Speed;
        }

        public float GetDamage()
        {
            return _weaponStats.Damage;
        }

        public float GetScale()
        {
            return _weaponStats.Scale;
        }

        public float GetTotalTimeToLive()
        {
            return _weaponStats.TimeToLive;
        }

        public float GetHealPerHit(bool allowCrit)
        {
            return IsCrit() ? _weaponStats.HealPerHit : _weaponStats.HealPerHit * (1 + _weaponStats.CritDamage);
        }

        public double GetDetectionRange()
        {
            return _weaponStats.DetectionRange;
        }

        public int GetPassThroughCount()
        {
            return _weaponStats.PassThroughCount;
        }

        public float GetLifeSteal()
        {
            return _weaponStats.LifeSteal;
        }

        public float GetResPen()
        {
            return _weaponStats.ResPen;
        }

        public DamageResult GetSplitDamageDealt(float damageIncrease = 0, float flatDamageIncrease = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}