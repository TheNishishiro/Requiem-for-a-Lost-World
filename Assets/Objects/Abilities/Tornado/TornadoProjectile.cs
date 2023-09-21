using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using Managers;
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
	public class TornadoProjectile : ProjectileBase
	{
		[SerializeField] public GameObject lightningChainPrefab;
		private Rigidbody _rb;
		private TornadoWeapon TornadoWeapon => ParentWeapon as TornadoWeapon;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			StartCoroutine(Movement());
		}

		private void Update()
		{
			TickLifeTime();
		}

		private void OnCollisionStay(Collision other)
		{
			DamageArea(other.collider, out _);
			if (TornadoWeapon.IsStaticDischarge && Random.value < 0.15f && Time.frameCount % 60 == 0)
				SpawnChainLightning(other.collider);
		}

		private IEnumerator Movement()
		{
			var speed = WeaponStats.GetSpeed() * (GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 1.5f : 1f);

			while (!_isDead)
			{
				var enemy = FindObjectsOfType<Enemy>().OrderBy(_ => Random.value).FirstOrDefault();
				if (enemy == null)
					yield break;
				
				var targetPosition = enemy.transform.position;
        
				while(Vector3.Distance(_rb.position, targetPosition) > 0.3f)
				{
					var newPosition = Vector3.MoveTowards(_rb.position, targetPosition, speed * Time.deltaTime);
					_rb.MovePosition(new Vector3(newPosition.x, _rb.position.y, newPosition.z));
					yield return null;
				}
			}
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