using System;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Characters.Nishi.Skill
{
	public class NishiBloodLust : CharacterSkillBase, IEnemyDiedHandler, ISpecialBarFilledHandler
	{
		[SerializeField] private BlackFlameComponent blackFlamePrefab;
		private SpecialBarManager _specialBarManager;
		private SpecialBarManager SpecialBarManager
		{
			get
			{
				if (_specialBarManager == null)
					_specialBarManager = FindObjectOfType<SpecialBarManager>();
				return _specialBarManager;
			}
		}
		
		private PlayerStatsComponent _playerStatsComponent;
		private PlayerStatsComponent PlayerStatsComponent
		{
			get
			{
				if (_playerStatsComponent == null)
					_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
				return _playerStatsComponent;
			}
		}
		
		
		private void OnEnable()
		{
			EnemyDiedEvent.Register(this);
			SpecialBarFilledEvent.Register(this);
		}
    
		private void OnDisable()
		{
			EnemyDiedEvent.Unregister(this);
			SpecialBarFilledEvent.Unregister(this);
		}
		
		public void OnEnemyDied()
		{
			SpecialBarManager.Increment();
		}

		public void OnSpecialBarFilled()
		{
			var enemies = EnemyManager.instance.GetActiveEnemies();
			foreach (var enemy in enemies)
			{
				SpawnManager.instance.SpawnObject(enemy.transform.position, blackFlamePrefab.gameObject);
			}
			
			var damage = GameData.GetPlayerCharacterRank() >= CharacterRank.E5 ? 45 : 20;
			
			if (PlayerStatsComponent.GetHealth() - damage >= PlayerStatsComponent.GetMaxHealth() * 0.15)
			{
				PlayerStatsComponent.TakeDamage(damage);
			}

			SpecialBarManager.ResetBar();
		}
	}
}