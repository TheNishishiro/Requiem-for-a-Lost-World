using System;
using Interfaces;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldProjectile : PoolableProjectile<HealingFieldProjectile>
	{
        [SerializeField] private float healingFrequency;
        private HealingFieldWeapon HealingFieldWeapon => (HealingFieldWeapon)ParentWeapon;
        private float _currentHealingFrequency;

        public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
        {
	        base.SetStats(weaponStatsStrategy);
	        _currentHealingFrequency = 0;
	        transformCache = transform;
        }

        protected override void CustomUpdate()
		{
			_currentHealingFrequency -= Time.deltaTime;
			if (_currentHealingFrequency <= 0)
			{
				_currentHealingFrequency = healingFrequency;
				SpawnHealingField();
			}
		}
        
        private void SpawnHealingField()
		{
			HealingFieldWeapon.SpawnSubProjectile(transformCache.position);
		}
	}
}