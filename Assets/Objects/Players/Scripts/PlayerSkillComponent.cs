using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Managers;
using Objects.Abilities;
using Objects.Abilities.Laser_Gun;
using Objects.Characters;
using Objects.Characters.Chronastra.Skill;
using Objects.Characters.Nishi.Skill;
using Objects.Enemies;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Objects.Players.Scripts
{
	public class PlayerSkillComponent : MonoBehaviour
	{
		[SerializeField] private CharacterController controller;
		[SerializeField] private Image skillCooldownImage;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		[SerializeField] private BoxCollider playerCollider;
		[SerializeField] private Transform abilityContainer;
		[SerializeField] private HealthComponent healthComponent;
		[SerializeField] private SpecialBar specialBar;
		[SerializeField] private AbilityDurationBar abilityDurationBar;
		[SerializeField] private WeaponManager _weaponManager;
		private AmeliaGlassShield _ameliaGlassShield;
		private float _currentSkillCooldown = 0f;
		private float _skillCooldown = 5f;
		private float _dashDuration = 0;
		private float _dashDistance = 10;
		private Transform _transform;
		private Vector3 _dashPosition;
		private Queue<Vector3> _previousPositions = new ();
		private float _positionRecordTimer;
		private bool _applyQueuedPosition;

		public void Start()
		{
			_skillCooldown = GameData.GetCharacterSkillCooldown();
			_transform = transform;
			ApplySpecial();
		}

		public void Update()
		{
			if (_currentSkillCooldown > 0f)
			{
				skillCooldownImage.fillAmount = _currentSkillCooldown / _skillCooldown;
				_currentSkillCooldown -= Time.deltaTime;
			}

			if (GameData.GetPlayerCharacterId() == CharactersEnum.Truzi_BoT)
			{
				_positionRecordTimer -= Time.deltaTime;
				if (_positionRecordTimer <= 0)
				{
					_positionRecordTimer = 0.5f;
					if (_previousPositions.Count >= 10)
						_previousPositions.Dequeue();
				
					_previousPositions.Enqueue(_transform.position);
				}
			}
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				UseSkill(GameData.GetPlayerCharacterId());
			}
		}

		public void FixedUpdate()
		{
			if (_dashDuration > 0)
			{
				_dashPosition = _transform.position;

				_dashPosition = Utilities.GetPointOnColliderSurface(_dashPosition += transform.forward * (_dashDistance * Time.deltaTime), _transform, 0.5f);
				_transform.position = _dashPosition;
				_dashDuration -= Time.deltaTime;
			}

			if (_applyQueuedPosition)
			{
				_applyQueuedPosition = false;
				_transform.position = _previousPositions.Peek();
				SpawnManager.instance.SpawnObject(_transform.position, GameData.GetSkillPrefab().gameObject, _transform.rotation);
			}
		}

		private IEnumerator IFrames(float iframeDuration)
		{
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), true);
			yield return new WaitForSeconds(iframeDuration);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), false);
		}

		private void ApplySpecial()
		{
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Amelia)
				_ameliaGlassShield = Instantiate(GameData.GetSpecialPrefab(), abilityContainer).GetComponent<AmeliaGlassShield>();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Nishi)
				Instantiate(GameData.GetSpecialPrefab(), abilityContainer);
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW && GameData.GetPlayerCharacterRank() >= CharacterRank.E5)
				Instantiate(GameData.GetSpecialPrefab(), abilityContainer);
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Adam_OBoV && GameData.GetPlayerCharacterRank() >= CharacterRank.E5)
				Instantiate(GameData.GetSpecialPrefab(), abilityContainer);
		}

		private void UseSkill(CharactersEnum activeCharacterId)
		{
			if (_currentSkillCooldown > 0)
				return;

			_currentSkillCooldown = _skillCooldown * PlayerStatsScaler.GetScaler().GetSkillCooldownReductionPercentage();
			switch (activeCharacterId)
			{
				case CharactersEnum.Chitose:
					ChitoseSkill();
					break;
				case CharactersEnum.Maid:
					MaidSkill();
					break;
				case CharactersEnum.Amelia_BoD:
					AmeliaBoDSkill();
					break;
				case CharactersEnum.David_BoF:
					StartCoroutine(DavidSkill());
					break;
				case CharactersEnum.Arika_BoV:
					ArikaSkill();
					break;
				case CharactersEnum.Corina_BoB:
					StartCoroutine(CorinaSkill());
					break;
				case CharactersEnum.Amelia:
					AmeliaSkill();
					break;
				case CharactersEnum.Nishi:
					NishiSkill();
					break;
				case CharactersEnum.Natalie_BoW:
					NatalieSkill();
					break;
				case CharactersEnum.Summer:
					SummerSkill();
					break;
				case CharactersEnum.Adam_OBoV:
					AdamSkill();
					break;
				case CharactersEnum.Oana_BoI:
					OanaSkill();
					break;
				case CharactersEnum.Alice_BoL:
					StartCoroutine(AliceSkill());
					break;
				case CharactersEnum.Lucy_BoC:
					LucySkill();
					break;
				case CharactersEnum.Truzi_BoT:
					TruziSkill();
					break;
			}
		}

		private void TruziSkill()
		{
			if (_previousPositions.Count <= 0) return;

			_applyQueuedPosition = true;
			SpawnManager.instance.SpawnObject(_transform.position, GameData.GetSkillPrefab().gameObject, _transform.rotation);

			if (!GameData.IsCharacterRank(CharacterRank.E4)) return;
			
			var skillDuration = 5f;
			var cdrIncrease = 0.5f;
			playerStatsComponent.TemporaryStatBoost(StatEnum.CooldownReductionPercentage, cdrIncrease, skillDuration);
			abilityDurationBar.StartTick(skillDuration);
		}

		private void LucySkill()
		{
			var maxEnemies = GameData.IsCharacterRank(CharacterRank.E3) ? 20 : 10;
			
			var enemies = EnemyManager.instance.GetActiveEnemies()
				.Where(x => GameData.IsCharacterRank(CharacterRank.E1) || x.IsBoss())
				.OrderBy(_ => Random.value)
				.Take(Random.Range(5, maxEnemies));
			foreach (var enemy in enemies)
			{
				enemy.MarkAsPlayerControlled(GameData.IsCharacterRank(CharacterRank.E1) ? 25 : 10);
			}
		}

		private void AmeliaSkill()
		{
			_ameliaGlassShield.SpawnShards(6);
			if (GameData.GetPlayerCharacterRank() >= CharacterRank.E1)
				PickupManager.instance.SummonToPlayer();
		}

		private void OanaSkill()
		{
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			var skillDuration = obj.LifeTime;
			abilityDurationBar.StartTick(skillDuration);
		}

		private void NishiSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(_transform.position + _transform.forward * 1.5f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);

			if (GameData.GetPlayerCharacterRank() < CharacterRank.E2) return;
			
			for (var i = 0; i < 3; i++)
			{
				var randomPosition = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(_transform.position, 5f), gameObject.transform);
				SpawnManager.instance.SpawnObject(randomPosition, GameData.GetSkillPrefab().gameObject, _transform.rotation);
			}
		}

		private void AdamSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward * 2f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private IEnumerator AliceSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
			
			var rank = GameData.GetPlayerCharacterRank();
			var reductionMultiplier = (_weaponManager.maxWeaponCount - _weaponManager.GetUnlockedWeaponsAsInterface().Count) + 1;
			switch (rank)
			{
				case >= CharacterRank.E5:
					reductionMultiplier = 6;
					break;
				case >= CharacterRank.E3 when reductionMultiplier < 4:
					reductionMultiplier = 4;
					break;
			}
			
			var cooldownReduction = reductionMultiplier * 0.10f;
			var skillDuration = rank >= CharacterRank.E2 ? 10f : 5f;

			playerStatsComponent.IncreaseCooldownReductionPercentage(cooldownReduction);
			abilityDurationBar.StartTick(skillDuration);
			yield return new WaitForSeconds(skillDuration);
			playerStatsComponent.IncreaseCooldownReductionPercentage(-cooldownReduction);
		}

		private void NatalieSkill()
		{
			SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private void SummerSkill()
		{
			var arrow = SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject);
			var projectileComponent = arrow.GetComponent<SummerSkill>();
			projectileComponent.SetDirection(transform.forward, 0);

			if (GameData.IsCharacterRank(CharacterRank.E2))
			{
				arrow = SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject);
				projectileComponent = arrow.GetComponent<SummerSkill>();
				projectileComponent.SetDirection(transform.forward, 30);
				
				arrow = SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject);
				projectileComponent = arrow.GetComponent<SummerSkill>();
				projectileComponent.SetDirection(transform.forward, -30);
			}
		}

		private IEnumerator CorinaSkill()
		{
			if (!GameData.IsCharacterRank(CharacterRank.E5))
			{
				healthComponent.Damage(PlayerStatsScaler.GetScaler().GetHealth() * 0.9f);
				healthComponent.UpdateHealthBar();
			}

			var rank = GameData.GetPlayerCharacterRank();
			var attackCount = rank > CharacterRank.E4 ? 20 : 10;
			var enemies = EnemyManager.instance.GetActiveEnemies().OrderBy(_ => Random.value).Take(attackCount);

			foreach (var enemy in enemies)
			{
				var result = Utilities.GetPointOnColliderSurface(enemy.transform.position, transform);
				enemy.GetChaseComponent().SetImmobile(rank == CharacterRank.E5 ? 2f : 1.5f);
				SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject);
			}

			yield return null;
		}

		private void ChitoseSkill()
		{
			StartCoroutine(IFrames(GameData.IsCharacterRank(CharacterRank.E2) ? 1f : 0.5f));
			_dashDuration = 0.2f;
			_dashDistance = 10;
			
			if (GameData.IsCharacterRank(CharacterRank.E1))
				playerStatsComponent.TemporaryStatBoost(StatEnum.CritDamage, 2.5f, 3);
			if (GameData.IsCharacterRank(CharacterRank.E5))
				WeaponManager.instance.ReduceWeaponCooldowns(1);
		}

		private void MaidSkill()
		{
			var skillDuration = GameData.IsCharacterRank(CharacterRank.E1) ? 13f : 8f;
			var damageIncreasePercentage = GameData.IsCharacterRank(CharacterRank.E3) ? 2f : 0.5f;
			
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			obj.LifeTime = skillDuration;
			if (GameData.IsCharacterRank(CharacterRank.E2))
				playerStatsComponent.TemporaryStatBoost(StatEnum.DodgeChance, 0.5f, skillDuration);
			if (GameData.IsCharacterRank(CharacterRank.E5))
				playerStatsComponent.TemporaryStatBoost(StatEnum.CritRate, 1, skillDuration);
			playerStatsComponent.TemporaryStatBoost(StatEnum.DamagePercentageIncrease, damageIncreasePercentage, skillDuration);
			abilityDurationBar.StartTick(skillDuration);
		}
		
		private void AmeliaBoDSkill()
		{
			SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private IEnumerator DavidSkill()
		{
			var rank = GameData.GetPlayerCharacterRank();
			const float skillDuration = 10f;
			var hpPenalty = 0.1f;
			if (rank >= CharacterRank.E2)
				hpPenalty = 0.6f;
			if (rank >= CharacterRank.E5)
				hpPenalty = 0.4f;
			
			playerStatsComponent.SetInvincible(true);
			var targetHp = PlayerStatsScaler.GetScaler().GetMaxHealth() * (1 - hpPenalty);
			var hpDiff = targetHp - PlayerStatsScaler.GetScaler().GetHealth();
			GameManager.instance.playerComponent.TakeDamage(hpDiff, true, true);
			
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			obj.LifeTime = skillDuration;
			
			abilityDurationBar.StartTick(skillDuration);
			yield return new WaitForSeconds(skillDuration);
			playerStatsComponent.SetInvincible(false);
		}

		private void ArikaSkill()
		{
			var skill = FindFirstObjectByType<ArikaSkill>(FindObjectsInactive.Include);
			var skillDuration = GameData.IsCharacterRank(CharacterRank.E1) ? 25 : 15;
			skill.SetDuration(skillDuration);
			skill.gameObject.SetActive(true);
			
			abilityDurationBar.StartTick(skillDuration);
		}
	}
}