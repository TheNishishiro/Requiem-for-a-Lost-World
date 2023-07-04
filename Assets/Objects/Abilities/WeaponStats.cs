using System;
using System.Text;
using DefaultNamespace.Extensions;
using Objects.Players.Scripts;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Objects.Abilities
{
	[Serializable]
	public class WeaponStats
	{
		private PlayerStatsComponent _playerStatsComponent;
		private bool _isInitialized;
		public float Damage;
		public float Cooldown;
		public float CooldownReduction;
		public float Scale;
		public float Speed;
		public float TimeToLive;
		public int AttackCount;
		public int PassThroughCount;
		public float DamageCooldown;
		public float DuplicateSpawnDelay = 0.2f;
		public float DetectionRange;
		public float CritRate;
		public float CritDamage;
		public float Weakness;
		public float DamageIncreasePercentage;
		public float HealPerHit;
		public float DamageOverTime;
		public float DamageOverTimeDuration;
		public float DamageOverTimeFrequency;

		public void AssignPlayerStatsComponent(PlayerStatsComponent playerStatsComponent)
		{
			_playerStatsComponent = playerStatsComponent;
			_isInitialized = _playerStatsComponent != null; 
		}

		public void ApplyRarity(int rarity)
         {
             var rarityFactor = 1 + ((rarity - 1) * 0.1f); // This will result in an increase from no increase (rarity 1) to 50% (rarity 5)
         
             Damage *= rarityFactor;
             Cooldown *= 2 - rarityFactor;
             CooldownReduction *= rarityFactor;
             Scale *= rarityFactor;
             Speed *= rarityFactor;
             TimeToLive *= rarityFactor;
             DamageCooldown *= rarityFactor;
             DuplicateSpawnDelay *= rarityFactor;
             CritRate *= rarityFactor;
             CritDamage *= rarityFactor;
             Weakness *= rarityFactor;
             DamageIncreasePercentage *= rarityFactor;
             HealPerHit *= rarityFactor;
             DamageOverTime *= rarityFactor;
             DamageOverTimeDuration *= rarityFactor;
             DamageOverTimeFrequency *= 2 - rarityFactor;
         }
		
		public void Sum(WeaponStats weaponStats, int rarity)
        {
            var rarityFactor = 1 + ((rarity - 1) * 0.1f);
              
            AttackCount += weaponStats.AttackCount;
            Damage += weaponStats.Damage * rarityFactor;
            Cooldown += weaponStats.Cooldown * rarityFactor;
            CooldownReduction += weaponStats.CooldownReduction * rarityFactor;
            Scale += weaponStats.Scale * rarityFactor;
            Speed += weaponStats.Speed * rarityFactor;
            TimeToLive += weaponStats.TimeToLive * rarityFactor;
            PassThroughCount += weaponStats.PassThroughCount; 
            DamageCooldown += weaponStats.DamageCooldown * rarityFactor;
            DuplicateSpawnDelay += weaponStats.DuplicateSpawnDelay * rarityFactor;
            CritRate += weaponStats.CritRate * rarityFactor;
            CritDamage += weaponStats.CritDamage * rarityFactor;
            Weakness += weaponStats.Weakness * rarityFactor;
            DamageIncreasePercentage += weaponStats.DamageIncreasePercentage * rarityFactor;      
            HealPerHit += weaponStats.HealPerHit * rarityFactor;      
            DamageOverTime += weaponStats.DamageOverTime * rarityFactor;      
            DamageOverTimeDuration += weaponStats.DamageOverTimeDuration * rarityFactor;      
            DamageOverTimeFrequency += weaponStats.DamageOverTimeFrequency * rarityFactor;      
        }

		public string GetDescription(int rarity)
        {
            var builder = new StringBuilder();
        
            builder.AppendStat("Damage", Damage, rarity);
            builder.AppendStat("Damage%", DamageIncreasePercentage*100, rarity, "%");
            builder.AppendStat("Cooldown", Cooldown, rarity, "sec");
            builder.AppendStat("Cooldown%", CooldownReduction*100, rarity, "%");
            builder.AppendStat("Scale", Scale, rarity);
            builder.AppendStat("Speed", Speed, rarity, "m/s");
            builder.AppendStat("Projectile time", TimeToLive, rarity, "sec");
            builder.AppendStat("Hit count", PassThroughCount, rarity);
            builder.AppendStat("Damage delay", DamageCooldown, rarity, "sec");
            builder.AppendStat("Attack delay", DuplicateSpawnDelay, rarity, "sec");
            builder.AppendStat("Critical rate", CritRate*100, rarity, "%");
            builder.AppendStat("Critical damage", CritDamage*100, rarity, "%");
            builder.AppendStat("Weakness", Weakness*100, rarity, "%");
            builder.AppendStat("Attack count", AttackCount, rarity);
            builder.AppendStat("Regen per hit", HealPerHit, rarity);
            builder.AppendStat("Damage over time", DamageOverTime, rarity);
            builder.AppendStat("DoT duration", DamageOverTimeDuration, rarity);
            builder.AppendStat("DoT frequency", DamageOverTimeFrequency, rarity);
        
            return builder.ToString();
        }

		public float GetCooldown()
		{
			if (!_isInitialized) return Cooldown * (1 + CooldownReduction);

			var calculatedCooldown = (Cooldown - _playerStatsComponent.GetCooldownReduction()) * (_playerStatsComponent.GetCooldownReductionPercentage() + CooldownReduction);
			return calculatedCooldown <= 0 ? 0.01f : calculatedCooldown;
		}
		
		public int GetAttackCount()
		{
			if (!_isInitialized) return AttackCount;
			return AttackCount + _playerStatsComponent.GetAttackCount();
		}
		

		public float GetDamage()
		{
			if (!_isInitialized) 
				return (int) ((Random.value < CritRate ? Damage * CritDamage : Damage) * (1 + DamageIncreasePercentage));

			var damage = (Damage + _playerStatsComponent.GetDamage());
			var critRate = CritRate + _playerStatsComponent.GetCritRate();
			var critDamage = CritDamage + _playerStatsComponent.GetCritDamage();
			return (float)(Random.value < critRate ? damage * critDamage : damage) * (_playerStatsComponent.GetDamageIncreasePercentage() + DamageIncreasePercentage);
		}

		public float GetDamageOverTime()
		{
			if (!_isInitialized) return DamageOverTime * DamageIncreasePercentage;
			return (DamageOverTime + _playerStatsComponent.GetDamageOverTime()) * (_playerStatsComponent.GetDamageIncreasePercentage() + DamageIncreasePercentage);
		}

		public float GetDamageIncreasePercentage()
		{
			if (!_isInitialized) return DamageIncreasePercentage;
			return DamageIncreasePercentage + _playerStatsComponent.GetDamageIncreasePercentage();
		}
		
		public float GetScale()
		{
			if (!_isInitialized) return Scale;
			return Scale + _playerStatsComponent.GetScale();
		}
		
		public float GetSpeed()
		{
			if (!_isInitialized) return Speed;
			return Speed + _playerStatsComponent.GetProjectileSpeed();
		}
		
		public float GetTimeToLive()
		{
			if (!_isInitialized) return TimeToLive;
			return (TimeToLive + _playerStatsComponent.GetProjectileLifeTime()) * _playerStatsComponent.GetProjectileLifeTimeIncreasePercentage();
		}
		
		public float GetDetectionRange()
		{
			if (!_isInitialized) return DetectionRange;
			return DetectionRange + _playerStatsComponent.GetProjectileDetectionRange();
		}

		public int GetPassThroughCount()
		{
			if (!_isInitialized) return PassThroughCount;
			return PassThroughCount + _playerStatsComponent.GetProjectilePassThroughCount();
		}
		
		public float GetWeakness()
		{
			return Weakness;
		}
	}
}