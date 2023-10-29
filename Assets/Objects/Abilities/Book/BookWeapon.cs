using System.Collections;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Magic_Ball;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Book
{
	public class BookWeapon : PoolableWeapon<BookProjectile>
	{
		private float rotateOffset = 0;
		[HideInInspector] public bool IsShadowBurst;
		[SerializeField] public GameObject ExplosionPrefab;

		protected override BookProjectile ProjectileInit()
		{
			var book = Instantiate(spawnPrefab, transform).GetComponent<BookProjectile>();
			book.SetParentWeapon(this);
			book.SetTarget(gameObject);
			return book;
		}

		protected override bool ProjectileSpawn(BookProjectile projectile)
		{
			projectile.transform.position = transform.position + new Vector3(weaponStats.GetScale(),0,0);
			projectile.transform.RotateAround(transform.position, Vector3.up, rotateOffset);
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override IEnumerator AttackProcess()
		{
			var rotationStep = GetRotationByAttackCount();
			rotateOffset = 0;
			
			for (var i = 0; i < weaponStats.GetAttackCount(); i++)
			{
				Attack();
				rotateOffset += rotationStep;
			}

			yield break;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsShadowBurst = true;
		}
	}
}