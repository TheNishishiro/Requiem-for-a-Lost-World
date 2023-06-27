using UnityEngine;
using Weapons;

namespace Interfaces
{
	public interface IDamageable
	{
		void TakeDamage(int damage, WeaponBase damageSource);
		void SetHealth(int health);
		bool IsDestroyed();
		void TakeDamageWithCooldown(int damage, GameObject damageSource, float damageCooldown, WeaponBase weaponBase);
		void SetVulnerable(float time, float percentage);
	}
}