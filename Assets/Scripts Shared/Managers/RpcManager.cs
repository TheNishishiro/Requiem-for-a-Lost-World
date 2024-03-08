using System;
using System.Linq;
using DefaultNamespace;
using Events.Scripts;
using Objects;
using Objects.Abilities;
using Objects.Enemies;
using Objects.Stage;
using UI.Labels.InGame.MP_List;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

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
        
        [Rpc(SendTo.Server)]
        public void LucySkillRpc(bool isE3, bool isE1)
        {
            var maxEnemies = isE3 ? 20 : 10;
			
            var enemies = EnemyManager.instance.GetActiveEnemies()
                .Where(x => isE1 || !x.IsBoss())
                .OrderBy(_ => Random.value)
                .Take(Random.Range(5, maxEnemies));
            foreach (var enemy in enemies)
            {
                enemy.MarkAsPlayerControlled(isE1 ? 25 : 10);
            }
        }

        [Rpc(SendTo.Server)]
        public void SpawnReviveCardRpc(Vector3 playerTransformPosition, ulong clientId)
        {
            var reviveCard = Instantiate(GameManager.instance.playerComponent.reviveCardPrefab, playerTransformPosition, Quaternion.identity).GetComponent<NetworkObject>();
            reviveCard.Spawn();
            reviveCard.GetComponent<ReviveCard>().SetClientId(clientId);
        }

        [Rpc(SendTo.Server)]
        public void DeSpawnReviveCardRpc(NetworkBehaviourReference networkBehaviourReference)
        {
            if (networkBehaviourReference.TryGet(out var networkObject))
            {
                Destroy(networkObject.gameObject);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void FireProjectileRpc(WeaponEnum weaponId, Vector3 spawnPosition, ulong clientId)
        {
            var projectile = NetworkObjectPool.Singleton.GetNetworkObject(WeaponManager.instance.weaponProjectilePrefabs[weaponId], spawnPosition, Quaternion.identity);
            var netObj = projectile.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership(clientId);
            
            FireProjectileRpc(weaponId, netObj.GetComponent<NetworkProjectile>(), RpcTarget.Single(clientId, RpcTargetUse.Temp));
        }        
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void FireProjectileRpc(WeaponEnum weaponId, NetworkBehaviourReference objectReference, RpcParams rpcParams)
        {
            if (objectReference.TryGet(out NetworkProjectile networkProjectile))
            {
                var unlockedWeapon = WeaponManager.instance.GetUnlockedWeapon(weaponId);
                unlockedWeapon.SetupProjectile(networkProjectile);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void DespawnProjectileRpc(NetworkObjectReference networkObjectReference, WeaponEnum weaponId)
        {
            if (networkObjectReference.TryGet(out var networkObject))
            {
                NetworkObjectPool.Singleton.ReturnNetworkObject(networkObject, WeaponManager.instance.weaponProjectilePrefabs[weaponId]);
                networkObject.Despawn(false);
            }
        }   
        
        [Rpc(SendTo.Server)]
        public void ParentObjectRpc(NetworkBehaviourReference networkBehaviourReference, ulong singletonLocalClientId)
        {
            if (networkBehaviourReference.TryGet(out var networkObject))
            {
                networkObject.transform.SetParent(NetworkManager.Singleton.ConnectedClients[singletonLocalClientId].PlayerObject.transform);
            }
        }   
        
        [Rpc(SendTo.Everyone)]
        public void AddEnemyRpc(NetworkBehaviourReference enemy)
        {
            if (enemy.TryGet(out Enemy enemyComponent))
            {
                EnemyManager.instance.AddEnemy(enemyComponent);
            }
        }

        [Rpc(SendTo.Everyone)]
        public void RemoveEnemyRpc(NetworkBehaviourReference enemy)
        {
            if (enemy.TryGet(out Enemy enemyComponent))
            {
                if (IsHost)
                {
                    var networkObject = enemyComponent.GetComponent<NetworkObject>();
                    NetworkObjectPool.Singleton.ReturnNetworkObject(networkObject, EnemyManager.instance.enemyGameObject);
                    networkObject.Despawn(false);
                }
			
                EnemyManager.instance.RemoveEnemy(enemyComponent);
            }
        }   

        public void HealPlayer(float healAmount, ulong clientId)
        {
            HealPlayerRpc(healAmount, RpcTarget.Single(clientId, RpcTargetUse.Temp));
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void HealPlayerRpc(float amount, RpcParams rpcParams)
        {
            GameManager.instance.playerComponent.TakeDamage(-amount);
        }
    }
}