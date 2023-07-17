using System;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldProjectile : ProjectileBase
	{
        [SerializeField] private GameObject healingFieldPrefab;
        [SerializeField] private float healingFrequency;
        private float _currentHealingFrequency;

        public override void SetStats(WeaponStats weaponStats)
        {
	        base.SetStats(weaponStats);
	        _currentHealingFrequency = healingFrequency;
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
	        healingField.GetComponent<HealingField>().SetHealAmount(WeaponStats.GetDamage());
		}
	}
}