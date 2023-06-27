using System.Collections;
using DefaultNamespace;
using Managers;
using Objects.Abilities.Magic_Ball;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Book
{
	public class BookWeapon : WeaponBase
	{
		private float rotateOffset = 0;
		[HideInInspector] public bool IsShadowBurst;
		[SerializeField] public GameObject ExplosionPrefab;
		
		public override void Attack()
		{
			var book = Instantiate(spawnPrefab, transform);
			book.transform.position += new Vector3(weaponStats.GetScale(),0,0);
			book.transform.RotateAround(transform.position, Vector3.up, rotateOffset);
			var projectileComponent = book.GetComponent<BookProjectile>();
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetTarget(gameObject);
			projectileComponent.SetStats(weaponStats);
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