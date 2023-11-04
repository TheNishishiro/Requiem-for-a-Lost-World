using System;
using DefaultNamespace;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Laser_Gun
{
	public class LaserGunProjectile : PoolableProjectile<LaserGunProjectile>
	{
		[SerializeField] public LineRenderer lineRenderer;
		[SerializeField] public Transform laserFirePoint;
		private Damageable _laserTarget;
		private Transform _transform;
		private Transform _targetTransform;

		protected override void Awake()
		{
			base.Awake();
			_laserTarget = null;
			_transform = transform;
		}

		public override void SetStats(WeaponStats weaponStats)
		{
			base.SetStats(weaponStats);
			SetTarget();
		}

		void Update()
		{
			TickProjectile();
			
			if (!isDamageCooldownExpired) return;
			ResetDamageCooldown();

			if (_laserTarget != null && _laserTarget.gameObject.activeSelf && lineRenderer.positionCount != 0 && Vector3.Distance(_targetTransform.position, _transform.position) < WeaponStats.GetDetectionRange())
			{
				Debug.Log("dealing damage");
				lineRenderer.SetPosition(1, _targetTransform.position);
				_transform.LookAt(_targetTransform);
				SimpleDamage(_laserTarget, false);
			}
			else
			{
				SetTarget();
			}
		}

		private void SetTarget()
		{
			var closestTarget = Utilities.FindClosestEnemy(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
			if (closestTarget == null || distanceToClosest > WeaponStats.GetDetectionRange())
			{
				lineRenderer.positionCount = 0;
				return;
			}
			lineRenderer.positionCount = 2;

			_laserTarget = closestTarget.GetDamagableComponent();
			_targetTransform = closestTarget.TargetPoint;
			lineRenderer.SetPosition(0, laserFirePoint.position);
			lineRenderer.SetPosition(1, _targetTransform.position);
			_transform.LookAt(_targetTransform);
		}
	}
}