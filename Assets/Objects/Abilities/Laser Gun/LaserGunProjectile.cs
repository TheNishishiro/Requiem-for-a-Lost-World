using System;
using DefaultNamespace;
using Interfaces;
using JetBrains.Annotations;
using Objects.Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Laser_Gun
{
	public class LaserGunProjectile : PoolableProjectile<LaserGunProjectile>
	{
		[SerializeField] public LineRenderer lineRenderer;
		[SerializeField] public LaserGunNetworkComponent gunNetworkComponent;
		private Enemy _laserTarget;
		private Transform _targetTransform;
		
		public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			base.SetStats(weaponStatsStrategy);
			SetTarget();
			_laserTarget = null;
		}

		protected override void CustomUpdate()
		{
			if (!isDamageCooldownExpired) return;
			ResetDamageCooldown();

			if (_laserTarget != null && !_laserTarget.IsDying() && _laserTarget.gameObject.activeSelf && lineRenderer.positionCount != 0 && Vector3.Distance(_targetTransform.position, transformCache.position) < WeaponStatsStrategy.GetDetectionRange())
			{
				var position = _targetTransform.position;
				lineRenderer.SetPosition(1, position);
				transformCache.LookAt(_targetTransform);
				SimpleDamage(_laserTarget.GetDamagableComponent(), false, false);
			}
			else
			{
				SetTarget();
			}
		}

		private void SetTarget()
		{
			_laserTarget = Utilities.FindClosestEnemy(transformCache.position, EnemyManager.instance.GetActiveEnemies(), out var distanceToClosest);
			if (_laserTarget == null || distanceToClosest > WeaponStatsStrategy.GetDetectionRange())
			{
				lineRenderer.positionCount = 0;
				return;
			}
			lineRenderer.positionCount = 2;

			_targetTransform = _laserTarget.TargetPoint;
			gunNetworkComponent.SetTarget(_targetTransform.position);
			
			transformCache.LookAt(_targetTransform);
		}
	}
}