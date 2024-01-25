using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Abilities.Lightning_Chain;
using Objects.Characters;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Tornado
{
	public class TornadoProjectile : PoolableProjectile<TornadoProjectile>
	{
		[SerializeField] private Rigidbody rb;
		[SerializeField] private MeshRenderer coreRenderer;
		[SerializeField] private float coreMaxDissolve;
		[SerializeField] private MeshRenderer windOuterRenderer;
		[SerializeField] private float windOuterMaxDissolve;
		[SerializeField] private MeshRenderer shadowRenderer;
		[SerializeField] private float shadowMaxDissolve;
		[SerializeField] private MeshRenderer windInnerRenderer;
		[SerializeField] private float windInnerMaxDissolve;
		[SerializeField] private ParticleSystem particles;
		private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
		private TornadoWeapon TornadoWeapon => ParentWeapon as TornadoWeapon;
		private float _dischargeCooldown;

		public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			base.SetStats(weaponStatsStrategy);
			StartCoroutine(Movement());
		}

		protected override void CustomUpdate()
		{
			if (_dischargeCooldown > 0)
				_dischargeCooldown -= Time.deltaTime;
			
			if (TimeAlive < 1f)
			{
				SetDissolveValues(true);
			}
			else if (CurrentTimeToLive < 1f)
			{
				if (particles.isEmitting)
					particles.Stop(true);
				SetDissolveValues(false);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out var damageable);
			if (other.CompareTag("Enemy") && GameData.IsCharacterWithRank(CharactersEnum.Natalie_BoW, CharacterRank.E1) && Random.value < 0.7f)
				DamageOverTime(damageable, other);
			if (other.CompareTag("Enemy") && GameData.IsCharacterWithRank(CharactersEnum.Natalie_BoW, CharacterRank.E2))
				other.GetComponent<ChaseComponent>()?.SetSlow(0.2f, 1);
			if (TornadoWeapon.IsStaticDischarge && Random.value < 0.15f && _dischargeCooldown <= 0)
				SpawnChainLightning(other);
		}

		private IEnumerator Movement()
		{
			var speed = WeaponStatsStrategy.GetSpeed() * (GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 1.5f : 1f);

			while (!IsDead)
			{
				var enemy = EnemyManager.instance.GetRandomEnemy();
				if (enemy == null)
					yield break;
				
				var targetPosition = enemy.transform.position;
        
				while(Vector3.Distance(rb.position, targetPosition) > 0.3f)
				{
					var newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
					rb.MovePosition(new Vector3(newPosition.x, rb.position.y, newPosition.z));
					yield return null;
				}
			}
		}
		
		private void SpawnChainLightning(Component other)
		{
			TornadoWeapon.SpawnSubProjectile(other.gameObject.transform.position);
			_dischargeCooldown = 0.5f;
		}
		

		private void SetDissolveValues(bool isAnticipation)
		{
			SetRendererDissolve(coreRenderer, coreMaxDissolve, GetDissolveValue(coreMaxDissolve, isAnticipation));
			SetRendererDissolve(windOuterRenderer, windOuterMaxDissolve, GetDissolveValue(windOuterMaxDissolve, isAnticipation));
			SetRendererDissolve(shadowRenderer, shadowMaxDissolve, GetDissolveValue(shadowMaxDissolve, isAnticipation));
			SetRendererDissolve(windInnerRenderer, windInnerMaxDissolve, GetDissolveValue(windInnerMaxDissolve, isAnticipation));
		}

		private float GetDissolveValue(float max, bool isAnticipation)
		{
			return isAnticipation ? Mathf.Lerp(0f, max, TimeAlive) : Mathf.Lerp(0f, max, CurrentTimeToLive);
		}

		private void SetRendererDissolve(MeshRenderer renderer, float maxDissolve, float dissolveValue)
		{
			var material = renderer.materials[0];
			var clampedDissolve = Mathf.Clamp(dissolveValue, 0f, maxDissolve);
			material.SetFloat(Dissolve, clampedDissolve);
		}
	}
}