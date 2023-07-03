using System;
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
	}
}