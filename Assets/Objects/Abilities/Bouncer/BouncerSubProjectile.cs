using System.Collections;
using System.Linq;
using Data.Elements;
using DefaultNamespace;
using Managers;
using Objects.Abilities.SpaceExpansionBall;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Bouncer
{
	public class BouncerSubProjectile : PoolableProjectile<BouncerSubProjectile>
	{
		private Vector3 direction;
		private BouncerWeapon BouncerWeapon => (BouncerWeapon) ParentWeapon;
		
		public void SetTarget(Damageable target)
		{
			if (target == null)
				OnLifeTimeEnd();
				
			direction = (target.targetPoint.transform.position - transformCache.position).normalized;
		}

		protected override void CustomUpdate()
		{
			transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, true, false, out var damageable);
			FindNextTarget();

			if (damageable == null)
				return;
			
			if (BouncerWeapon.ElectroDefenceShred > 0)
				damageable.ReduceElementalDefence(Element.Lightning, BouncerWeapon.ElectroDefenceShred);
		}

		public void FindNextTarget()
		{
			var target = EnemyManager.instance.GetRandomEnemy().GetDamagableComponent();
			SetTarget(target);
		}
		
		protected override void Destroy()
		{
			ReturnToPool(_objectPool, _object);
		}
	}
}