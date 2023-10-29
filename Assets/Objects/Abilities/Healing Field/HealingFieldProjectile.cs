using System;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldProjectile : PoolableProjectile<HealingFieldProjectile>
	{
        [SerializeField] private GameObject healingFieldPrefab;
        [SerializeField] private float healingFrequency;
        private HealingFieldWeapon HealingFieldWeapon => (HealingFieldWeapon)ParentWeapon;
        private float _currentHealingFrequency;

        public override void SetStats(WeaponStats weaponStats)
        {
	        base.SetStats(weaponStats);
	        _currentHealingFrequency = 0;
        }

        private void Update()
		{
			TickLifeTime();
			_currentHealingFrequency -= Time.deltaTime;
			if (_currentHealingFrequency <= 0)
			{
				_currentHealingFrequency = healingFrequency;
				SpawnHealingField();
			}
		}
        
        private void SpawnHealingField()
		{
	        var healingField = SpawnManager.instance.SpawnObject(transform.position, healingFieldPrefab);
	        healingField.transform.localScale *= WeaponStats.GetScale();
	        healingField.GetComponent<HealingField>().Setup(WeaponStats.HealPerHit, HealingFieldWeapon.IsEmpowering);
		}
	}
}