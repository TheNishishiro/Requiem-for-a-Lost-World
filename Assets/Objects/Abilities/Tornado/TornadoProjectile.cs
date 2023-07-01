using System;
using System.Collections;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Lightning_Chain;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Tornado
{
	public class TornadoProjectile : ProjectileBase
	{
		[SerializeField] public GameObject lightningChainPrefab;
		public iTween.EaseType movementEaseType = iTween.EaseType.easeInOutSine;
		public float movementRadius = 4;
		private Vector3 _initialPosition;
		private TornadoWeapon TornadoWeapon => ParentWeapon as TornadoWeapon;

		private void Start()
		{
			_initialPosition = transform.position;
			StartCoroutine(Movement());
		}

		private void Update()
		{
			TickLifeTime();
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out _);
			if (TornadoWeapon.IsStaticDischarge && Random.value < 0.15f && Time.frameCount % 60 == 0)
				SpawnChainLightning(other);
		}

		private IEnumerator Movement()
		{
			if (_isDead)
				yield break;

			var newPosition = Utilities.GetRandomInAreaFreezeParameter(_initialPosition, movementRadius, isFreezeY: true);
			var distance = newPosition - _initialPosition;
			var time = distance.magnitude / WeaponStats.GetSpeed();
			
			iTween.MoveTo(gameObject, iTween.Hash(
				"position", newPosition,
				"time", time,
				"easetype", movementEaseType
			));
			yield return new WaitForSeconds(time + 0.1f);
			StartCoroutine(Movement());
		}
		
		private void SpawnChainLightning(Component other)
		{
			var chainLighting = SpawnManager.instance.SpawnObject(other.gameObject.transform.position, lightningChainPrefab);
			var lightingChainProjectile = chainLighting.GetComponent<LightningChainProjectile>();
			lightingChainProjectile.SetParentWeapon(ParentWeapon);
			lightingChainProjectile.SetStats(new WeaponStats()
			{
				TimeToLive = 0.3f,
				Damage = WeaponStats.GetDamage() * 2.5f,
				Scale = 1f,
				DetectionRange = 2f
			});
			lightingChainProjectile.SeekTargets(2);
		}
	}
}