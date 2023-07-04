using System;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using Objects.Drops;
using Objects.Drops.ChestDrop;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Objects.Enemies
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField] private ChaseComponent chaseComponent;
		[SerializeField] private GameResultData gameResultData;
		[SerializeField] private GameObject chestDrop;
		[SerializeField] private GameObject expDrop;
		[SerializeField] private GameObject goldDrop;
		[SerializeField] private GameObject gemDrop;
		[SerializeField] private UnityEvent<Enemy> OnEnemyDeath;
		private EnemyManager _enemyManager;
		private Damageable damageable;
		private EnemyStats stats;
		private GameObject targetGameObject;
		private Player playerTarget;
		private float _damageCooldown = 0.5f;
		private float _currentDamageCooldown = 0;
		private float _timeAlive = 0;
		private bool _isBossEnemy = false;
		private float _removeCollisionsTimer;
		private bool _isRemoveCollisions;
		private float _damageReduction;
		private List<Collision> _ignoredEnemyColliders = new List<Collision>();


		public void Setup(EnemyData newStats, Player target, EnemyManager enemyManager, PlayerStatsComponent playerStats)
		{
			_enemyManager = enemyManager;
			playerTarget = target;
			stats = new EnemyStats(newStats.stats);
			var dropOnDestroyComponent = GetComponentInChildren<DropOnDestroy>();
			if (newStats.isBossEnemy)
			{
				_isBossEnemy = true;
				stats.hp *= playerTarget?.GetLevel() ?? 1;
				dropOnDestroyComponent.AddDrop(new ChanceDrop()
				{
					chance = 1,
					gameObject = chestDrop,
				});
				dropOnDestroyComponent.AddDrop(new ChanceDrop()
				{
					chance = 0.75f,
					amount = newStats.ExpDrop,
					gameObject = expDrop,
				});
			}
			else
			{
				if (newStats.ExpDrop > 0)
				{
					dropOnDestroyComponent.AddDrop(new ChanceDrop()
					{
						chance = 1f,
						amount = newStats.ExpDrop,
						gameObject = expDrop,
					});
				}
			}

			dropOnDestroyComponent.AddDrop(new ChanceDrop()
			{
				chance = (playerTarget?.playerStatsComponent?.GetLuck() ?? 0) / 8,
				amount = Random.Range(1, 25),
				gameObject = goldDrop,
			});
			dropOnDestroyComponent.AddDrop(new ChanceDrop()
			{
				chance = (playerTarget?.playerStatsComponent?.GetLuck() ?? 0) / 16,
				amount = Random.Range(1, 50),
				gameObject = gemDrop,
			});
			
			stats.hp = (int)(stats.hp * playerStats.GetEnemyHealthIncrease());
			stats.speed *= playerStats.GetEnemySpeedIncrease();
			chaseComponent.FollowYAxis = newStats.allowFlying;
			SetChaseTarget(target.gameObject);
		}

		private void SetChaseTarget(GameObject target)
		{
			targetGameObject = target;
			chaseComponent.SetTarget(target);
			chaseComponent.SetSpeed(stats.speed);
			damageable = GetComponentInChildren<Damageable>();
			if (damageable == null)
				Debug.LogWarning($"Enemy {gameObject.name} could not find a damagable component");

			if (damageable != null) 
				damageable.SetHealth(stats.hp);
		}

		private void Update()
		{
			chaseComponent.SetMovementState(_enemyManager.IsTimeStop());
			_timeAlive += Time.deltaTime;
			
			if (_currentDamageCooldown > 0)
				_currentDamageCooldown -= Time.deltaTime;
			if (_timeAlive > 90 && !_isBossEnemy)
			{
				_enemyManager.EnemyDespawn();
				Destroy(gameObject);
			}
			
			if (damageable.IsDestroyed())
				Die();

			if (_removeCollisionsTimer > 0)
			{
				_isRemoveCollisions = true;
				_removeCollisionsTimer -= Time.deltaTime;
			}
			else if (_removeCollisionsTimer <= 0 && _isRemoveCollisions)
			{
				_isRemoveCollisions = false;
				RevertIgnoredCollisions();
			}
		}

		private void Die()
		{
			gameResultData.MonstersKilled++;
			GetComponentInChildren<DropOnDestroy>()?.CheckDrop();
			_enemyManager.EnemyDespawn();
			OnEnemyDeath?.Invoke(this);
			Destroy(gameObject);
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (collisionInfo.gameObject.CompareTag("Player"))
			{
				var playerComponent = collisionInfo.gameObject.GetComponent<Player>();
				Attack(playerComponent);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && _isRemoveCollisions)
			{
				_ignoredEnemyColliders.Add(collisionInfo);
				Physics.IgnoreCollision(collisionInfo.collider, GetComponentInChildren<Collider>(), true);
			}
		}
		
		private void RevertIgnoredCollisions()
		{
			var collider = GetComponentInChildren<Collider>();
			for (var index = 0; index < _ignoredEnemyColliders.Count; index++)
			{
				var ignoredEnemyCollider = _ignoredEnemyColliders[index];
				if (ignoredEnemyCollider?.collider != null && collider != null)
					Physics.IgnoreCollision(ignoredEnemyCollider.collider, collider, false);
				
				_ignoredEnemyColliders.RemoveAt(index--);
			}

			_ignoredEnemyColliders = new List<Collision>();
		}

		private void Attack(Player player)
		{
			if (_currentDamageCooldown > 0 || _isRemoveCollisions)
				return;

			_currentDamageCooldown = _damageCooldown;
			player.TakeDamage(stats.damage * (1 + Math.Max(_damageReduction, -1)));
		}

		public void SetNoCollisions(float timer)
		{
			if (_removeCollisionsTimer < timer)
				_removeCollisionsTimer = timer;
		}

		public void AddDamageReduction(float amount)
		{
			_damageReduction -= amount;
		}
	}
}