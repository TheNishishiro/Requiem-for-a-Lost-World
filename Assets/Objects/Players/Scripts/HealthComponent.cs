using System;
using Managers;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Players.Scripts
{
	public class HealthComponent : MonoBehaviour
	{
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		[SerializeField] private ExperienceBar healthBar;
		[SerializeField] private GameOverScreenManager gameOverScreenManager;
		private float healthRegenCooldown = 5;
		private float healthRegenCurrentCooldown = 5;
		
		public void Damage(int amount)
		{
			if (amount > 0)
			{
				amount -= playerStatsComponent.GetArmor();
				if (amount < 0)
					amount = 0;
			}
			
			playerStatsComponent.TakeDamage(amount);
			UpdateHealthBar();
			
			if (playerStatsComponent.IsDead())
				Death();
		}

		public void Update()
		{
			healthRegenCurrentCooldown -= Time.deltaTime;
			if (healthRegenCurrentCooldown >= 0) return;
			
			healthRegenCurrentCooldown = healthRegenCooldown;
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
			
			gameOverScreenManager.OpenPanel(false);
		}
	}
}