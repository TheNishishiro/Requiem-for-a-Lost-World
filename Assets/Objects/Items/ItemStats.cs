using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine.Serialization;

namespace Objects.Items
{
	[Serializable]
	public class ItemStats
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
		public float HealingReceivedIncreasePercentage;
		public float Luck;
		public float DamageOverTime;
		public int Rerolls;
		public int Skips;
		public float DamageOverTimeFrequencyReductionPercentage;
		public float DamageOverTimeDurationIncreasePercentage;
		public float LifeSteal;

		public void ApplyRarity(int rarity)
		{
			var rarityFactor = GetRarityFactor(rarity);

			Health *= rarityFactor;
			HealthMax *= rarityFactor;
			MagnetSize *= rarityFactor;
			CooldownReduction *= rarityFactor;
			CooldownReductionPercentage *= rarityFactor;
			Damage *= rarityFactor;
			Scale *= rarityFactor;
			Speed *= rarityFactor;
			TimeToLive *= rarityFactor;
			DetectionRange *= rarityFactor;
			DamagePercentageIncrease *= rarityFactor;
			ExperienceIncreasePercentage *= rarityFactor;
			MovementSpeed *= rarityFactor;
			SkillCooldownReductionPercentage *= rarityFactor;
			HealthRegen *= rarityFactor;
			CritRate *= rarityFactor;
			CritDamage *= rarityFactor;
			PassThroughCount = PassThroughCount != 0 ? PassThroughCount + (rarity - 1) : PassThroughCount;
			Armor = Armor != 0 ? Armor + (rarity - 1) : Armor;
			EnemySpeedIncreasePercentage *= rarityFactor;
			EnemySpawnRateIncreasePercentage *= rarityFactor;
			EnemyHealthIncreasePercentage *= rarityFactor;
			EnemyMaxCountIncreasePercentage *= rarityFactor;
			ItemRewardIncrease *= rarityFactor;
			ProjectileLifeTimeIncreasePercentage *= rarityFactor;
			DodgeChance *= rarityFactor;
			DamageTakenIncreasePercentage *= rarityFactor;
			HealingReceivedIncreasePercentage *= rarityFactor;
			Luck *= rarityFactor;
			DamageOverTime *= rarityFactor;
			DamageOverTimeFrequencyReductionPercentage *= rarityFactor;
			DamageOverTimeDurationIncreasePercentage *= rarityFactor;
			LifeSteal *= rarityFactor;
			Rerolls = Rerolls != 0 ? Rerolls + (rarity - 1) : Rerolls;
			Skips = Skips != 0 ? Skips + (rarity - 1) : Skips;
		}
		
