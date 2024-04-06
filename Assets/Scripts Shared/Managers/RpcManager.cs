using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DefaultNamespace;
using Events.Scripts;
using Objects;
using Objects.Abilities;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Labels.InGame.MP_List;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Managers
{
    public class RpcManager : NetworkBehaviour
    {
        public static RpcManager instance;
        [SerializeField] private GameObject shrinePrefab;

        public override void OnNetworkSpawn()
        {
            if (instance == null)
                instance = this;
            
            base.OnNetworkSpawn();
        }
        
        [Rpc(SendTo.Server)]
        public void SpawnShrinesRpc(int amount, Vector3 mapCenter, float spawnRange, float minDistance)
        {
            var spawnedPositions = Utilities.GetPositionsOnSurfaceWithMinDistance(amount, mapCenter, spawnRange, minDistance, transform, 100);
            foreach (var position in spawnedPositions)
            {
                NetworkObjectPool.Singleton.GetNetworkObject(shrinePrefab, position, Quaternion.identity).Spawn();
            }
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
            GameResultData.MonstersKilled++;
            EnemyDiedEvent.Invoke();
            AchievementManager.instance.OnEnemyKilled(isBoss);
        }

        public void TriggerWin()
        {
            if (IsHost)
                WinRpc();
        }

        [Rpc(SendTo.Server)]
        public void TriggerLoseServerRpc()
        {
            LoseRpc();
        }

        [Rpc(SendTo.Everyone)]
        public void WinRpc()
        {
            GameResultData.IsWin = true;
            FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(true, true);
        }

        [Rpc(SendTo.Everyone)]
        public void LoseRpc()
        {
            Debug.Log("Displaying lose screen");
            GameResultData.IsWin = false;
            FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(false, true);
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
        public void SpectatePlayerServerRpc(ulong spectatingClientId, ulong clientToSpectateId)
        {
            var playerComponent = NetworkManager.Singleton.ConnectedClients[spectatingClientId].PlayerObject.GetComponent<MultiplayerPlayer>();
            var targetComponent = NetworkManager.Singleton.ConnectedClients[clientToSpectateId].PlayerObject.GetComponent<MultiplayerPlayer>();

            SpectatePlayerServerRpc(playerComponent, targetComponent, RpcTarget.Single(spectatingClientId, RpcTargetUse.Temp));
        }

        [Rpc(SendTo.SpecifiedInParams)]
        public void SpectatePlayerServerRpc(NetworkBehaviourReference spectatingClient, NetworkBehaviourReference clientToSpectate, RpcParams rpcParams)
        {
            if (!spectatingClient.TryGet(out MultiplayerPlayer source)) return;
            if (!clientToSpectate.TryGet(out MultiplayerPlayer target)) return;

            source.SetCameraTarget(target.cameraRoot.transform);
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
        
        [Rpc(SendTo.Server)]
        public void RevivePlayerServerRpc(NetworkObjectReference shrine, Vector3 respawnPointPosition, ulong clientId)
        {
            ReviveRpc(respawnPointPosition, RpcTarget.Single(clientId, RpcTargetUse.Temp));
           
            if (shrine.TryGet(out var oNetworkBehaviour))
            {
                MarkShrineUsedClientRpc(oNetworkBehaviour.GetComponent<Shrine>());
            }
        }
        
        [Rpc(SendTo.Everyone)]
        public void MarkShrineUsedClientRpc(NetworkBehaviourReference shrine)
        {
            if (shrine.TryGet(out Shrine oNetworkBehaviour))
            {
                oNetworkBehaviour.MarkAsUsed();
            }
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void ReviveRpc(Vector3 respawnPointPosition, RpcParams rpcParams)
        {
            GameManager.instance.PlayerTransform.position = respawnPointPosition;
            GameManager.instance.playerStatsComponent.SetHealth(PlayerStatsScaler.GetScaler().GetMaxHealth());
            GameManager.instance.playerComponent.healthComponent.UpdateHealthBar();
            GameManager.instance.playerStatsComponent.ChangeDeathState(false);
            GameManager.instance.playerMpComponent.ResetCameraFollow();
            GameManager.instance.playerMpComponent.SetCollider(true);
        }
    }
}