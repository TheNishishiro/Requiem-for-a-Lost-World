using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Corruption_Aura
{
	public class CorruptionAuraWeapon : WeaponBase
	{
		public override void Update()
		{
			transform.localScale = new Vector3(weaponStats.GetScale(), 0.01f, weaponStats.GetScale());
			transform.rotation = Quaternion.Euler(0,0,0);
		}

		public override void Attack()
		{
		}

		public void OnTriggerStay(Collider other)
		{
			var damageable = other.GetComponent<IDamageable>();
			if (damageable != null)
				damageable.TakeDamageWithCooldown(weaponStats.GetDamage(), gameObject, weaponStats.DamageCooldown, this);
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithLucy);
		}
	}
}