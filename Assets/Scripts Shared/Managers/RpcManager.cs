using System;
using DefaultNamespace;
using Events.Scripts;
using Objects;
using Objects.Enemies;
using Objects.Stage;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Managers
{
    public class RpcManager : NetworkBehaviour
    {
        public static RpcManager instance;
        [SerializeField] private GameResultData gameResultData;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        
        [Rpc(SendTo.Server)]
        public void DealDamageToEnemyRpc(NetworkBehaviourReference  target, float damage)
        {
            if (target.TryGet(out Damageable damageableComponent))
            {
                damageableComponent.Health -= damage;
            }
        }
        
        [Rpc(SendTo.Server)]
        public void FireProjectileRpc(WeaponEnum weaponId, Vector3 spawnPosition, ulong clientId)
        {
            var projectile = NetworkObjectPool.Singleton.GetNetworkObject(WeaponManager.instance.weaponProjectilePrefabs[weaponId], spawnPosition, Quaternion.identity);
            var netObj = projectile.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership(clientId);
            
            FireProjectileRpc(weaponId, netObj, RpcTarget.Single(clientId, RpcTargetUse.Temp));
        }        
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void FireProjectileRpc(WeaponEnum weaponId, NetworkObjectReference objectReference, RpcParams rpcParams)
        {
            if (objectReference.TryGet(out var networkObject))
            {
                var unlockedWeapon = WeaponManager.instance.GetUnlockedWeapon(weaponId);
                unlockedWeapon.SetupProjectile(networkObject.gameObject);
            }
        }

        [Rpc(SendTo.Everyone)]
        public void AddEnemyKillRpc(bool isBoss)
        {
            gameResultData.MonstersKilled++;
            EnemyDiedEvent.Invoke();
            AchievementManager.instance.OnEnemyKilled(isBoss);
        }

        public void TriggerWin()
        {
            if (IsHost)
                WinRpc();
        }

        [Rpc(SendTo.Everyone)]
        public void WinRpc()
        {
            gameResultData.IsWin = true;
            FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(true);
        }
    }
}