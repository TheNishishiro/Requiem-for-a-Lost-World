using System.Collections;
using DefaultNamespace.Data.Weapons;
using Managers;
using Objects.Abilities.Boomerang;
using Objects.Characters;
using Objects.Stage;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.KeyOfDespair
{
    public class KeyOfDespairWeapon : PoolableWeapon<BoomerangProjectile>
    {
        [SerializeField] private GameObject slashProjectilePrefab;
        private NetworkProjectile _slashProjectile;

        public override void SetupProjectile(NetworkProjectile networkProjectile)
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
            var projectile = _slashProjectile.GetProjectile<SlashProjectile>();
            
            projectile.SetParentWeapon(this);
            _slashProjectile.transform.position = transform.position;
            projectile.SetRotation();
            projectile.gameObject.SetActive(true);
            projectile.SetNextStage();
            yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
            OnAttackEnd();
        }
    }
}