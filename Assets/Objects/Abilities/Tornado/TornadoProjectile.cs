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
	public class TornadoProjectile : PoolableProjectile<TornadoProjectile>
	{
		private Rigidbody _rb;
		private TornadoWeapon TornadoWeapon => ParentWeapon as TornadoWeapon;
		private float _dischargeCooldown;

		protected override void Awake()
		{
			base.Awake();
			_rb = GetComponent<Rigidbody>();
		}

		public override void SetStats(WeaponStats weaponStats)
		{
			base.SetStats(weaponStats);
			StartCoroutine(Movement());
		}

		private void Update()
		{
			TickLifeTime();
			if (_dischargeCooldown > 0)
				_dischargeCooldown -= Time.deltaTime;
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out _);
			if (TornadoWeapon.IsStaticDischarge && Random.value < 0.15f && _dischargeCooldown <= 0)
				SpawnChainLightning(other);
		}

		private IEnumerator Movement()
		{
			var speed = WeaponStats.GetSpeed() * (GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 1.5f : 1f);

			while (!IsDead)
			{
				var enemy = EnemyManager.instance.GetRandomEnemy();
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
			TornadoWeapon.SpawnSubProjectile(other.gameObject.transform.position);
			_dischargeCooldown = 0.5f;
		}
	}
}