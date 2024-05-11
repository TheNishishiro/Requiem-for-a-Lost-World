using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Settings;
using Interfaces;
using Managers;
using Objects.Abilities;
using Objects.Abilities.Laser_Gun;
using Objects.Characters;
using Objects.Characters.Chronastra.Skill;
using Objects.Characters.Nishi.Skill;
using Objects.Enemies;
using Objects.Players.Containers;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UI.In_Game.GUI.Scripts.Managers;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Objects.Players.Scripts
{
	public class PlayerSkillComponent : NetworkBehaviour
	{
		public static PlayerSkillComponent instance;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		[SerializeField] private HealthComponent healthComponent;
		[SerializeField] private SpecialBar specialBar;
		[SerializeField] private AbilityDurationBar abilityDurationBar;
		[SerializeField] private WeaponManager _weaponManager;
		private Transform _abilityContainer;
		private AmeliaGlassShield _ameliaGlassShield;
		private float _currentSkillCooldown = 0f;
		private float _currentDashCooldown = 0f;
		private float _skillCooldown = 5f;
		private float _dashCooldown = 10f;
		private float _dashDuration = 0;
		private float _dashDistance = 10;
		private int _dashStacks = 2;

		private int DashStacks
		{
			get => _dashStacks;
			set
			{
				_dashStacks = value;
				GuiManager.instance.UpdateDashes(_dashStacks);
			}
		}
		private Transform _transform;
		private Vector3 _dashPosition;
		private Queue<Vector3> _previousPositions = new ();
		private float _positionRecordTimer;
		private bool _applyQueuedPosition;

		
		public void Start()
		{
			if (instance == null)
				instance = this;
            
			_skillCooldown = GameData.GetCharacterSkillCooldown();
		}

		public void Init(Transform abilityContainerTransform)
		{
			_abilityContainer = abilityContainerTransform;
			DashStacks = PlayerStatsScaler.GetScaler().GetDashCount();
			ApplySpecial();
		}
		
		public void Update()
		{
			_transform = GameManager.instance.PlayerTransform;
			if (_transform == null) return;
			
			if (_currentSkillCooldown > 0f)
			{
				GuiManager.instance.UpdateAbilityCooldown(_currentSkillCooldown, _skillCooldown);
				_currentSkillCooldown -= Time.deltaTime;
			}
			
			if (_dashStacks < PlayerStatsScaler.GetScaler().GetDashCount())
			{
				_currentDashCooldown -= Time.deltaTime;
				if (_currentDashCooldown <= 0f)
                {
	                DashStacks++;
                    _currentDashCooldown = _dashCooldown;
                }
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
			
			if (Input.GetKeyDown(SaveFile.Instance.GetKeybinding(KeyAction.Ability)))
			{
				UseSkill();
			}
			
			if (Input.GetKeyDown(SaveFile.Instance.GetKeybinding(KeyAction.Dash)))
			{
				UseDash();
			}
		}

		public void UseSkill()
		{
			UseSkill(GameData.GetPlayerCharacterId());
		}

		public void UseDash()
		{
			if (_dashStacks <= 0)
				return;
			DashStacks--;
			_currentDashCooldown = _dashCooldown;
			_dashDuration = 0.2f;
			_dashDistance = 10;
			StartCoroutine(IFrames(0.2f));
		}

		public void FixedUpdate()
		{
			if (_dashDuration > 0)
			{
				_dashPosition = _transform.position;

				_dashPosition = Utilities.GetPointOnColliderSurface(_dashPosition += _transform.forward * (_dashDistance * Time.deltaTime), _transform, 0.5f);
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
				_ameliaGlassShield = Instantiate(GameData.GetSpecialPrefab(), _abilityContainer).GetComponent<AmeliaGlassShield>();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Nishi)
				Instantiate(GameData.GetSpecialPrefab(), _abilityContainer);
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Nishi_HoF)
				Instantiate(GameData.GetSpecialPrefab(), _abilityContainer);
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW && GameData.GetPlayerCharacterRank() >= CharacterRank.E3)
				Instantiate(GameData.GetSpecialPrefab(), _abilityContainer);
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Adam_OBoV && GameData.GetPlayerCharacterRank() >= CharacterRank.E5)
				Instantiate(GameData.GetSpecialPrefab(), _abilityContainer);
		}

		private void UseSkill(CharactersEnum activeCharacterId)
		{
			if (_currentSkillCooldown > 0)
				return;

			if (playerStatsComponent.IsDead())
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
					StartCoroutine(NishiSkill());
					break;
				case CharactersEnum.Nishi_HoF:
					NishiHoFSkill();
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
			RpcManager.instance.LucySkillRpc(GameData.IsCharacterRank(CharacterRank.E3), GameData.IsCharacterRank(CharacterRank.E1));
		}

		private void AmeliaSkill()
		{
			_ameliaGlassShield.SpawnShards(6);
			if (GameData.GetPlayerCharacterRank() >= CharacterRank.E1)
				PickupManager.instance.SummonToPlayer();
		}

		private void OanaSkill()
		{
			var obj = Instantiate(GameData.GetSkillPrefab(), _abilityContainer);
			var skillDuration = obj.LifeTime;
			abilityDurationBar.StartTick(skillDuration);
		}

		private IEnumerator NishiSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(_transform.position + _transform.forward * 1.5f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, _transform.rotation);

			if (GameData.GetPlayerCharacterRank() < CharacterRank.E2) yield break;
			
			for (var i = 0; i < 8; i++)
			{
				yield return new WaitForSeconds(0.3f);
				var randomPosition = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(_transform.position, 3.5f), gameObject.transform);
				SpawnManager.instance.SpawnObject(randomPosition, GameData.GetSkillPrefab().gameObject, _transform.rotation);
			}
		}

		private void NishiHoFSkill()
		{
			if (GameData.IsCharacterRank(CharacterRank.E3))
				SpecialBarManager.instance.Increment(50);
			
			var result = Utilities.GetPointOnColliderSurface(_transform.position + _transform.forward * 10f, gameObject.transform, 1.5f);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, _transform.rotation);
		}

		private void AdamSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(_transform.position + _transform.forward * 2f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, _transform.rotation);
		}

		private IEnumerator AliceSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(_transform.position + _transform.forward, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, _transform.rotation);
			
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
			SpawnManager.instance.SpawnObject(_transform.position, GameData.GetSkillPrefab().gameObject, _transform.rotation);
		}

		private void SummerSkill()
		{
			var arrow = SpawnManager.instance.SpawnObject(_transform.position, GameData.GetSkillPrefab().gameObject);
			var projectileComponent = arrow.GetComponent<SummerSkill>();
			abilityDurationBar.StartTick(10f);
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
				var result = Utilities.GetPointOnColliderSurface(enemy.transform.position, _transform);
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
				playerStatsComponent.TemporaryStatBoost(StatEnum.CritDamage, 2.5f, 6);
			if (GameData.IsCharacterRank(CharacterRank.E5))
				WeaponManager.instance.ReduceWeaponCooldowns(1);
		}

		private void MaidSkill()
		{
			var skillDuration = GameData.IsCharacterRank(CharacterRank.E1) ? 13f : 8f;
			var damageIncreasePercentage = GameData.IsCharacterRank(CharacterRank.E3) ? 2f : 0.5f;
			
			var obj = Instantiate(GameData.GetSkillPrefab(), _abilityContainer);
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
			SpawnManager.instance.SpawnObject(_transform.position, GameData.GetSkillPrefab().gameObject, _transform.rotation);
		}

		private IEnumerator DavidSkill()
		{
			var rank = GameData.GetPlayerCharacterRank();
			const float skillDuration = 10f;
			var hpPenalty = 0.1f;
			if (rank >= CharacterRank.E2)
				hpPenalty = 0.4f;
			if (rank >= CharacterRank.E5)
				hpPenalty = 0.6f;
			
			playerStatsComponent.SetInvincible(true);
			var targetHp = PlayerStatsScaler.GetScaler().GetMaxHealth() * (1 - hpPenalty);
			var hpDiff = PlayerStatsScaler.GetScaler().GetHealth() - targetHp;
			GameManager.instance.playerComponent.TakeDamage(hpDiff, true, true);
			
			var obj = Instantiate(GameData.GetSkillPrefab(), _abilityContainer);
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