using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace.Data;
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
		public float SpecialValue;
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
		public int DashCount;
		public float Stamina;
		public float ElementalReactionEffectIncreasePercentage;
		public float FollowUpDamageIncrease;
		public float HealthIncreasePercentage;
		public bool CanDotCrit;

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
			DashCount = 2;
			Stamina = 2;
			SpecialValue = 0;
			ElementalReactionEffectIncreasePercentage = 0;
			FollowUpDamageIncrease = 0;
			HealthIncreasePercentage = 0;
			CanDotCrit = false;
		}

		public void Sum(ItemStats item, int rarity)
        {
            var rarityFactor = 1 + ((rarity - 1) * 0.1f); 
        
            AttackCount += item.AttackCount;
            PassThroughCount += item.PassThroughCount;            
            RevivesField += item.Revives;
            DashCount += item.DashCount;
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
            FireDamageIncrease += item.FireDamageIncrease * rarityFactor;
			LightningDamageIncrease += item.LightningDamageIncrease * rarityFactor;
			IceDamageIncrease += item.IceDamageIncrease * rarityFactor;
			PhysicalDamageIncrease += item.PhysicalDamageIncrease * rarityFactor;
			WindDamageIncrease += item.WindDamageIncrease * rarityFactor;
			LightDamageIncrease += item.LightDamageIncrease * rarityFactor;
			CosmicDamageIncrease += item.CosmicDamageIncrease * rarityFactor;
			EarthDamageIncrease += item.EarthDamageIncrease * rarityFactor;
			Stamina += item.Stamina * rarityFactor;
			ElementalReactionEffectIncreasePercentage += item.ElementalReactionEffectIncreasePercentage * rarityFactor;
			FollowUpDamageIncrease += item.FollowUpDamageIncrease * rarityFactor;
			HealthIncreasePercentage += item.HealthIncreasePercentage * rarityFactor;
			if (item.CanDotCrit) CanDotCrit = true;
        }

		public void Set(PlayerStats playerStats)
        {
            var characterId = GameData.GetPlayerCharacterId();
        
            CopyPlayerStats(playerStats);
        
            var playerStatsUpdater = new PlayerStrategyApplier();
            playerStatsUpdater.ApplyRankStrategy(characterId, GameData.GetPlayerCharacterRank(), this);
            foreach (var characterRune in SaveFile.Instance.GetCharacterSaveData(characterId).GetCharacterRunes())
            {
	            Add(characterRune.statType, GameData.ScaleRune(characterRune));
            }
        }
		
		private void CopyPlayerStats(PlayerStats playerStats)
		{
		    var properties = typeof(PlayerStats).GetFields();
		    foreach (var property in properties)
		    {
		        if (property.IsPublic)
		        {
		            property.SetValue(this, property.GetValue(playerStats));
		        }
		    }
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
				case PermUpgradeType.ElementalReactionEffectIncreasePercentage:
					ElementalReactionEffectIncreasePercentage += value;
					break;
				case PermUpgradeType.FollowUpDamageIncrease:
					FollowUpDamageIncrease += value;
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
				case StatEnum.StaminaIncrease:
					Stamina += value;
					break;
				case StatEnum.ElementalReactionEffectIncreasePercentage:
					ElementalReactionEffectIncreasePercentage += value;
					break;
				case StatEnum.FollowUpDamageIncrease:
					FollowUpDamageIncrease += value;
					break;
				case StatEnum.HealthIncreasePercentage:
					HealthIncreasePercentage += value;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}
	}
}