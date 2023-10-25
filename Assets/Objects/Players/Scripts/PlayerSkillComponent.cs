using System;
using System.Collections;
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
		[SerializeField] private ChronastaSkill chronastaSkill;
		[SerializeField] private SpecialBar specialBar;
		[SerializeField] private AbilityDurationBar abilityDurationBar;
		private AmeliaGlassShield _ameliaGlassShield;
		private float _currentSkillCooldown = 0f;
		private float _skillCooldown = 5f;

		public void Start()
		{
			_skillCooldown = GameData.GetCharacterSkillCooldown();
			ApplySpecial();
		}

		public void Update()
		{
			if (_currentSkillCooldown > 0f)
			{
				skillCooldownImage.fillAmount = _currentSkillCooldown / _skillCooldown;
				_currentSkillCooldown -= Time.deltaTime;
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				UseSkill(GameData.GetPlayerCharacterId());
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

			_currentSkillCooldown = _skillCooldown * playerStatsComponent.GetSkillCooldownReductionPercentage();
			switch (activeCharacterId)
			{
				case CharactersEnum.Chitose:
					ChitoseSkill();
					break;
				case CharactersEnum.Maid:
					StartCoroutine(MaidSkill());
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
					AliceSkill();
					break;
			}
		}

		private void AmeliaSkill()
		{
			_ameliaGlassShield.SpawnShards(6);
		}

		private void OanaSkill()
		{
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			var skillDuration = obj.LifeTime;
			abilityDurationBar.StartTick(skillDuration);
		}

		private void NishiSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward * 1.5f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private void AdamSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward * 2f, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private void AliceSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private void NatalieSkill()
		{
			SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private void SummerSkill()
		{
			var throwingKnife = SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject);
			var projectileComponent = throwingKnife.GetComponent<SummerSkill>();

			projectileComponent.SetDirection(transform.forward);
		}

		private IEnumerator CorinaSkill()
		{
			healthComponent.Damage(playerStatsComponent.GetHealth() * 0.9f);
			healthComponent.UpdateHealthBar();
			var rank = GameData.GetPlayerCharacterRank();
			var attackCount = rank > CharacterRank.E4 ? 20 : 10;
			var enemies = FindObjectsOfType<Enemy>().OrderBy(x => Random.value).Take(attackCount);

			foreach (var enemy in enemies)
			{
				var result = Utilities.GetPointOnColliderSurface(enemy.transform.position, transform);
				enemy.GetComponent<ChaseComponent>().SetImmobile(rank == CharacterRank.E5 ? 2f : 1.5f);
				SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject);
			}

			yield return null;
		}

		private void ChitoseSkill()
		{
			var forward = transform.forward;
			forward.y = 0f;
			StartCoroutine(IFrames(0.5f));
			controller.Move(forward * 0.2f);
		}

		private IEnumerator MaidSkill()
		{
			var rank = GameData.GetPlayerCharacterRank();
			var damageIncreasePercentage = rank >= CharacterRank.E3 ? 0.75f : 0.5f;
			var skillDuration = rank >= CharacterRank.E5 ? 12f : 8f;
			
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			obj.LifeTime = skillDuration;
			playerStatsComponent.IncreaseDamageIncreasePercentage(damageIncreasePercentage);
			abilityDurationBar.StartTick(skillDuration);
			yield return new WaitForSeconds(skillDuration);
			playerStatsComponent.IncreaseDamageIncreasePercentage(-damageIncreasePercentage);
		}
		
		private void AmeliaBoDSkill()
		{
			SpawnManager.instance.SpawnObject(transform.position, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}

		private IEnumerator DavidSkill()
		{
			var rank = GameData.GetPlayerCharacterRank();
			const float skillDuration = 10f;
			var hpPenalty = 40;
			if (rank >= CharacterRank.E3)
				hpPenalty = 80;
			if (rank >= CharacterRank.E5)
				hpPenalty = 120;
			
			playerStatsComponent.SetInvincible(true);
			playerStatsComponent.SetHealth(Math.Max(1, playerStatsComponent.GetHealth() - hpPenalty));
			healthComponent.UpdateHealthBar();
			
			var obj = Instantiate(GameData.GetSkillPrefab(), abilityContainer);
			obj.LifeTime = skillDuration;

			var damageFactor = 150f;
			if (rank >= CharacterRank.E4)
				damageFactor = 105f;
			
			var damageIncrease = Math.Abs(playerStatsComponent.GetHealth() - playerStatsComponent.GetMaxHealth()) / damageFactor;
			playerStatsComponent.IncreaseDamageIncreasePercentage(damageIncrease);
			abilityDurationBar.StartTick(skillDuration);
			yield return new WaitForSeconds(skillDuration);
			playerStatsComponent.IncreaseDamageIncreasePercentage(-damageIncrease);
			playerStatsComponent.SetInvincible(false);
		}

		private void ArikaSkill()
		{
			var result = Utilities.GetPointOnColliderSurface(transform.position + transform.forward * 3, gameObject.transform);
			SpawnManager.instance.SpawnObject(result, GameData.GetSkillPrefab().gameObject, transform.rotation);
		}
	}
}