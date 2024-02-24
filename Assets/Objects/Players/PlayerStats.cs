using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace.Data.Statuses;
using DefaultNamespace.Extensions;
using Managers;
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
		public float Armor;
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
		public float DamageOverTimeFrequencyReductionPercentage;
		public float DamageOverTimeDurationIncreasePercentage;
		public float LifeSteal;
		public float FireDamageIncrease;
		public float LightningDamageIncrease;
		public float IceDamageIncrease;
		public float PhysicalDamageIncrease;
		public float WindDamageIncrease;
		public float LightDamageIncrease;
		public float CosmicDamageIncrease;
		public float EarthDamageIncrease;

		public int RevivesField
		{
			get => Revives;
			set
			{
				var valueChanged = Revives != value;
				Revives = value;
				if (valueChanged && StatusEffectManager.instance != null)
					StatusEffectManager.instance.AddOrRemoveEffect(StatusEffectType.Revive, Revives);
			}
		}
        
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
			DamageOverTimeFrequencyReductionPercentage = 0;
			DamageOverTimeDurationIncreasePercentage = 0;
			LifeSteal = 0;
			FireDamageIncrease = 0;
			LightningDamageIncrease = 0;
			IceDamageIncrease = 0;
			PhysicalDamageIncrease = 0;
			WindDamageIncrease = 0;
			LightDamageIncrease = 0;
			CosmicDamageIncrease = 0;
			EarthDamageIncrease = 0;
		}

		public void Sum(ItemStats item, int rarity)
        {
            var rarityFactor = 1 + ((rarity - 1) * 0.1f); 
        
            AttackCount += item.AttackCount;
            PassThroughCount += item.PassThroughCount;            
            RevivesField += item.Revives;
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
            Armor += item.Armor * rarityFactor;
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
            DamageOverTimeFrequencyReductionPercentage += item.DamageOverTimeFrequencyReductionPercentage * rarityFactor;
            DamageOverTimeDurationIncreasePercentage += item.DamageOverTimeDurationIncreasePercentage * rarityFactor;
            Rerolls += item.Rerolls != 0 ? item.Rerolls + (rarity - 1) : item.Rerolls;
            Skips += item.Skips != 0 ? item.Skips + (rarity - 1) : item.Skips;
            LifeSteal += item.LifeSteal * rarityFactor;
            FireDamageIncrease = item.FireDamageIncrease * rarityFactor;
			LightningDamageIncrease = item.LightningDamageIncrease * rarityFactor;
			IceDamageIncrease = item.IceDamageIncrease * rarityFactor;
			PhysicalDamageIncrease = item.PhysicalDamageIncrease * rarityFactor;
			WindDamageIncrease = item.WindDamageIncrease * rarityFactor;
			LightDamageIncrease = item.LightDamageIncrease * rarityFactor;
			CosmicDamageIncrease = item.CosmicDamageIncrease * rarityFactor;
			EarthDamageIncrease = item.EarthDamageIncrease * rarityFactor;
        }

		public void Set(PlayerStats playerStats)
        {
            var characterId = GameData.GetPlayerCharacterId();
        
            CopyPlayerStats(playerStats);
        
            var playerStatsUpdater = new PlayerStrategyApplier();
            playerStatsUpdater.ApplyRankStrategy(characterId, GameData.GetPlayerCharacterRank(), this);
            playerStatsUpdater.ApplySkillTreeStrategy(characterId, GameData.GetUnlockedSkillTreeNodeIds(), this);
        }
        
        private void CopyPlayerStats(PlayerStats playerStats)
        {
            Health = HealthMax = playerStats.HealthMax;
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
            DamageOverTimeFrequencyReductionPercentage = playerStats.DamageOverTimeFrequencyReductionPercentage;
            DamageOverTimeDurationIncreasePercentage = playerStats.DamageOverTimeDurationIncreasePercentage;
            LifeSteal = playerStats.LifeSteal;
            FireDamageIncrease = playerStats.FireDamageIncrease;
            LightningDamageIncrease =  playerStats.LightningDamageIncrease;
            IceDamageIncrease = playerStats.IceDamageIncrease;
            PhysicalDamageIncrease =  playerStats.PhysicalDamageIncrease;
            WindDamageIncrease =  playerStats.WindDamageIncrease;
            LightDamageIncrease =  playerStats.LightDamageIncrease;
            CosmicDamageIncrease = playerStats.CosmicDamageIncrease;
            EarthDamageIncrease =  playerStats.EarthDamageIncrease;
        }

		public IEnumerable<StatsDisplayData> GetStatsList()
		{
			var stats = new List<StatsDisplayData>
			{
				new("Health", HealthMax, "Max health the character can have. Defines the amount of damage the player can take.", baseValue: 80),
				new("Health regen", HealthRegen, "Amount of health character regenerates per second"),
				new("Heal increase%", HealingIncreasePercentage, "The increase of healing received by the player from any source", true),
				new("Life steal%", LifeSteal, "Amount of damage converted into healing", true),
				new("CDR", CooldownReduction, "Flat reduction of weapon attack cooldown in seconds"),
				new("CDR%", CooldownReductionPercentage, "Reduces weapon attack cooldown by given percentage", true),
				new("Skill CDR%", SkillCooldownReductionPercentage, "Percentage of character skill cooldown reduction", true),
				new("Projectiles", AttackCount, "The amount of times each weapon is allowed to attack"),
				new("Damage", Damage, "Flat damage increase of a weapon attack"),
				new("Damage%", DamagePercentageIncrease, "Increases weapon damage dealt by given percentage", true),
				new("Crit rate", CritRate, "The chance of any weapon hit to critically strike, each hit deals additional damage based on crit damage", true),
				new("Crit damage", CritDamage, "Percentage damage increase during critical strikes", true),
				new("DamageOverTime", DamageOverTime, "Flat damage over time applied to enemies (with applicable weapons)"),
				new("DoT frequency", DamageOverTimeFrequencyReductionPercentage, "Cooldown reduction between applying another DoT effect", true),
				new("DoT duration", DamageOverTimeDurationIncreasePercentage, "Duration increase for every DoT effect", true),
				new("Magnet", MagnetSize, "Defines the distance from which pickups will automatically get pulled towards player", baseValue: 0.6f),
				new("Projectile size", Scale, "Percentage of weapon size increase. Dictates how big AoE effects can be", true),
				new("Projectile speed", Speed, "Percentage of weapon speed increase. Dictates how fast projectiles travels", true),
				new("Attack duration", TimeToLive, "How long the projectile will stay on the screen before disappearing"),
				new("Attack duration%", ProjectileLifeTimeIncreasePercentage, "Percentage increase of projectile life time on screen", true),
				new("Weapon range", DetectionRange, "How far the weapon can detect enemies (with applicable weapons)"),
				new("EXP%", ExperienceIncreasePercentage, "Character in game experience gain increase per EXP shard pickup", true),
				new("Movement speed", MovementSpeed, "Flat character movement speed increase", baseValue: 1.6f),
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
					Health = HealthMax;
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

		public void Add(StatEnum stat, float value)
		{
			switch (stat)
			{
				case StatEnum.Health:
					Health += value;
					break;
				case StatEnum.HealthMax:
					HealthMax += value;
					break;
				case StatEnum.SpecialMax:
					SpecialMax += value;
					break;
				case StatEnum.SpecialIncrease:
					SpecialIncrease += value;
					break;
				case StatEnum.MagnetSize:
					MagnetSize += value;
					break;
				case StatEnum.CooldownReduction:
					CooldownReduction += value;
					break;
				case StatEnum.CooldownReductionPercentage:
					CooldownReductionPercentage += value;
					break;
				case StatEnum.AttackCount:
					AttackCount += (int)value;
					break;
				case StatEnum.Damage:
					Damage += value;
					break;
				case StatEnum.Scale:
					Scale += value;
					break;
				case StatEnum.Speed:
					Speed += value;
					break;
				case StatEnum.TimeToLive:
					TimeToLive += value;
					break;
				case StatEnum.DetectionRange:
					DetectionRange += value;
					break;
				case StatEnum.DamagePercentageIncrease:
					DamagePercentageIncrease += value;
					break;
				case StatEnum.ExperienceIncreasePercentage:
					ExperienceIncreasePercentage += value;
					break;
				case StatEnum.MovementSpeed:
					MovementSpeed += value;
					break;
				case StatEnum.SkillCooldownReductionPercentage:
					SkillCooldownReductionPercentage += value;
					break;
				case StatEnum.HealthRegen:
					HealthRegen += value;
					break;
				case StatEnum.CritRate:
					CritRate += value;
					break;
				case StatEnum.CritDamage:
					CritDamage += value;
					break;
				case StatEnum.PassThroughCount:
					PassThroughCount += (int)value;
					break;
				case StatEnum.Armor:
					Armor += value;
					break;
				case StatEnum.EnemySpeedIncreasePercentage:
					EnemySpeedIncreasePercentage += value;
					break;
				case StatEnum.EnemySpawnRateIncreasePercentage:
					EnemySpawnRateIncreasePercentage += value;
					break;
				case StatEnum.EnemyHealthIncreasePercentage:
					EnemyHealthIncreasePercentage += value;
					break;
				case StatEnum.EnemyMaxCountIncreasePercentage:
					EnemyMaxCountIncreasePercentage += value;
					break;
				case StatEnum.ItemRewardIncrease:
					ItemRewardIncrease += value;
					break;
				case StatEnum.Revives:
					RevivesField += (int)value;
					break;
				case StatEnum.ProjectileLifeTimeIncreasePercentage:
					ProjectileLifeTimeIncreasePercentage += value;
					break;
				case StatEnum.DodgeChance:
					DodgeChance += value;
					break;
				case StatEnum.DamageTakenIncreasePercentage:
					DamageTakenIncreasePercentage += value;
					break;
				case StatEnum.HealingIncreasePercentage:
					HealingIncreasePercentage += value;
					break;
				case StatEnum.Luck:
					Luck += value;
					break;
				case StatEnum.DamageOverTime:
					DamageOverTime += value;
					break;
				case StatEnum.Rerolls:
					Rerolls += (int)value;
					break;
				case StatEnum.Skips:
					Skips +=(int) value;
					break;
				case StatEnum.DamageOverTimeFrequencyReduction:
					DamageOverTimeFrequencyReductionPercentage += value;
					break;
				case StatEnum.DamageOverTimeDurationIncrease:
					DamageOverTimeDurationIncreasePercentage += value;
					break;
				case StatEnum.LifeSteal:
					LifeSteal += value;
					break;
				case StatEnum.FireDamageIncrease:
					FireDamageIncrease += value;
					break;
				case StatEnum.LightningDamageIncrease:
					LightningDamageIncrease += value;
					break;
				case StatEnum.IceDamageIncrease:
					IceDamageIncrease += value;
					break;
				case StatEnum.PhysicalDamageIncrease:
					PhysicalDamageIncrease += value;
					break;
				case StatEnum.WindDamageIncrease:
					WindDamageIncrease += value;
					break;
				case StatEnum.LightDamageIncrease:
					LightDamageIncrease += value;
					break;
				case StatEnum.CosmicDamageIncrease:
					CosmicDamageIncrease += value;
					break;
				case StatEnum.EarthDamageIncrease:
					EarthDamageIncrease += value;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}
	}
}