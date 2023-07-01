using System;
using System.Collections.Generic;
using DefaultNamespace.Extensions;
using Objects.Abilities;
using Objects.Characters;
using Objects.Items;
using Objects.Players.PermUpgrades;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace Objects.Players
{
	[Serializable]
	public class PlayerStats
	{
		public float Health;
		public float HealthMax;
		public float MagnetSize;
		public float CooldownReduction;
		public float CooldownReductionPercentage;
		public int AttackCount;
		public float Damage;
		public float Scale;
		public float Speed;
		public float TimeToLive;
		public float DetectionRange;
		public float DamagePercentageIncrease;
		public float ExperienceIncreasePercentage;
		public float MovementSpeed;
		public float SkillCooldownReductionPercentage;
		public float HealthRegen;
		public float CritRate;
		public float CritDamage;
		public int PassThroughCount;
		public int Armor;
		public float EnemySpeedIncreasePercentage;
		public float EnemySpawnRateIncreasePercentage;
		public float EnemyHealthIncreasePercentage;
		public float EnemyMaxCountIncreasePercentage;
		public float ItemRewardIncrease;
		public int Revives;
		public float ProjectileLifeTimeIncreasePercentage;

		public PlayerStats()
		{
			ApplyDefaultStats();
		}
		
		public void ApplyDefaultStats()
		{
			Health = 100;
			HealthMax = 100;
			MagnetSize = 1;
			CooldownReduction = 0;
			CooldownReductionPercentage = 0;
			AttackCount = 0;
			Damage = 0;
			Scale = 0;
			Speed = 0;
			TimeToLive = 0;
			DetectionRange = 0;
			DamagePercentageIncrease = 0;
			ExperienceIncreasePercentage = 0;
			MovementSpeed = 1;
			SkillCooldownReductionPercentage = 0;
			HealthRegen = 0;
			CritRate = 0;
			CritDamage = 0;
			PassThroughCount = 0;
			Armor = 0;
			EnemySpeedIncreasePercentage = 0;
			EnemySpawnRateIncreasePercentage = 0;
			EnemyHealthIncreasePercentage = 0;
			EnemyMaxCountIncreasePercentage = 0;
			ItemRewardIncrease = 0;
			Revives = 0;
			ProjectileLifeTimeIncreasePercentage = 0;
		}

		public void Sum(ItemStats item, int rarity)
        {
            var rarityFactor = 1 + ((rarity - 1) * 0.1f); 
        
            Health += item.Health * rarityFactor;
            HealthMax += item.HealthMax * rarityFactor;
            MagnetSize += item.MagnetSize * rarityFactor;
            CooldownReduction += item.CooldownReduction * rarityFactor;
            CooldownReductionPercentage += item.CooldownReductionPercentage * rarityFactor;
            AttackCount += item.AttackCount;
            Damage += item.Damage * rarityFactor;
            Scale += item.Scale * rarityFactor;
            Speed += item.Speed * rarityFactor;
            TimeToLive += item.TimeToLive * rarityFactor;
            DetectionRange += item.DetectionRange * rarityFactor;
            DamagePercentageIncrease += item.DamagePercentageIncrease * rarityFactor;
            ExperienceIncreasePercentage += item.ExperienceIncreasePercentage * rarityFactor;
            MovementSpeed += item.MovementSpeed * rarityFactor;
            SkillCooldownReductionPercentage += item.SkillCooldownReductionPercentage * rarityFactor;
            HealthRegen += item.HealthRegen * rarityFactor;
            CritRate += item.CritRate * rarityFactor;
            CritDamage += item.CritDamage * rarityFactor;
            PassThroughCount += item.PassThroughCount;
            Armor += (int)(item.Armor * rarityFactor);
            EnemySpeedIncreasePercentage += item.EnemySpeedIncreasePercentage * rarityFactor;
            EnemySpawnRateIncreasePercentage += item.EnemySpawnRateIncreasePercentage * rarityFactor;
            EnemyHealthIncreasePercentage += item.EnemyHealthIncreasePercentage * rarityFactor;
            EnemyMaxCountIncreasePercentage += item.EnemyMaxCountIncreasePercentage * rarityFactor;
            ItemRewardIncrease += item.ItemRewardIncrease * rarityFactor;
            Revives += item.Revives;
            ProjectileLifeTimeIncreasePercentage += item.ProjectileLifeTimeIncreasePercentage * rarityFactor;
        }

		public void Set(PlayerStats playerStats)
		{
			Health = playerStats.Health;
			HealthMax = playerStats.HealthMax;
			MagnetSize = playerStats.MagnetSize;
			CooldownReduction = playerStats.CooldownReduction;
			CooldownReductionPercentage = playerStats.CooldownReductionPercentage;
			AttackCount = playerStats.AttackCount;
			Damage = playerStats.Damage;
			Scale = playerStats.Scale;
			Speed = playerStats.Speed;
			TimeToLive = playerStats.TimeToLive;
			DetectionRange = playerStats.DetectionRange;
			DamagePercentageIncrease = playerStats.DamagePercentageIncrease;
			ExperienceIncreasePercentage = playerStats.ExperienceIncreasePercentage;
			MovementSpeed = playerStats.MovementSpeed;
			SkillCooldownReductionPercentage = playerStats.SkillCooldownReductionPercentage;
			HealthRegen = playerStats.HealthRegen;
			CritRate = playerStats.CritRate;
			CritDamage = playerStats.CritDamage;
			PassThroughCount = playerStats.PassThroughCount;
			Armor = playerStats.Armor;
			EnemySpeedIncreasePercentage = playerStats.EnemySpeedIncreasePercentage;
			EnemySpawnRateIncreasePercentage = playerStats.EnemySpawnRateIncreasePercentage;
			EnemyHealthIncreasePercentage = playerStats.EnemyHealthIncreasePercentage;
			EnemyMaxCountIncreasePercentage = playerStats.EnemyMaxCountIncreasePercentage;
			ItemRewardIncrease = playerStats.ItemRewardIncrease;
			Revives = playerStats.Revives;
			ProjectileLifeTimeIncreasePercentage = playerStats.ProjectileLifeTimeIncreasePercentage;
		}

		public IEnumerable<CharacterStats> GetStatsList()
		{
			var stats = new List<CharacterStats>
			{
				new("Health", HealthMax),
				new("Magnet", MagnetSize),
				new("CDR", CooldownReduction),
				new("CDR%", CooldownReductionPercentage, true),
				new("Projectiles", AttackCount),
				new("Damage", Damage),
				new("Projectile size", Scale, true),
				new("Projectile speed", Speed),
				new("Attack duration", TimeToLive),
				new("Weapon range", DetectionRange),
				new("Damage%", DamagePercentageIncrease, true),
				new("EXP%", ExperienceIncreasePercentage, true),
				new("Movement speed", MovementSpeed),
				new("Skill CDR%", SkillCooldownReductionPercentage, true),
				new("Health regen", HealthRegen),
				new("Crit rate", CritRate),
				new("Crit damage", CritDamage),
				new("Pass through", PassThroughCount),
				new("Armor", Armor),
				new("Enemy speed%", EnemySpeedIncreasePercentage, true, true),
				new("Enemy spawn rate%", EnemySpawnRateIncreasePercentage, true, true),
				new("Enemy health%", EnemyHealthIncreasePercentage, true, true),
				new("Enemy count%", EnemyMaxCountIncreasePercentage, true, true),
				new("Reward increase", ItemRewardIncrease, true),
				new("Revives", Revives),
				new("Weapon duration%", ProjectileLifeTimeIncreasePercentage, true)
			};
			return stats;
		}

		public void Add(PermUpgradeType permUpgradeType, float value)
		{
			switch (permUpgradeType)
			{
				case PermUpgradeType.Magnet:
					MagnetSize += value;
					break;
				case PermUpgradeType.MovementSpeed:
					MovementSpeed += value;
					break;
				case PermUpgradeType.AttackCount:
					AttackCount += (int) value;
					break;
				case PermUpgradeType.HealthRegen:
					HealthRegen += (int) value;
					break;
				case PermUpgradeType.DamageIncrease:
					DamagePercentageIncrease += value;
					break;
				case PermUpgradeType.Armor:
					Armor += (int) value;
					break;
				case PermUpgradeType.CooldownReduction:
					CooldownReductionPercentage += value;
					break;
				case PermUpgradeType.Health:
					HealthMax += (int) value;
					break;
				case PermUpgradeType.Curse:
					EnemySpeedIncreasePercentage += value;
					EnemySpawnRateIncreasePercentage += value;
					EnemyMaxCountIncreasePercentage += value;
					EnemyHealthIncreasePercentage += value;
					break;
				case PermUpgradeType.Revival:
					Revives += (int) value;
					break;
				case PermUpgradeType.CritRate:
					CritRate += value;
					break;
				case PermUpgradeType.CritDamage:
					CritDamage += value;
					break;
				case PermUpgradeType.AreaIncrease:
					Scale += value;
					break;
				case PermUpgradeType.ProjectileSpeed:
					Speed += value;
					break;
				case PermUpgradeType.EffectDuration:
					ProjectileLifeTimeIncreasePercentage += value;
					break;
				case PermUpgradeType.ExpIncrease:
					ExperienceIncreasePercentage += value;
					break;
				case PermUpgradeType.Greed:
					ItemRewardIncrease += value;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(permUpgradeType), permUpgradeType, null);
			}
		}
	}
}