using System.Collections;
using DefaultNamespace.Data.Weapons;
using Managers;
using Objects.Abilities.Boomerang;
using Objects.Abilities.KeyOfDespair;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Claws
{
    public class ClawWeapon : PoolableWeapon<BoomerangProjectile>
    {
        public bool BeastsWrath;
        private NetworkProjectile _slashProjectile;

        public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
        {
            networkProjectile.Initialize(this, transform.position);
            networkProjectile.ParentToPlayer();
            _slashProjectile = networkProjectile;
        }

        protected override IEnumerator AttackProcess()
        {
            OnAttackStart();
            if (_slashProjectile == null)
            {
                RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Main);
                yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
            }
            var projectile = _slashProjectile.GetProjectile<ClawProjectile>();
            
            projectile.SetParentWeapon(this);
            _slashProjectile.transform.position = transform.position;
            projectile.SetRotation();
            projectile.gameObject.SetActive(true);
            projectile.SetNextStage();
            yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
            OnAttackEnd();
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 10)
                BeastsWrath = true;
        }
    }
}