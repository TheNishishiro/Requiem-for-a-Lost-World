using System;
using Events.Scripts;
using Managers;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Objects.Players.Scripts
{
	public class HealthComponent : MonoBehaviour
	{
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		[SerializeField] private ExperienceBar healthBar;
		[SerializeField] private GameOverScreenManager gameOverScreenManager;
		private const float HealthRegenCooldown = 1;
		private float _healthRegenCurrentCooldown = 1;
		
		public void Damage(float amount, bool isIgnoreArmor = false, bool isPreventDeath = false)
		{
			if (amount > 0)
			{
				amount *= PlayerStatsScaler.GetScaler().GetDamageTakenIncrease();
				if (!isIgnoreArmor)
					amount -= PlayerStatsScaler.GetScaler().GetArmor();
				if (amount < 0)
					amount = 0;
				
				AudioManager.instance.PlayPlayerHitAudio();
				AchievementManager.instance.OnDamageTaken(amount);
			}
			else if (amount < 0)
			{
				amount *= PlayerStatsScaler.GetScaler().GetHealingIncrease();
				AchievementManager.instance.OnHealing(amount);
			}
			
			playerStatsComponent.TakeDamage(amount, isPreventDeath);
			DamageTakenEvent.Invoke(amount);
			UpdateHealthBar();
			
			if (playerStatsComponent.CanDie())
				Death();
		}

		public void Update()
		{
			_healthRegenCurrentCooldown -= Time.deltaTime;
			if (_healthRegenCurrentCooldown >= 0) return;
			
			_healthRegenCurrentCooldown = HealthRegenCooldown;
			Regen();
		}

		private void Regen()
		{
			if (playerStatsComponent.IsDead()) return;
			if (playerStatsComponent.IsFullHealth()) return;

			Damage(-PlayerStatsScaler.GetScaler().GetHealthRegeneration());
		}

		public void UpdateHealthBar()
		{
			healthBar.UpdateSlider(PlayerStatsScaler.GetScaler().GetHealth(), PlayerStatsScaler.GetScaler().GetMaxHealth());
		}
		
		private void Death()
		{
			if (PlayerStatsScaler.GetScaler().GetRevives() > 0)
			{
				playerStatsComponent.UseRevive();
				playerStatsComponent.SetHealth(PlayerStatsScaler.GetScaler().GetMaxHealth() / 2);
				UpdateHealthBar();
				return;
			}
			
			AchievementManager.instance.OnDeath();
			playerStatsComponent.ChangeDeathState(true);
			gameOverScreenManager.OpenPanel(false);
		}

		public void IncreaseMaxHealth(float amount)
		{
			playerStatsComponent.IncreaseMaxHealth(amount);
		}
	}
}