using System;
using Data.Elements;
using DefaultNamespace.Data.Weapons;
using Managers;
using Objects.Players;
using Random = UnityEngine.Random;

namespace Objects.Characters
{
    public class CharacterScalingStrategyBase
    {
	    protected static PlayerStats PlayerStats => GameManager.instance.playerStatsComponent.GetStats();

	    public virtual DamageResult GetDamageDealt(float damageIncrease = 0)
	    {
		    var damageResult = new DamageResult
		    {
			    IsCriticalHit = Random.value < GetCritRate()
		    };

		    damageResult.Damage = (float)(damageResult.IsCriticalHit
			    ? GetDamage() * (GetDamageIncreasePercentage() + damageIncrease) * GetCritDamage()
			    : GetDamage() * (GetDamageIncreasePercentage() + damageIncrease));

		    return damageResult;
	    }
        
        public virtual float GetHealth()
        {
            return PlayerStats?.Health ?? 0;
        }

        public virtual float GetMaxHealth()
        {
            return PlayerStats?.HealthMax ?? 0;
        }
		
        public virtual float GetEnemyHealthIncrease()
        {
            return 1 + PlayerStats?.EnemyHealthIncreasePercentage ?? 0;
        }
		
        public virtual float GetEnemySpawnRateIncrease()
        {
            return 1 + PlayerStats?.EnemySpawnRateIncreasePercentage ?? 0;
        }
		
        public virtual float GetEnemyCountIncrease()
        {
            return 1 + PlayerStats?.EnemyMaxCountIncreasePercentage ?? 0;
        }
		
        public virtual float GetEnemySpeedIncrease()
        {
            return 1 + PlayerStats?.EnemySpeedIncreasePercentage ?? 0;
        }
		
        public virtual float GetExperienceIncrease()
        {
            return 1 + PlayerStats?.ExperienceIncreasePercentage ?? 0;
        }

        public virtual float GetMovementSpeed()
        {
            return PlayerStats?.MovementSpeed ?? 0;
        }

        public virtual float GetDamageReduction()
        {
            return PlayerStats?.Armor ?? 0;
        }

        public virtual float GetSkillCooldownReductionPercentage()
        {
            return 1 - PlayerStats?.SkillCooldownReductionPercentage ?? 0;
        }
        
        public virtual double GetMagnetSize()
		{
			return PlayerStats?.MagnetSize ?? 0;
		}

		public virtual float GetCooldownReductionPercentage()
		{
			return 1 - (PlayerStats?.CooldownReductionPercentage ?? 0);
		}

		public virtual float GetCooldownReduction()
		{
			return PlayerStats?.CooldownReduction ?? 0;
		}

		public virtual int GetAttackCount()
		{
			return PlayerStats?.AttackCount ?? 0;
		}

		public virtual float GetDamageIncreasePercentage()
		{
			return 1 + (PlayerStats?.DamagePercentageIncrease ?? 0);
		}

		public virtual float GetDamage()
		{
			return PlayerStats?.Damage ?? 0;
		}

		public virtual double GetCritRate()
		{
			return PlayerStats?.CritRate ?? 0;
		}

		public virtual double GetCritDamage()
		{
			return 1 + (PlayerStats?.CritDamage ?? 0.5);
		}

		public virtual float GetScale()
		{
			return PlayerStats?.Scale ?? 0;
		}

		public virtual float GetProjectileSpeed()
		{
			return PlayerStats?.Speed ?? 0;
		}

		public virtual float GetProjectileLifeTime()
		{
			return PlayerStats?.TimeToLive ?? 0;
		}

		public virtual float GetProjectileDetectionRange()
		{
			return PlayerStats?.DetectionRange ?? 0;
		}

		public virtual int GetProjectilePassThroughCount()
		{
			return PlayerStats?.PassThroughCount ?? 0;
		}

		public virtual float GetItemRewardIncrease()
		{
			return 1 + (PlayerStats?.ItemRewardIncrease ?? 0);
		}
		
		public virtual int GetRevives()
		{
			return PlayerStats?.Revives ?? 0;
		}

		public virtual float GetProjectileLifeTimeIncreasePercentage()
		{
			return 1 + (PlayerStats?.ProjectileLifeTimeIncreasePercentage ?? 0);
		}

