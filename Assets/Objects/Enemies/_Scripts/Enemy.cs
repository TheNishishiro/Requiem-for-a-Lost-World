using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
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
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Objects.Enemies
{
	public class Enemy : NetworkBehaviour
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
		[SerializeField] private NetworkTransform networkTransport;
		public Transform TargetPoint => damageableComponent.targetPoint.transform;
		private NetworkVariable<EnemyTypeEnum> enemyType = new ();
		private EnemyStats stats;
		private NetworkVariable<float> damage = new();
		private Transform playerTarget;
		private const float DamageCooldown = 0.5f;
		private float _currentDamageCooldown = 0;
		private float _timeAlive = 0;
		private bool _isBossEnemy = false;
		private NetworkVariable<float> removeCollisionsTimer = new ();
		private NetworkVariable<bool> isRemoveCollisions = new ();
		private NetworkVariable<bool> isGrandOcti = new ();
		private float _damageReduction;
		private NetworkVariable<bool> _isDying= new ();
		private bool _isPlayerControlled;
		private float? _playerControlDuration;
		private List<Collision> _ignoredEnemyColliders = new();

		private ChanceDrop chestDropChance;
		private ChanceDrop expDropChance;
		private ChanceDrop goldDropChance;
		private ChanceDrop gemDropChance;
		private ChanceDrop healingOrbDropChance;

		public override void OnNetworkSpawn()
		{
			spriteRenderer.sprite = EnemyManager.instance.GetSpriteByEnemy(enemyType.Value);
			enemyType.OnValueChanged += OnTypeChange;
			isGrandOcti.OnValueChanged += OnBossChanged;
		}

		public override void OnNetworkDespawn()
		{
			enemyType.OnValueChanged -= OnTypeChange;
		}

		private void OnTypeChange(EnemyTypeEnum oldType, EnemyTypeEnum newType)
		{
			spriteRenderer.sprite = EnemyManager.instance.GetSpriteByEnemy(newType);
		}

		private void OnBossChanged(bool oldValue, bool newValue)
		{
			if (newValue)
				SetupBoss();
		}

		public void Setup(EnemyNetworkStats newStats, float healthMultiplier, float speedMultiplier, float expDropMultiplier, float damageMultiplier)
		{
			damageableComponent.Clear();
			chaseComponent.Clear();
			dissolveController.Clear();
			capsuleCollider.enabled = true;

			_timeAlive = 0;
			_currentDamageCooldown = 0;
			_damageReduction = 0;
			removeCollisionsTimer.Value = 0;
			isRemoveCollisions.Value = false;
			RevertIgnoredCollisions();
			_ignoredEnemyColliders.Clear();
			_isDying.Value = false;
			_isPlayerControlled = false;
			_isBossEnemy = false;
			_playerControlDuration = null;
			curseParticleSystem.gameObject.SetActive(false);

			enemyType.Value = newStats.enemyType;
			spriteRenderer.transform.localPosition = new Vector3(0, newStats.groundOffset, 0);
			grandOctiBoss.SetActive(false);
			stats ??= new EnemyStats();
			stats.Copy(newStats);
			dropOnDestroyComponent.ClearDrop();

			if (newStats.expDrop > 0)
			{
				expDropChance ??= new ChanceDrop()
				{
					pickupObject = expDrop,
				};
				expDropChance.amount = (int)(newStats.expDrop * expDropMultiplier);
				expDropChance.chance = 0.75f;
				dropOnDestroyComponent.AddDrop(expDropChance);
			}

			if (newStats.isBossEnemy)
			{
				_isBossEnemy = true;
				stats.hp *= GameManager.instance.playerComponent?.GetLevel() ?? 1;

				dropOnDestroyComponent.AddDrop(chestDropChance ??= new ChanceDrop()
				{
					chance = 1,
					pickupObject = chestDrop,
				});
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

			stats.hp = (int)(stats.hp * healthMultiplier * PlayerStatsScaler.GetScaler().GetEnemyHealthIncrease()) * GameData.GetCurrentDifficulty().EnemyHealthModifier;
			stats.speed *= speedMultiplier;
			stats.speed *= PlayerStatsScaler.GetScaler().GetEnemySpeedIncrease();
			stats.speed *= GameData.GetCurrentDifficulty().EnemySpeedModifier;
			stats.damage *= GameData.GetCurrentDifficulty().EnemyDamageModifier * damageMultiplier;
			damage.Value = stats.damage;
			chaseComponent.FollowYAxis = newStats.allowFlying;
			chaseComponent.SetSpeed(stats.speed);
			damageableComponent.SetHealth(stats.hp);
			damageableComponent.SetResistances(stats.elementStats);
			networkTransport.Interpolate = true;
			
		}

		public void SetPlayerTarget(Transform targetClient)
		{
			playerTarget = targetClient;
			SetChaseTarget(playerTarget.gameObject);
		}

		public void SetupBoss()
		{
			if (IsHost) isGrandOcti.Value = true;
			grandOctiBoss.SetActive(true);
		}

		public bool IsBoss()
		{
			return _isBossEnemy;
		}

		private void SetChaseTarget(GameObject target)
		{
			if (!IsHost) return;
			
			chaseComponent.SetTarget(target);
		}

		private void Update()
		{
			if (_isDying.Value) return;

			if (_currentDamageCooldown > 0)
				_currentDamageCooldown -= Time.deltaTime;
			
			if (IsHost) HostUpdate();
			
			else if (removeCollisionsTimer.Value <= 0 && isRemoveCollisions.Value)
			{
				if (IsHost)
					isRemoveCollisions.Value = false;
				RevertIgnoredCollisions();
			}
		}

		private void HostUpdate()
		{
			chaseComponent.SetMovementState(EnemyManager.instance.IsTimeStop());
			_timeAlive += Time.deltaTime;
			
			
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
			
			if (removeCollisionsTimer.Value > 0)
			{
				isRemoveCollisions.Value = true;
				removeCollisionsTimer.Value -= Time.deltaTime;
			}
		}

		private void Die()
		{
			if (_isDying.Value) return;
			networkTransport.Interpolate = false;
			capsuleCollider.enabled = false;
			chaseComponent.SetMovementState(true);
			_isDying.Value = true;
			RpcManager.instance.AddEnemyKillRpc(IsBoss());
			dropOnDestroyComponent.CheckDrop();
			StartCoroutine(DieAnimation());
		}

		private IEnumerator DieAnimation()
		{
			var dissolveTime = 0.25f;
			dissolveController.Dissolve(dissolveTime);
			yield return new WaitForSeconds(0.5f);
			EnemyManager.instance.Despawn(this);
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (_isDying.Value)
				return;

			if (IsHost) HostCollisionDetection(collisionInfo);
			else ClientCollisionDetection(collisionInfo);
		}

		private void ClientCollisionDetection(Collision collisionInfo)
		{
			if (collisionInfo.gameObject.CompareTag("Player") && collisionInfo.gameObject.GetComponent<NetworkObject>()?.IsOwner == true)
			{
				var playerComponent = GameManager.instance.playerComponent;
				Attack(playerComponent);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && isRemoveCollisions.Value)
			{
				_ignoredEnemyColliders.Add(collisionInfo);
				Physics.IgnoreCollision(collisionInfo.collider, GetComponent<CapsuleCollider>(), true);
			}
		}

		private void HostCollisionDetection(Collision collisionInfo)
		{
			if (collisionInfo.gameObject.CompareTag("Player") && collisionInfo.gameObject.GetComponent<NetworkObject>()?.IsOwner == true)
			{
				var playerComponent = GameManager.instance.playerComponent;
				Attack(playerComponent);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && isRemoveCollisions.Value)
			{
				_ignoredEnemyColliders.Add(collisionInfo);
				Physics.IgnoreCollision(collisionInfo.collider, GetComponent<CapsuleCollider>(), true);
			}
			else if (collisionInfo.gameObject.CompareTag("Enemy") && _isPlayerControlled)
			{
				var enemyComponent = collisionInfo.gameObject.GetComponent<Enemy>();
				var isE5Lucy = GameData.IsCharacterWithRank(CharactersEnum.Lucy_BoC, CharacterRank.E5);
				
				var damageIncrease = isE5Lucy ? 2f : 1f;
				enemyComponent.GetDamagableComponent().TakeDamageWithCooldown(new DamageResult{Damage = damage.Value * damageIncrease * PlayerStatsScaler.GetScaler().GetDamageIncreasePercentage() }, collisionInfo.gameObject, DamageCooldown, null);
				
				// Take damage from other enemies
				var damageReduction = isE5Lucy ? 0.3f : 1f;
				GetDamagableComponent().TakeDamageWithCooldown(new DamageResult{Damage = damage.Value * damageReduction}
					, collisionInfo.gameObject, DamageCooldown, null);
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
			if (_currentDamageCooldown > 0 || isRemoveCollisions.Value)
				return;

			_currentDamageCooldown = DamageCooldown;
			player.TakeDamage(damage.Value * (1 + Math.Max(_damageReduction, -1)));
		}

		public void SetNoCollisions(float timer)
		{
			if (IsHost && removeCollisionsTimer.Value < timer)
				removeCollisionsTimer.Value = timer;
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

		public bool IsPlayerControlled()
		{
			return _isPlayerControlled;
		}

		public bool IsDying()
		{
			return _isDying.Value;
		}
	}
}