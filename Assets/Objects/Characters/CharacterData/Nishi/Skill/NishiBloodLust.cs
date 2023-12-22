using System;
using System.Linq;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

		private ObjectPool<GameObject> _objectPool;
		private Vector3 _flamePosition;

		protected override void Awake()
		{
			_objectPool = new ObjectPool<GameObject>(
				() => SpawnManager.instance.SpawnObject(_flamePosition, blackFlamePrefab.gameObject), 
				o =>
				{
					o.transform.position = _flamePosition;
					o.gameObject.SetActive(true);
				}, 
				o =>o.gameObject.SetActive(false), 
				o => Destroy(o.gameObject), 
				true, 75);
			
			base.Awake();
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
			var enemies = EnemyManager.instance.GetActiveEnemies().OrderBy(_ => Random.value).Take(50);
			foreach (var enemy in enemies)
			{
				_flamePosition = enemy.transform.position;
				_objectPool.Get();
			}
			
			if (GameData.GetPlayerCharacterRank() < CharacterRank.E5)
			{
				var damage = GameData.GetPlayerCharacterRank() >= CharacterRank.E5 ? 45 : 20;
				if (PlayerStatsComponent.GetHealth() - damage >= PlayerStatsComponent.GetMaxHealth() * 0.15)
				{
					PlayerStatsComponent.TakeDamage(damage);
				}
			}

			SpecialBarManager.ResetBar();
		}
	}
}