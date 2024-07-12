using Data.Elements;
using DefaultNamespace.Data.Weapons;
using Interfaces;
using Managers;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Abilities
{
    public class WeaponStatsStrategyBase : IWeaponStatsStrategy
    {
        private readonly WeaponStats _weaponStats;
        private readonly Element _weaponElement;
        
        public WeaponStatsStrategyBase(WeaponStats weaponStats, Element elementField)
        {
            _weaponStats = weaponStats;
            _weaponElement = elementField;
        }
        
        public virtual DamageResult GetDamageDealt(float damageIncrease = 0, float flatDamageIncrease = 0, bool isFollowUp = false)
        {
            if (isFollowUp)
                return GetFollowUpDamageDealt(damageIncrease, flatDamageIncrease);
            
            var damageResult = new DamageResult
            {
                IsCriticalHit = IsCrit()
            };
            
            var nonCritDamage = (GetDamage() + flatDamageIncrease) * GetElementalDamageIncrease() * (GetDamageIncreasePercentage() + damageIncrease);
            damageResult.Damage = damageResult.IsCriticalHit ? nonCritDamage * GetCritDamage()  : nonCritDamage;

            return damageResult;
        }        
        
        protected virtual DamageResult GetFollowUpDamageDealt(float damageIncrease = 0, float flatDamageIncrease = 0)
        {
            var damageResult = new DamageResult
            {
                IsCriticalHit = Random.value < 0.5f
            };
            
            var nonCritDamage = (GetDamage() + flatDamageIncrease) * GetElementalDamageIncrease() * (1f + damageIncrease) * GetFollowUpAttackDamageIncrease();
            damageResult.Damage = damageResult.IsCriticalHit ? nonCritDamage * (GetCritDamage()/2f)  : nonCritDamage;

            return damageResult;
        }

        public virtual float GetTotalCooldown()
        {
            var cooldownValue = GetCooldownReductionPercentage();
            return GetCooldown() * (cooldownValue < 0.1f ? 0.1f : cooldownValue);
        }

        public virtual float GetDuplicateSpawnDelay()
        {
            return _weaponStats.DuplicateSpawnDelay;
        }

        public virtual int GetAttackCount()
        {
            var weaponCooldown = _weaponStats.AttackCount;
            var playerCooldown = PlayerStatsScaler.GetScaler().GetAttackCount();

            return weaponCooldown + playerCooldown;
        }

        public virtual float GetDamageCooldown()
        {
            return _weaponStats.DamageCooldown;
        }

        public virtual float GetDamageOverTime(float damageIncrease = 0)
        {
            return GetDamageOverTime() * (damageIncrease + GetDamageIncreasePercentage());
        }

        protected virtual float GetDamageOverTime()
        {
            var weaponDamage = _weaponStats.DamageOverTime;
            var playerDamage = PlayerStatsScaler.GetScaler().GetDamageOverTime();

            return weaponDamage + playerDamage;
        }

        public virtual float GetDamageOverTimeFrequency()
        {
            var weaponDoTFrequency = _weaponStats.DamageOverTimeFrequency;
            var playerDoTFrequencyReduction = PlayerStatsScaler.GetScaler().GetDamageOverTimeFrequencyReductionPercentage();

            return weaponDoTFrequency * playerDoTFrequencyReduction;
        }

        public virtual float GetDamageOverTimeDuration()
        {
            var weaponDamageOverTimeDuration = _weaponStats.DamageOverTimeDuration;
            var damageOverTimeDurationIncreasePercentage = PlayerStatsScaler.GetScaler().GetDamageOverTimeDurationIncreasePercentage();

            return weaponDamageOverTimeDuration * damageOverTimeDurationIncreasePercentage;
        }

        public virtual float GetWeakness()
        {
            return _weaponStats.Weakness;
        }

        public float GetSpeed()
        {
            var weaponSpeed = _weaponStats.Speed;
            var playerProjectileSpeed = PlayerStatsScaler.GetScaler().GetProjectileSpeed();

            return weaponSpeed + playerProjectileSpeed;
        }

        public virtual float GetCooldown()
        {
            var weaponCooldown = _weaponStats.Cooldown;
            var playerCooldown = PlayerStatsScaler.GetScaler().GetCooldownReduction();

            return weaponCooldown - playerCooldown;
        }

        protected virtual float GetCooldownReductionPercentage()
        {
            var weaponCooldown = _weaponStats.CooldownReduction;
            var playerCooldown = PlayerStatsScaler.GetScaler().GetCooldownReductionPercentage();

            return playerCooldown - weaponCooldown;
        }

        public virtual float GetDamage()
        {
            var weaponDamage = _weaponStats.Damage;
            var playerDamage = PlayerStatsScaler.GetScaler().GetDamage();
            return weaponDamage + playerDamage;
        }

        public virtual float GetScale()
        {
            var weaponScale = _weaponStats.Scale;
            var playerProjectileScale = PlayerStatsScaler.GetScaler().GetScale();
            return weaponScale + playerProjectileScale;
        }

        public virtual float GetTotalTimeToLive()
        {
            return GetTimeToLive() * GetProjectileLifeTimeIncreasePercentage();
        }

        public virtual float GetHealPerHit(bool allowCrit)
        {
            return IsCrit()
                ? _weaponStats.HealPerHit * GetCritDamage()
                : _weaponStats.HealPerHit;
        }

        public virtual double GetDetectionRange()
        {
            var weaponDetectionRange = _weaponStats.DetectionRange;
            var playerDetectionRange = PlayerStatsScaler.GetScaler().GetProjectileDetectionRange();
            return weaponDetectionRange + playerDetectionRange;
        }

        public virtual int GetPassThroughCount()
        {
            var weaponPassCount = _weaponStats.PassThroughCount;
            var playerProjectilePassThroughCount = PlayerStatsScaler.GetScaler().GetProjectilePassThroughCount();
            return weaponPassCount + playerProjectilePassThroughCount;
        }

        public virtual float GetElementalDamageIncrease()
        {
            return PlayerStatsScaler.GetScaler().GetElementalDamageIncrease(_weaponElement);
        }

        public virtual float GetLifeSteal()
        {
            var weaponLifeSteal = _weaponStats.LifeSteal;
            var playerLifeSteal = PlayerStatsScaler.GetScaler().GetLifeSteal();

            return weaponLifeSteal + playerLifeSteal;
        }

        protected virtual float GetTimeToLive()
        {
            var weaponTimeToLive = _weaponStats.TimeToLive;
            var playerProjectileTimeToLive = PlayerStatsScaler.GetScaler().GetProjectileLifeTime();
            return weaponTimeToLive + playerProjectileTimeToLive;
        }

        protected virtual float GetProjectileLifeTimeIncreasePercentage()
        {
            return PlayerStatsScaler.GetScaler().GetProjectileLifeTimeIncreasePercentage();
        }

        protected virtual float GetDamageIncreasePercentage()
        {
            var weaponDamage = _weaponStats.DamageIncreasePercentage;
            var playerDamage = PlayerStatsScaler.GetScaler().GetDamageIncreasePercentage();
            return weaponDamage + playerDamage;
        }

        protected virtual float GetCritDamage()
        {
           var weaponCritDamage = _weaponStats.CritDamage;
           var playerCritDamage = (float)PlayerStatsScaler.GetScaler().GetCritDamage();
           return weaponCritDamage + playerCritDamage;
        }

        protected virtual bool IsCrit()
        { 
            var critValue =  GetCritRate();
            return Random.value < critValue;
        }

        protected virtual float GetCritRate()
        {
            var weaponCritRate = _weaponStats.CritRate;
            var playerCritRate = (float)PlayerStatsScaler.GetScaler().GetCritRate();
            return weaponCritRate + playerCritRate;
        }

        protected virtual float GetFollowUpAttackDamageIncrease()
        {
            var weaponDamage = _weaponStats.FollowUpDamageIncrease;
            var playerDamage = PlayerStatsScaler.GetScaler().GetFollowUpAttackDamageIncrease();
            return weaponDamage + playerDamage;
        }
    }
}