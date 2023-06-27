using System;
using DefaultNamespace;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Laser_Gun
{
	public class LaserGunProjectile : ProjectileBase
	{
		[SerializeField] public LineRenderer lineRenderer;
		[SerializeField] public Transform laserFirePoint;
		[SerializeField] public Transform modelTransform;
		private Damageable laserTarget;
		private float damageCooldown;

		private void Awake()
		{
			lineRenderer.positionCount = 2;
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
				var closestTarget = Utilities.FindClosestDamageable(transform.position, FindObjectsOfType<Damageable>(), out var distanceToClosest);
				if (closestTarget == null || distanceToClosest > WeaponStats.GetDetectionRange())
					return;

				SetTarget(closestTarget);
			}
		}

		private void SetTarget(Damageable target)
		{
			if (target is null)
				return;

			laserTarget = target;
			lineRenderer.SetPosition(0, laserFirePoint.position);
			lineRenderer.SetPosition(1, laserTarget.transform.position);
			transform.LookAt(laserTarget.transform);
		}
	}
}