		public virtual float GetDodgeChance()
		{
			return PlayerStats?.DodgeChance ?? 0;
		}

		public virtual float GetDamageTakenIncrease()
		{
			return 1 + (PlayerStats?.DamageTakenIncreasePercentage ?? 0);
		}

		public virtual float GetHealingIncrease()
		{
			return 1 + (PlayerStats?.HealingIncreasePercentage ?? 0);
		}

		public virtual float GetLuck()
		{
			return PlayerStats?.Luck ?? 0;
		}

		public virtual float GetDamageOverTime()
		{
			return PlayerStats?.DamageOverTime ?? 0;
		}

		public virtual float GetLifeSteal()
		{
			return PlayerStats?.LifeSteal ?? 0;
		}

		public virtual bool HasRerolls()
		{
			return PlayerStats?.Rerolls > 0;
		}

		public virtual bool HasSkips()
		{
			return PlayerStats?.Skips > 0;
		}

		public virtual int GetSkips()
		{
			return PlayerStats?.Skips ?? 0;
		}

		public virtual int GetRerolls()
		{
			return PlayerStats?.Rerolls ?? 0;
		}

		public virtual float GetSpecialMaxValue()
		{
			return PlayerStats?.SpecialMax ?? 0;
		}

		public virtual float GetSpecialIncrementAmount()
		{
			return PlayerStats?.SpecialIncrease ?? 0;
		}

		public virtual float GetDamageOverTimeFrequencyReductionPercentage()
		{
			return (1 - PlayerStats?.DamageOverTimeFrequencyReductionPercentage ?? 0);
		}

		public virtual float GetDamageOverTimeDurationIncreasePercentage()
		{
			return (1 + PlayerStats?.DamageOverTimeDurationIncreasePercentage ?? 0);
		}

		public virtual float GetHealthRegeneration()
		{
			return PlayerStats?.HealthRegen ?? 0;
		}

		public float GetElementalDamageIncrease(Element weaponElement)
		{
			var damageIncrease = 1.0f;
			switch (weaponElement)
			{
				case Element.Fire:
					damageIncrease += GetFireDamageIncrease();
					break;
				case Element.Lightning:
					damageIncrease += GetLightningDamageIncrease();
					break;
				case Element.Ice:
					damageIncrease += GetIceDamageIncrease();
					break;
				case Element.Physical:
					damageIncrease += GetPhysicalDamageIncrease();
					break;
				case Element.Wind:
					damageIncrease += GetWindDamageIncrease();
					break;
				case Element.Light:
					damageIncrease += GetLightDamageIncrease();
					break;
				case Element.Cosmic:
					damageIncrease += GetCosmicDamageIncrease();
					break;
				case Element.Earth:
					damageIncrease += GetEarthDamageIncrease();
					break;
			}

			return damageIncrease;
		}

		public virtual float GetFireDamageIncrease()
		{
			return PlayerStats?.FireDamageIncrease ?? 0;
		}

		public virtual float GetLightningDamageIncrease()
		{
			return PlayerStats?.LightningDamageIncrease ?? 0;
		}

		public virtual float GetIceDamageIncrease()
		{
			return PlayerStats?.IceDamageIncrease ?? 0;
		}

		public virtual float GetPhysicalDamageIncrease()
		{
			return PlayerStats?.PhysicalDamageIncrease ?? 0;
		}

		public virtual float GetWindDamageIncrease()
		{
			return PlayerStats?.WindDamageIncrease ?? 0;
		}

		public virtual float GetLightDamageIncrease()
		{
			return PlayerStats?.LightDamageIncrease ?? 0;
		}

		public virtual float GetCosmicDamageIncrease()
		{
			return PlayerStats?.CosmicDamageIncrease ?? 0;
		}

		public virtual float GetEarthDamageIncrease()
		{
			return PlayerStats?.EarthDamageIncrease ?? 0;
		}

		public float GetMissingHealthPercentage()
		{
			return 1 - GetHealth() / GetMaxHealth();
		}

		public float GetMissingHealth()
		{
			return GetMaxHealth() - GetHealth();
		}

		public int GetDashCount()
		{
			return PlayerStats?.DashCount ?? 0;
		}

		public float GetStamina()
		{
			return PlayerStats?.Stamina ?? 0;
		}

		public float GetArmor()
		{
			return PlayerStats?.Armor ?? 0;
		}
    }
}