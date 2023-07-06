using System;
using System.Collections.Generic;
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
		public float HealingReceivedIncreasePercentage;
		public float Luck;
		public float DamageOverTime;
		
		public void ApplyRarity(int rarity)
		{
			var rarityFactor = 1 + ((rarity - 1) * 0.1f); 

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
		}
		
		public void Apply(ItemStats itemUpgradeItemStats, int rarity)
        {
	        var rarityFactor = 1 + ((rarity - 1) * 0.1f); 
	        
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
        }
		
		public ICollection<StatsDisplayData> GetDescription()
		{
			var stats = new List<StatsDisplayData>
			{
				new("Health", HealthMax),
				new("Magnet", MagnetSize),
				new("CDR", CooldownReduction),
				new("CDR%", CooldownReductionPercentage, true),
				new("Projectiles", AttackCount),
				new("Damage", Damage),
				new("DamageOverTime", DamageOverTime),
				new("Projectile size", Scale, true),
				new("Projectile speed", Speed),
				new("Attack duration", TimeToLive),
				new("Weapon range", DetectionRange),
				new("Damage%", DamagePercentageIncrease, true),
				new("EXP%", ExperienceIncreasePercentage, true),
				new("Movement speed", MovementSpeed),
				new("Skill CDR%", SkillCooldownReductionPercentage, true),
				new("Health regen", HealthRegen),
				new("Crit rate", CritRate, true),
				new("Crit damage", CritDamage, true),
				new("Pass through", PassThroughCount),
				new("Armor", Armor),
				new("Enemy speed%", EnemySpeedIncreasePercentage, true, true),
				new("Enemy spawn rate%", EnemySpawnRateIncreasePercentage, true, true),
				new("Enemy health%", EnemyHealthIncreasePercentage, true, true),
				new("Enemy count%", EnemyMaxCountIncreasePercentage, true, true),
				new("Reward increase", ItemRewardIncrease, true),
				new("Revives", Revives),
				new("Weapon duration%", ProjectileLifeTimeIncreasePercentage, true),
				new("Dodge chance%", DodgeChance, true),
				new("Damage increase%", DamageTakenIncreasePercentage, true),
				new("Heal increase%", HealingReceivedIncreasePercentage, true),
				new("Luck%", Luck, true),
			};
			return stats;
		}
	}
}