		public void Apply(ItemStats itemUpgradeItemStats, int rarity)
        {
	        var rarityFactor = GetRarityFactor(rarity);
	        
            Health += itemUpgradeItemStats.Health * rarityFactor;
            HealthMax += itemUpgradeItemStats.HealthMax * rarityFactor;
            MagnetSize += itemUpgradeItemStats.MagnetSize * rarityFactor;
            CooldownReduction += itemUpgradeItemStats.CooldownReduction * rarityFactor;
            CooldownReductionPercentage += itemUpgradeItemStats.CooldownReductionPercentage * rarityFactor;
            AttackCount += itemUpgradeItemStats.AttackCount;
            Damage += itemUpgradeItemStats.Damage * rarityFactor;
            Scale += itemUpgradeItemStats.Scale * rarityFactor;
            Speed += itemUpgradeItemStats.Speed * rarityFactor;
            TimeToLive += itemUpgradeItemStats.TimeToLive * rarityFactor;
            DetectionRange += itemUpgradeItemStats.DetectionRange * rarityFactor;
            DamagePercentageIncrease += itemUpgradeItemStats.DamagePercentageIncrease * rarityFactor;
            ExperienceIncreasePercentage += itemUpgradeItemStats.ExperienceIncreasePercentage * rarityFactor;
            MovementSpeed += itemUpgradeItemStats.MovementSpeed * rarityFactor;
            SkillCooldownReductionPercentage += itemUpgradeItemStats.SkillCooldownReductionPercentage * rarityFactor;
            HealthRegen += itemUpgradeItemStats.HealthRegen * rarityFactor;
            CritRate += itemUpgradeItemStats.CritRate * rarityFactor;
            CritDamage += itemUpgradeItemStats.CritDamage * rarityFactor;
            PassThroughCount += itemUpgradeItemStats.PassThroughCount == 0 ? 0 : itemUpgradeItemStats.PassThroughCount + (rarity - 1);
            Armor += itemUpgradeItemStats.Armor == 0 ? 0 : itemUpgradeItemStats.Armor + (rarity - 1);
            EnemySpeedIncreasePercentage += itemUpgradeItemStats.EnemySpeedIncreasePercentage * rarityFactor;
            EnemySpawnRateIncreasePercentage += itemUpgradeItemStats.EnemySpawnRateIncreasePercentage * rarityFactor;
            EnemyHealthIncreasePercentage += itemUpgradeItemStats.EnemyHealthIncreasePercentage * rarityFactor;
            EnemyMaxCountIncreasePercentage += itemUpgradeItemStats.EnemyMaxCountIncreasePercentage * rarityFactor;
            ItemRewardIncrease += itemUpgradeItemStats.ItemRewardIncrease * rarityFactor;
            Revives += itemUpgradeItemStats.Revives;
            ProjectileLifeTimeIncreasePercentage += itemUpgradeItemStats.ProjectileLifeTimeIncreasePercentage * rarityFactor;
            DodgeChance += itemUpgradeItemStats.DodgeChance * rarityFactor;
            DamageTakenIncreasePercentage += itemUpgradeItemStats.DamageTakenIncreasePercentage * rarityFactor;
            HealingReceivedIncreasePercentage += itemUpgradeItemStats.HealingReceivedIncreasePercentage * rarityFactor;
            Luck += itemUpgradeItemStats.Luck * rarityFactor;
            DamageOverTime += itemUpgradeItemStats.DamageOverTime * rarityFactor;
            DamageOverTimeFrequencyReductionPercentage += itemUpgradeItemStats.DamageOverTimeFrequencyReductionPercentage * rarityFactor;
            DamageOverTimeDurationIncreasePercentage += itemUpgradeItemStats.DamageOverTimeFrequencyReductionPercentage * rarityFactor;
            LifeSteal += itemUpgradeItemStats.LifeSteal * rarityFactor;
            Rerolls += itemUpgradeItemStats.Rerolls == 0 ? 0 : itemUpgradeItemStats.Rerolls + (rarity - 1);
            Skips += itemUpgradeItemStats.Skips == 0 ? 0 : itemUpgradeItemStats.Skips + (rarity - 1);
        }

		public string GetDescription(string description, int rarity)
		{
			var rarityFactor = GetRarityFactor(rarity);

			return description
				.Replace("{Health}", Utilities.StatToString(Health, rarityFactor))
				.Replace("{HealthMax}", Utilities.StatToString(HealthMax, rarityFactor))
				.Replace("{MagnetSize}", Utilities.StatToString(MagnetSize, rarityFactor))
				.Replace("{CooldownReduction}", Utilities.StatToString(CooldownReduction, rarityFactor))
				.Replace("{CooldownReductionPercentage}", Utilities.StatToString(CooldownReductionPercentage, rarityFactor, true))
				.Replace("{AttackCount}", Utilities.StatToString(AttackCount))
				.Replace("{Damage}", Utilities.StatToString(Damage, rarityFactor))
				.Replace("{Scale}", Utilities.StatToString(Scale, rarityFactor, true))
				.Replace("{Speed}", Utilities.StatToString(Speed, rarityFactor))
				.Replace("{TimeToLive}", Utilities.StatToString(TimeToLive, rarityFactor))
				.Replace("{DetectionRange}", Utilities.StatToString(DetectionRange, rarityFactor))
				.Replace("{DamagePercentageIncrease}", Utilities.StatToString(DamagePercentageIncrease, rarityFactor, true))
				.Replace("{ExperienceIncreasePercentage}", Utilities.StatToString(ExperienceIncreasePercentage, rarityFactor, true))
				.Replace("{MovementSpeed}", Utilities.StatToString(MovementSpeed, rarityFactor))
				.Replace("{SkillCooldownReductionPercentage}", Utilities.StatToString(SkillCooldownReductionPercentage, rarityFactor, true))
				.Replace("{HealthRegen}", Utilities.StatToString(HealthRegen, rarityFactor))
				.Replace("{CritRate}", Utilities.StatToString(CritRate, rarityFactor, true))
				.Replace("{CritDamage}", Utilities.StatToString(CritDamage, rarityFactor, true))
				.Replace("{PassThroughCount}", (PassThroughCount == 0 ? 0 : PassThroughCount + (rarity - 1)).ToString())
				.Replace("{Armor}", Utilities.StatToString(Armor, rarityFactor))
				.Replace("{EnemySpeedIncreasePercentage}", Utilities.StatToString(EnemySpeedIncreasePercentage, rarityFactor, true))
				.Replace("{EnemySpawnRateIncreasePercentage}", Utilities.StatToString(EnemySpawnRateIncreasePercentage, rarityFactor, true))
				.Replace("{EnemyHealthIncreasePercentage}", Utilities.StatToString(EnemyHealthIncreasePercentage, rarityFactor, true))
				.Replace("{EnemyMaxCountIncreasePercentage}", Utilities.StatToString(EnemyMaxCountIncreasePercentage, rarityFactor, true))
				.Replace("{ItemRewardIncrease}", Utilities.StatToString(ItemRewardIncrease, rarityFactor, true))
				.Replace("{Revives}", Revives.ToString())
				.Replace("{ProjectileLifeTimeIncreasePercentage}", Utilities.StatToString(ProjectileLifeTimeIncreasePercentage, rarityFactor, true))
				.Replace("{DodgeChance}", Utilities.StatToString(DodgeChance, rarityFactor, true))
				.Replace("{DamageTakenIncreasePercentage}", Utilities.StatToString(DamageTakenIncreasePercentage, rarityFactor, true))
				.Replace("{HealingReceivedIncreasePercentage}", Utilities.StatToString(HealingReceivedIncreasePercentage, rarityFactor, true))
				.Replace("{LifeSteal}", Utilities.StatToString(LifeSteal, rarityFactor, true))
				.Replace("{Luck}", Utilities.StatToString(Luck, rarityFactor, true))
				.Replace("{DamageOverTime}", Utilities.StatToString(DamageOverTime, rarityFactor))
				.Replace("{DamageOverTimeFrequencyReduction}", Utilities.StatToString(DamageOverTimeFrequencyReductionPercentage, rarityFactor, true))
				.Replace("{DamageOverTimeDurationIncrease}", Utilities.StatToString(DamageOverTimeDurationIncreasePercentage, rarityFactor, true))
				.Replace("{Rerolls}", (Rerolls == 0 ? 0 : Rerolls + (rarity - 1)).ToString())
				.Replace("{Skips}", (Skips == 0 ? 0 : Skips + (rarity - 1)).ToString())
				;
		}
		
