using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Events.Scripts;
using Interfaces;
using Managers;
using Objects.Characters;
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
		[SerializeField] private Damageable damageableComponent;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private CapsuleCollider capsuleCollider;
		[SerializeField] private DropOnDestroy dropOnDestroyComponent;
		[SerializeField] private DissolveController dissolveController;
		[SerializeField] private Pickup chestDrop;
		[SerializeField] private Pickup expDrop;
		[SerializeField] private Pickup goldDrop;
		[SerializeField] private Pickup gemDrop;
		[SerializeField] private Pickup healingOrbDrop;
		[SerializeField] private GameObject grandOctiBoss;
		[SerializeField] private ParticleSystem curseParticleSystem;
		public Transform TargetPoint => damageableComponent.targetPoint.transform;
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
		private bool _isDying;
		private bool _isPlayerControlled;
		private float? _playerControlDuration;
		private List<Collision> _ignoredEnemyColliders = new();

		private ChanceDrop chestDropChance;
		private ChanceDrop expDropChance;
		private ChanceDrop goldDropChance;
		private ChanceDrop gemDropChance;
		private ChanceDrop healingOrbDropChance;

		public void Setup(EnemyData newStats, Player target, PlayerStatsComponent playerStats, float healthMultiplier,
			float speedMultiplier, Sprite sprite)
		{
			damageableComponent.Clear();
			chaseComponent.Clear();
			dissolveController.Clear();
			capsuleCollider.enabled = true;

			_timeAlive = 0;
			_currentDamageCooldown = 0;
			_damageCooldown = 0.5f;
			_removeCollisionsTimer = 0;
			_damageReduction = 0;
			_isRemoveCollisions = false;
			RevertIgnoredCollisions();
			_ignoredEnemyColliders.Clear();
			_isDying = false;
			_isPlayerControlled = false;
			_isBossEnemy = false;
			_playerControlDuration = null;
			curseParticleSystem.gameObject.SetActive(false);

			playerTarget = target;
			spriteRenderer.transform.localPosition = new Vector3(0, newStats.groundOffset, 0);
			spriteRenderer.sprite = sprite;
			grandOctiBoss.SetActive(false);
			stats ??= new EnemyStats();
			stats.Copy(newStats.stats);
			dropOnDestroyComponent.ClearDrop();

			if (newStats.ExpDrop > 0)
			{
				expDropChance ??= new ChanceDrop()
				{
					pickupObject = expDrop,
				};
				expDropChance.amount = newStats.ExpDrop;
				expDropChance.chance = 0.75f;
				dropOnDestroyComponent.AddDrop(expDropChance);
			}

			if (newStats.isBossEnemy)
			{
				_isBossEnemy = true;
				stats.hp *= playerTarget?.GetLevel() ?? 1;

				dropOnDestroyComponent.AddDrop(chestDropChance ??= new ChanceDrop()
				{
					chance = 1,
					pickupObject = chestDrop,
				});
				dropOnDestroyComponent.AddDrop(expDropChance);
			}

			goldDropChance ??= new ChanceDrop()
			{
				pickupObject = goldDrop,
			};
			
			goldDropChance.chance = PlayerStatsScaler.GetScaler().GetLuck() / 16;
			goldDropChance.amount = Random.Range(1, 25);
			dropOnDestroyComponent.AddDrop(goldDropChance);

			if (GameData.IsCharacter(CharactersEnum.Lucy_BoC))
			{
				healingOrbDropChance ??= new ChanceDrop()
				{
					pickupObject = healingOrbDrop,
				};
				healingOrbDropChance.chance = 0.05f;
				healingOrbDropChance.amount = 1;
				dropOnDestroyComponent.AddDrop(healingOrbDropChance);
			}

			gemDropChance ??= new ChanceDrop()
			{
				pickupObject = gemDrop,
			};
			gemDropChance.chance = PlayerStatsScaler.GetScaler().GetLuck() / 100;
			gemDropChance.amount = Random.Range(1, 50);
			dropOnDestroyComponent.AddDrop(gemDropChance);

			stats.hp = (int)(stats.hp * healthMultiplier * PlayerStatsScaler.GetScaler().GetEnemyHealthIncrease());
			stats.speed *= speedMultiplier;
			stats.speed *= PlayerStatsScaler.GetScaler().GetEnemySpeedIncrease();
			chaseComponent.FollowYAxis = newStats.allowFlying;
			SetChaseTarget(target.gameObject);
		}

		public void SetupBoss()
		{
			grandOctiBoss.SetActive(true);
		}

		public bool IsBoss()
		{
			return _isBossEnemy;
		}

		private void SetChaseTarget(GameObject target)
		{
			targetGameObject = target;
			chaseComponent.SetTarget(target);
			chaseComponent.SetSpeed(stats.speed);
			damageableComponent.SetHealth(stats.hp);
			damageableComponent.SetResistances(stats.elementStats);
		}

		private void Update()
		{
			if (_isDying) return;

			chaseComponent.SetMovementState(EnemyManager.instance.IsTimeStop());
			_timeAlive += Time.deltaTime;

			if (_currentDamageCooldown > 0)
				_currentDamageCooldown -= Time.deltaTime;
			if (_timeAlive > 90 && !_isBossEnemy)
			{
				EnemyManager.instance.Despawn(this);
			}

			if (damageableComponent.IsDestroyed())
				Die();

			if (_isPlayerControlled && _playerControlDuration.HasValue)
			{
				_playerControlDuration -= Time.deltaTime;
				if (_playerControlDuration <= 0)
				{
					TogglePlayerControl(false);
				}
			}
			
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
			if (_isDying) return;

			capsuleCollider.enabled = false;
			chaseComponent.SetMovementState(true);
			_isDying = true;
			gameResultData.MonstersKilled++;
			EnemyDiedEvent.Invoke();
			dropOnDestroyComponent.CheckDrop();
			AchievementManager.instance.OnEnemyKilled(this);
			StartCoroutine(DieAnimation());
		}

		private IEnumerator DieAnimation()
		{
			var dissolveTime = 0.25f;
			dissolveController.Dissolve(dissolveTime);
			yield return new WaitForSeconds(0.5f);
			EnemyManager.instance.Despawn(this);
		}

		public bool IsDead()
		{
			return _isDying;
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (_isDying)
				return;

			if (collisionInfo.gameObject.CompareTag("Player"))
			{
				var playerComponent = GameManager.instance.playerComponent;
				Attack(playerComponent);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && _isRemoveCollisions)
			{
				_ignoredEnemyColliders.Add(collisionInfo);
				Physics.IgnoreCollision(collisionInfo.collider, GetComponent<CapsuleCollider>(), true);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && _isPlayerControlled)
			{
				var enemyComponent = collisionInfo.gameObject.GetComponent<Enemy>();
				var isE5Lucy = GameData.IsCharacterWithRank(CharactersEnum.Lucy_BoC, CharacterRank.E5);
				
				var damageIncrease = isE5Lucy ? 2f : 1f;
				enemyComponent.GetDamagableComponent().TakeDamageWithCooldown(new DamageResult{Damage = stats.damage * damageIncrease * PlayerStatsScaler.GetScaler().GetDamageIncreasePercentage() }, collisionInfo.gameObject, _damageCooldown, null);
				
				// Take damage from other enemies
				var damageReduction = isE5Lucy ? 0.3f : 1f;
				GetDamagableComponent().TakeDamageWithCooldown(new DamageResult{Damage = enemyComponent.GetDamage() * damageReduction}
					, collisionInfo.gameObject, _damageCooldown, null);
			}
		}

		private void RevertIgnoredCollisions()
		{
			var collider = capsuleCollider;
			for (var index = 0; index < _ignoredEnemyColliders.Count; index++)
			{
				var ignoredEnemyCollider = _ignoredEnemyColliders[index];
				if (ignoredEnemyCollider?.collider != null && collider != null)
					Physics.IgnoreCollision(ignoredEnemyCollider.collider, collider, false);

				_ignoredEnemyColliders.RemoveAt(index--);
			}

			_ignoredEnemyColliders.Clear();
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

		public void MarkAsPlayerControlled(float? duration)
		{
			if (grandOctiBoss.gameObject.activeSelf)
				return;
			
			_playerControlDuration = duration;
			TogglePlayerControl(true);
		}

		private void TogglePlayerControl(bool isActive)
		{
			_isPlayerControlled = true;
			GetChaseComponent().MarkAsPlayerControlled(isActive);
			curseParticleSystem.gameObject.SetActive(isActive);
			if (isActive)
				GetChaseComponent().SetTarget(EnemyManager.instance.GetUncontrolledClosestEnemy(transform.position)?.gameObject);
			else
				chaseComponent.SetTarget(playerTarget.gameObject);
		}
		
		public void AddDamageReduction(float amount)
		{
			_damageReduction -= amount;
		}

		public Damageable GetDamagableComponent()
		{
			return damageableComponent;
		}

		public ChaseComponent GetChaseComponent()
		{
			return chaseComponent;
		}

		public Collider GetCollider()
		{
			return capsuleCollider;
		}

		public float GetDamage()
		{
			return stats.damage;
		}

		public bool IsPlayerControlled()
		{
			return _isPlayerControlled;
		}
	}
}