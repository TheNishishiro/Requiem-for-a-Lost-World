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
		public int Health;
		public int HealthMax;
		public float MagnetSize;
		public float CooldownReduction;
		public float CooldownReductionPercentage;
		public int AttackCount;
		public int Damage;
		public float Scale;
		public float Speed;
		public float TimeToLive;
		public float DetectionRange;
		public float DamagePercentageIncrease;
		public float ExperienceIncreasePercentage;
		public float MovementSpeed;
		public float SkillCooldownReductionPercentage;
		public int HealthRegen;
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

		public void Sum(ItemStats item)
		{
			Health += item.Health;
			HealthMax += item.HealthMax;
			MagnetSize += item.MagnetSize;
			CooldownReduction += item.CooldownReduction;
			CooldownReductionPercentage += item.CooldownReductionPercentage;
			AttackCount += item.AttackCount;
			Damage += item.Damage;
			Scale += item.Scale;
			Speed += item.Speed;
			TimeToLive += item.TimeToLive;
			DetectionRange += item.DetectionRange;
			DamagePercentageIncrease += item.DamagePercentageIncrease;
			ExperienceIncreasePercentage += item.ExperienceIncreasePercentage;
			MovementSpeed += item.MovementSpeed;
			SkillCooldownReductionPercentage += item.SkillCooldownReductionPercentage;
			HealthRegen += item.HealthRegen;
			CritRate += item.CritRate;
			CritDamage += item.CritDamage;
			PassThroughCount += item.PassThroughCount;
			Armor += item.Armor;
			EnemySpeedIncreasePercentage = item.EnemySpeedIncreasePercentage;
			EnemySpawnRateIncreasePercentage = item.EnemySpawnRateIncreasePercentage;
			EnemyHealthIncreasePercentage = item.EnemyHealthIncreasePercentage;
			EnemyMaxCountIncreasePercentage = item.EnemyMaxCountIncreasePercentage;
			ItemRewardIncrease = item.ItemRewardIncrease;
			Revives = item.Revives;
			ProjectileLifeTimeIncreasePercentage = item.ProjectileLifeTimeIncreasePercentage;
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
			var stats = new List<CharacterStats>();
			stats.Add(new CharacterStats("Health", HealthMax));
			stats.Add(new CharacterStats("Magnet", MagnetSize));
			stats.Add(new CharacterStats("CDR", CooldownReduction));
			stats.Add(new CharacterStats("CDR%", CooldownReductionPercentage, true));
			stats.Add(new CharacterStats("Projectiles", AttackCount));
			stats.Add(new CharacterStats("Damage", Damage));
			stats.Add(new CharacterStats("Projectile size", Scale, true));
			stats.Add(new CharacterStats("Projectile speed", Speed));
			stats.Add(new CharacterStats("Attack duration", TimeToLive));
			stats.Add(new CharacterStats("Weapon range", DetectionRange));
			stats.Add(new CharacterStats("Damage%", DamagePercentageIncrease, true));
			stats.Add(new CharacterStats("EXP%", ExperienceIncreasePercentage, true));
			stats.Add(new CharacterStats("Movement speed", MovementSpeed));
			stats.Add(new CharacterStats("Skill CDR%", SkillCooldownReductionPercentage, true));
			stats.Add(new CharacterStats("Health regen", HealthRegen));
			stats.Add(new CharacterStats("Crit rate", CritRate));
			stats.Add(new CharacterStats("Crit damage", CritDamage));
			stats.Add(new CharacterStats("Pass through", PassThroughCount));
			stats.Add(new CharacterStats("Armor", Armor));
			stats.Add(new CharacterStats("Enemy speed%", EnemySpeedIncreasePercentage, true, true));
			stats.Add(new CharacterStats("Enemy spawn rate%", EnemySpawnRateIncreasePercentage, true, true));
			stats.Add(new CharacterStats("Enemy health%", EnemyHealthIncreasePercentage, true, true));
			stats.Add(new CharacterStats("Enemy count%", EnemyMaxCountIncreasePercentage, true, true));
			stats.Add(new CharacterStats("Reward increase", ItemRewardIncrease, true));
			stats.Add(new CharacterStats("Revives", Revives));
			stats.Add(new CharacterStats("Weapon duration%", ProjectileLifeTimeIncreasePercentage, true));
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