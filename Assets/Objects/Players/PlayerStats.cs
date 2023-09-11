using System;
using System.Collections.Generic;
using DefaultNamespace.Extensions;
using Objects.Abilities;
using Objects.Characters;
using Objects.Items;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace Objects.Players
{
	[Serializable]
	public class PlayerStats
	{
		public float Health;
		public float HealthMax;
		public float SpecialMax;
		public float SpecialIncrease;
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
		public float DodgeChance;
		public float DamageTakenIncreasePercentage;
		public float HealingIncreasePercentage;
		public float Luck;
		public float DamageOverTime;
		public int Rerolls;
		public int Skips;

		public PlayerStats()
		{
			ApplyDefaultStats();
		}

		public PlayerStats(PlayerStats playerStats)
		{
			Set(playerStats);
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
			DodgeChance = 0;
			DamageTakenIncreasePercentage = 0;
			HealingIncreasePercentage = 0;
			Luck = 0;
			DamageOverTime = 0;
			Rerolls = 0;
			Skips = 0;
		}

		public void Sum(ItemStats item, int rarity)
        {
            var rarityFactor = 1 + ((rarity - 1) * 0.1f); 
        
            AttackCount += item.AttackCount;
            PassThroughCount += item.PassThroughCount;            
            Revives += item.Revives;
            Health += item.Health * rarityFactor;
            HealthMax += item.HealthMax * rarityFactor;
            MagnetSize += item.MagnetSize * rarityFactor;
            CooldownReduction += item.CooldownReduction * rarityFactor;
            CooldownReductionPercentage += item.CooldownReductionPercentage * rarityFactor;
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
            Armor += item.Armor != 0 ? item.Armor + (rarity - 1) : item.Armor;
            EnemySpeedIncreasePercentage += item.EnemySpeedIncreasePercentage * rarityFactor;
            EnemySpawnRateIncreasePercentage += item.EnemySpawnRateIncreasePercentage * rarityFactor;
            EnemyHealthIncreasePercentage += item.EnemyHealthIncreasePercentage * rarityFactor;
            EnemyMaxCountIncreasePercentage += item.EnemyMaxCountIncreasePercentage * rarityFactor;
            ItemRewardIncrease += item.ItemRewardIncrease * rarityFactor;
            ProjectileLifeTimeIncreasePercentage += item.ProjectileLifeTimeIncreasePercentage * rarityFactor;
            DodgeChance += item.DodgeChance * rarityFactor;
            DamageTakenIncreasePercentage += item.DamageTakenIncreasePercentage * rarityFactor;
            HealingIncreasePercentage += item.HealingReceivedIncreasePercentage * rarityFactor;
            Luck += item.Luck * rarityFactor;
            DamageOverTime += item.DamageOverTime * rarityFactor;
            Rerolls += item.Rerolls != 0 ? item.Rerolls + (rarity - 1) : item.Rerolls;
            Skips += item.Skips != 0 ? item.Skips + (rarity - 1) : item.Skips;
        }

		public void Set(PlayerStats playerStats)
        {
            var characterId = GameData.GetPlayerCharacterId();
            var characterRank = GameData.GetPlayerCharacterRank();
        
            CopyPlayerStats(playerStats);
        
            var playerStatsUpdater = new PlayerStrategyApplier();
            playerStatsUpdater.ApplyRankStrategy(characterId, characterRank, this);
        }
        
        private void CopyPlayerStats(PlayerStats playerStats)
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
            DodgeChance = playerStats.DodgeChance;
            DamageTakenIncreasePercentage = playerStats.DamageTakenIncreasePercentage;
            HealingIncreasePercentage = playerStats.HealingIncreasePercentage;
            Luck = playerStats.Luck;
            DamageOverTime = playerStats.DamageOverTime;
            Rerolls = playerStats.Rerolls;
            Skips = playerStats.Skips;
            SpecialIncrease = playerStats.SpecialIncrease;
            SpecialMax = playerStats.SpecialMax;
        }

		public IEnumerable<StatsDisplayData> GetStatsList()
		{
			var stats = new List<StatsDisplayData>
			{
				new("Health", HealthMax, "Max health the character can have. Defines the amount of damage the player can take.", baseValue: 80),
				new("Magnet", MagnetSize, "Defines the distance from which pickups will automatically get pulled towards player", baseValue: 0.6f),
				new("CDR", CooldownReduction, "Flat reduction of weapon attack cooldown in seconds"),
				new("CDR%", CooldownReductionPercentage, "Reduces weapon attack cooldown by given percentage", true),
				new("Projectiles", AttackCount, "The amount of times each weapon is allowed to attack"),
				new("Damage", Damage, "Flat damage increase of a weapon attack"),
				new("Damage%", DamagePercentageIncrease, "Increases weapon damage dealt by given percentage", true),
				new("DamageOverTime", DamageOverTime, "Flat damage over time applied to enemies (with applicable weapons)"),
				new("Projectile size", Scale, "Percentage of weapon size increase. Dictates how big AoE effects can be", true),
				new("Projectile speed", Speed, "Percentage of weapon speed increase. Dictates how fast projectiles travels", true),
				new("Attack duration", TimeToLive, "How long the projectile will stay on the screen before disappearing"),
				new("Attack duration%", ProjectileLifeTimeIncreasePercentage, "Percentage increase of projectile life time on screen", true),
				new("Weapon range", DetectionRange, "How far the weapon can detect enemies (with applicable weapons)"),
				new("EXP%", ExperienceIncreasePercentage, "Character in game experience gain increase per EXP shard pickup", true),
				new("Movement speed", MovementSpeed, "Flat character movement speed increase", baseValue: 1.6f),
				new("Skill CDR%", SkillCooldownReductionPercentage, "Percentage of character skill cooldown reduction", true),
				new("Health regen", HealthRegen, "Amount of health character regenerates per second"),
				new("Crit rate", CritRate, "The chance of any weapon hit to critically strike, each hit deals additional damage based on crit damage", true),
				new("Crit damage", CritDamage, "Percentage damage increase during critical strikes", true),
				new("Pass through", PassThroughCount, "How many times a projectile can pass through enemies before disappearing (with applicable weapons)"),
				new("Armor", Armor, "Flat damage reduction from enemy attacks"),
				new("Enemy speed%", EnemySpeedIncreasePercentage, "Increases enemy speed by given percentage. The higher the faster enemies will move towards player", true, true),
				new("Enemy spawn rate%", EnemySpawnRateIncreasePercentage, "Increase the rate at which enemies can spawn up until the max amount possible per wave", true, true),
				new("Enemy health%", EnemyHealthIncreasePercentage, "Increases enemy max health by given percentage", true, true),
				new("Enemy count%", EnemyMaxCountIncreasePercentage, "Increases the max amount of enemies that can spawn each wave by given percentage", true, true),
				new("Reward increase", ItemRewardIncrease, "Increases the amount of gold or gems rewarded per pickup", true),
				new("Revives", Revives, "The amount of times the player can be revived after dying. Revival brings player back to life with 50% health"),
				new("Dodge chance%", DodgeChance, "The chance to avoid taking damage after colliding with an enemy", true),
				new("Damage taken%", DamageTakenIncreasePercentage, "The increase of damage taken by the player from any source", true, true),
				new("Heal increase%", HealingIncreasePercentage, "The increase of healing received by the player from any source", true),
				new("Luck%", Luck, "Increase the chance of high rarity items appearing in chests of after leveling up. Also increases the chance of enemies dropping gold coins and gems upon death", true),
				new("Rerolls", Rerolls, "The amount of times each set of upgrades to pick from level ups can be rerolled"),
				new("Skips", Skips, "The amount of times each level up choice can be skipped")
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
					HealthRegen += value;
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
					HealthMax += value;
					break;
				case PermUpgradeType.Curse:
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
				case PermUpgradeType.Luck:
					Luck += value;
					break;
				case PermUpgradeType.Reroll:
					Rerolls += (int) value;
					break;
				case PermUpgradeType.Skip:
					Skips += (int) value;
					break;
				case PermUpgradeType.BuyGems:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(permUpgradeType), permUpgradeType, null);
			}
		}
	}
}