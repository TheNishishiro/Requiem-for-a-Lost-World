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
		
		public void Damage(float amount)
		{
			if (amount > 0)
			{
				amount *= playerStatsComponent.GetDamageTakenIncrease();
				amount -= playerStatsComponent.GetArmor();
				if (amount < 0)
					amount = 0;
				
				AudioManager.instance.PlayPlayerHitAudio();
				AchievementManager.instance.OnDamageTaken(amount);
				DamageTakenEvent.Invoke(amount);
			}
			else if (amount < 0)
			{
				amount *= playerStatsComponent.GetHealingIncrease();
				AchievementManager.instance.OnHealing(amount);
			}
			
			playerStatsComponent.TakeDamage(amount);
			UpdateHealthBar();
			
			if (playerStatsComponent.IsDead())
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
			if (playerStatsComponent.IsFullHealth()) return;
			playerStatsComponent.ApplyRegeneration();

			UpdateHealthBar();
		}

		public void UpdateHealthBar()
		{
			healthBar.UpdateSlider(playerStatsComponent.GetHealth(), playerStatsComponent.GetMaxHealth());
		}
		
		private void Death()
		{
			if (playerStatsComponent.GetRevives() > 0)
			{
				playerStatsComponent.UseRevive();
				playerStatsComponent.SetHealth(playerStatsComponent.GetMaxHealth() / 2);
				UpdateHealthBar();
				return;
			}
			
			AchievementManager.instance.OnDeath();
			gameOverScreenManager.OpenPanel(false);
		}

		public void IncreaseMaxHealth(float amount)
		{
			playerStatsComponent.IncreaseMaxHealth(amount);
		}
	}
}