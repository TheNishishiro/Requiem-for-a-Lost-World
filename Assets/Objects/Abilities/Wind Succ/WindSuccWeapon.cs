using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Managers;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Succ
{
    public class WindSuccWeapon : PoolableWeapon<WindSuccProjectile>
    {
        private Vector3 _shockwavePosition;
        private bool IsShockwaveActive;
        
        public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
        {
            if (weaponPoolId == WeaponPoolEnum.Main)
            {
                var position = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(transform.position, 5), transform);
                networkProjectile.Initialize(this, position);
            }
            else
            {
                networkProjectile.Initialize(this, _shockwavePosition);
            }
        }

        public void SpawnShockwave(Vector3 spawnPosition)
        {
            if (!IsShockwaveActive) return;
            
            _shockwavePosition = spawnPosition;
            RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Sub);
        }
        
        protected override void OnLevelUp()
        {
            if (LevelField == 8)
                IsShockwaveActive = true;
        }
    }
}