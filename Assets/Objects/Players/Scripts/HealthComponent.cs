using System;
using System.Linq;
using Events.Scripts;
using Managers;
using UI.In_Game.GUI.Scripts.Managers;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Objects.Players.Scripts
{
	public class HealthComponent : MonoBehaviour
	{
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		[SerializeField] private GameOverScreenManager gameOverScreenManager;
		private const float HealthRegenCooldown = 1;
		private float _healthRegenCurrentCooldown = 1;

		private void Start()
		{
			UpdateHealthBar();
		}

		public void Damage(float amount, bool isIgnoreArmor = false, bool isPreventDeath = false)
		{
			if (playerStatsComponent.IsDead()) return;
			
			if (amount > 0)
			{
				amount *= PlayerStatsScaler.GetScaler().GetDamageTakenIncrease();
				if (!isIgnoreArmor)
					amount *= 1f - PlayerStatsScaler.GetScaler().GetDamageReduction();
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
			UpdateHealthBar();
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
			GuiManager.instance.UpdateHealth(PlayerStatsScaler.GetScaler().GetHealth(), PlayerStatsScaler.GetScaler().GetMaxHealth());
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

			var players = FindObjectsByType<MultiplayerPlayer>(FindObjectsSortMode.None);
			if (players.Length <= 1)
				gameOverScreenManager.OpenPanel(false);
			else if (players.Where(x => x != NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<MultiplayerPlayer>())
			         .All(x => x.isPlayerDead.Value))
				RpcManager.instance.TriggerLoseServerRpc();
			else
				RpcManager.instance.SpawnReviveCardRpc(GameManager.instance.PlayerTransform.position, NetworkManager.Singleton.LocalClientId);
		}

		public void IncreaseMaxHealth(float amount)
		{
			playerStatsComponent.IncreaseMaxHealth(amount);
		}
	}
}