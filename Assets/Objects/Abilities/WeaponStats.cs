using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using DefaultNamespace;
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
	         var rarityFactor = GetRarityFactor(rarity);
         
             Damage *= rarityFactor;
             Cooldown *= 2 - rarityFactor;
             CooldownReduction *= rarityFactor;
             Scale *= rarityFactor;
             Speed *= rarityFactor;
             TimeToLive *= rarityFactor;
             DamageCooldown *= rarityFactor;
             DuplicateSpawnDelay *= rarityFactor;
             DetectionRange *= rarityFactor;
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
	        var rarityFactor = GetRarityFactor(rarity);
              
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
            DetectionRange += weaponStats.DetectionRange * rarityFactor;
            CritRate += weaponStats.CritRate * rarityFactor;
            CritDamage += weaponStats.CritDamage * rarityFactor;
            Weakness += weaponStats.Weakness * rarityFactor;
            DamageIncreasePercentage += weaponStats.DamageIncreasePercentage * rarityFactor;      
            HealPerHit += weaponStats.HealPerHit * rarityFactor;      
            DamageOverTime += weaponStats.DamageOverTime * rarityFactor;      
            DamageOverTimeDuration += weaponStats.DamageOverTimeDuration * rarityFactor;      
            DamageOverTimeFrequency += weaponStats.DamageOverTimeFrequency * (2 - rarityFactor);      
        }

		public string GetDescription(string description, int rarity)
		{
			var rarityFactor = GetRarityFactor(rarity);

			return description
				.Replace("{AttackCount}", Utilities.StatToString(AttackCount))
				.Replace("{Damage}", Utilities.StatToString(Damage, rarityFactor))
				.Replace("{Cooldown}", Utilities.StatToString(Cooldown, rarityFactor))
				.Replace("{CooldownReduction}", Utilities.StatToString(CooldownReduction, rarityFactor, true))
				.Replace("{Scale}", Utilities.StatToString(Scale, rarityFactor, true))
				.Replace("{Speed}", Utilities.StatToString(Speed, rarityFactor))
				.Replace("{TimeToLive}", Utilities.StatToString(TimeToLive, rarityFactor))
				.Replace("{PassThroughCount}", Utilities.StatToString(PassThroughCount))
				.Replace("{DamageCooldown}", Utilities.StatToString(DamageCooldown, rarityFactor))
				.Replace("{DuplicateSpawnDelay}", Utilities.StatToString(DuplicateSpawnDelay, rarityFactor))
				.Replace("{DetectionRange}", Utilities.StatToString(DetectionRange, rarityFactor))
				.Replace("{CritRate}", Utilities.StatToString(CritRate, rarityFactor, true))
				.Replace("{CritDamage}", Utilities.StatToString(CritDamage, rarityFactor, true))
				.Replace("{Weakness}", Utilities.StatToString(Weakness, rarityFactor, true))
				.Replace("{DamageIncreasePercentage}", Utilities.StatToString(DamageIncreasePercentage, rarityFactor, true))
				.Replace("{HealPerHit}", Utilities.StatToString(HealPerHit, rarityFactor))
				.Replace("{DamageOverTime}", Utilities.StatToString(DamageOverTime, rarityFactor))
				.Replace("{DamageOverTimeDuration}", Utilities.StatToString(DamageOverTimeDuration, rarityFactor))
				.Replace("{DamageOverTimeFrequency}", Utilities.StatToString(DamageOverTimeFrequency, rarityFactor, false, true))
				;
		}

		public ICollection<StatsDisplayData> GetStatsDisplayData()
        {
            var stats = new List<StatsDisplayData>
            {
	            new("Damage", Damage),
	            new("Damage%", DamageIncreasePercentage, isPercentage: true),
	            new("Cooldown", Cooldown),
	            new("Cooldown%", CooldownReduction, isPercentage: true),
	            new("Scale", Scale),
	            new("Speed", Speed),
	            new("Projectile time", TimeToLive),
	            new("Pierce", PassThroughCount),
	            new("Damage delay", DamageCooldown),
	            new("Attack delay", DuplicateSpawnDelay, baseValue:0.2f),
	            new("Critical rate", CritRate, isPercentage: true),
	            new("Critical damage", CritDamage, isPercentage: true),
	            new("Weakness", Weakness, isPercentage: true),
	            new("Attack count", AttackCount),
	            new("Regen per hit", HealPerHit),
	            new("Damage over time", DamageOverTime),
	            new("DoT duration", DamageOverTimeDuration),
	            new("DoT frequency", DamageOverTimeFrequency)
            };
        
            return stats;
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

		public float GetDamageOverTimeFrequency()
		{
			if (!_isInitialized) return DamageOverTimeFrequency;
			return DamageOverTimeFrequency * _playerStatsComponent.GetDamageOverTimeFrequencyReductionPercentage();
		}

		public float GetDamageOverTimeDuration()
		{
			if (!_isInitialized) return DamageOverTimeDuration;
			return DamageOverTimeDuration * _playerStatsComponent.GetDamageOverTimeDurationIncreasePercentage();
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

		private float GetRarityFactor(float rarity)
		{
			const float percentIncreasePerRarity = 0.025f;
			return 1.0f + ((rarity - 1.0f) * percentIncreasePerRarity);
		}

		public float GetHealPerHit(bool allowCrit)
		{
			if (!allowCrit) return HealPerHit;
			
			var critRate = CritRate + _playerStatsComponent.GetCritRate();
			var critDamage = CritDamage + _playerStatsComponent.GetCritDamage();
			return (float)(Random.value < critRate ? HealPerHit * critDamage : HealPerHit);
		}
	}
}