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

		protected override void Awake()
		{
			base.Awake();
			lineRenderer.positionCount = 2;
			laserTarget = null;
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

			if (laserTarget != null)
			{
				lineRenderer.SetPosition(1, laserTarget.transform.position);
				transform.LookAt(laserTarget.transform);
				SimpleDamage(laserTarget, false);
			}
			else
			{
				SetTarget();
			}
		}

		private void SetTarget()
		{
			var closestTarget = Utilities.FindClosestDamageable(transform.position, FindObjectsByType<Damageable>(FindObjectsSortMode.None), out var distanceToClosest);
			if (closestTarget == null || distanceToClosest > WeaponStats.GetDetectionRange())
				return;

			laserTarget = closestTarget;
			lineRenderer.SetPosition(0, laserFirePoint.position);
			lineRenderer.SetPosition(1, laserTarget.transform.position);
			transform.LookAt(laserTarget.transform);
		}
	}
}