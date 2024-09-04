using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Objects.Abilities;
using Objects.Abilities.Magic_Ball;
using UnityEngine;

namespace Objects.Enemies.EnemyWeapons.Weapon_Prefabs
{
    public class EnemyFireballWeapon : EnemyWeapon
    {
        public EnemyFireballWeapon(GameObject spawnPrefab, WeaponStats weaponStats, EnemyWeaponId weaponId) : base(spawnPrefab, weaponStats, weaponId)
        {
        }

        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = GetTarget();
            networkProjectile.Initialize(this, transformCache.position);
            networkProjectile.GetProjectile<EnemyFireballProjectile>().SetDirection(position.x, position.y, position.z);
            networkProjectile.projectile.gameObject.SetActive(true);
        }
    }
}