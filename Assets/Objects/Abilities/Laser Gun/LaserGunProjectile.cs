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
		private Damageable laserTarget;
		private Transform _transform;
		private Transform _targetTransform;

		protected override void Awake()
		{
			base.Awake();
			laserTarget = null;
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

			if (laserTarget != null && laserTarget.isActiveAndEnabled && lineRenderer.positionCount != 0 && Vector3.Distance(_targetTransform.position, _transform.position) < WeaponStats.GetDetectionRange())
			{
				lineRenderer.SetPosition(1, _targetTransform.position);
				_transform.LookAt(_targetTransform);
				SimpleDamage(laserTarget, false);
			}
			else
			{
				SetTarget();
			}
		}

		private void SetTarget()
		{
			var closestTarget = Utilities.FindClosestDamageable(transform.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
			if (closestTarget == null || distanceToClosest > WeaponStats.GetDetectionRange())
			{
				lineRenderer.positionCount = 0;
				return;
			}
			lineRenderer.positionCount = 2;
			
			laserTarget = closestTarget;
			_targetTransform = laserTarget.transform;
			lineRenderer.SetPosition(0, laserFirePoint.position);
			lineRenderer.SetPosition(1, _targetTransform.position);
			_transform.LookAt(_targetTransform);
		}
	}
}