		public ICollection<StatsDisplayData> GetStatsDisplayData()
		{
			var stats = new List<StatsDisplayData>
			{
				new("Health", HealthMax),
				new("Health regen", HealthRegen),
				new("Projectiles", AttackCount),
				new("CDR", CooldownReduction),
				new("CDR%", CooldownReductionPercentage, isPercentage: true),
				new("Damage", Damage),
				new("Damage%", DamagePercentageIncrease, isPercentage: true),
				new("DamageOverTime", DamageOverTime),
				new("DoT frequency", DamageOverTimeFrequencyReductionPercentage, isPercentage: true),
				new("DoT duration", DamageOverTimeDurationIncreasePercentage, isPercentage: true),
				new("Crit rate", CritRate, isPercentage: true),
				new("Crit damage", CritDamage, isPercentage: true),
				new("Magnet", MagnetSize),
				new("Projectile size", Scale, isPercentage: true),
				new("Projectile speed", Speed),
				new("Attack duration", TimeToLive),
				new("Weapon range", DetectionRange),
				new("EXP%", ExperienceIncreasePercentage, isPercentage: true),
				new("Movement speed", MovementSpeed),
				new("Skill CDR%", SkillCooldownReductionPercentage, isPercentage: true),
				new("Pass through", PassThroughCount),
				new("Armor", Armor),
				new("Enemy speed%", EnemySpeedIncreasePercentage, isPercentage: true, isInvertedColor: true),
				new("Enemy spawn rate%", EnemySpawnRateIncreasePercentage, isPercentage: true, isInvertedColor: true),
				new("Enemy health%", EnemyHealthIncreasePercentage, isPercentage: true, isInvertedColor: true),
				new("Enemy count%", EnemyMaxCountIncreasePercentage, isPercentage: true, isInvertedColor: true),
				new("Reward increase", ItemRewardIncrease, isPercentage: true),
				new("Revives", Revives),
				new("Weapon duration%", ProjectileLifeTimeIncreasePercentage, isPercentage: true),
				new("Dodge chance%", DodgeChance, isPercentage: true),
				new("Damage increase%", DamageTakenIncreasePercentage, isPercentage: true),
				new("Life steal%", LifeSteal, isPercentage: true),
				new("Heal increase%", HealingReceivedIncreasePercentage, isPercentage: true),
				new("Luck%", Luck, isPercentage: true),
				new("Rerolls", Rerolls),
				new("Skips", Skips),
			};
			return stats;
		}
		
		private float GetRarityFactor(float rarity)
		{
			const float percentIncreasePerRarity = 0.10f;
			return 1.0f + ((rarity - 1.0f) * percentIncreasePerRarity);
		}
	}
}