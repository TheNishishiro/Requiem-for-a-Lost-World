using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Lightning_Chain
{
	public class LightningChainProjectile : ProjectileBase
	{
		[SerializeField] public LineRenderer lineRenderer;

		private void Awake()
		{
			lineRenderer.positionCount = 0;
		}

		public void Update()
		{
			TickLifeTime();
		}
		
		public void SeekTargets(int maxTargers)
		{
			StartCoroutine(FindChainLightningTarget(maxTargers));
		}

		public IEnumerator FindChainLightningTarget(int maxTargets)
		{
			var lastPosition = transform.position;
			var targets = EnemyManager.instance.GetActiveEnemies().Select(x => x.GetDamagableComponent()).ToArray();
			
			for (var i = 0; i < maxTargets; i++)
			{
				var target = targets[Random.Range(0, targets.Length)];
				if (target == null)
					continue;
				
				AddTarget(lastPosition);
				target.TakeDamage(WeaponStats.GetDamage(), ParentWeapon);
				lastPosition = target.transform.position;
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		public void AddTarget(Vector3 targetPosition)
		{
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(lineRenderer.positionCount - 1, targetPosition);
		}
	}
}