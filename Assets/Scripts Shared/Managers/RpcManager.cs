using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Data.Elements;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Events.Scripts;
using Objects;
using Objects.Abilities;
using Objects.Characters;
using Objects.Enemies;
using Objects.Enemies.EnemyWeapons;
using Objects.Items;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.In_Game.GUI.Scripts.Managers;
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

        public override void OnNetworkSpawn()
        {
            if (instance == null)
                instance = this;
            
            base.OnNetworkSpawn();
        }
        
        [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable)]
        public void DealDamageToEnemyRpc(NetworkBehaviourReference target, float baseDamage, bool isCriticalHit, Element weaponElement, WeaponEnum weaponId, float elementalReactionEffectIncreasePercentage,
            float resPen, bool isRecursion, ulong clientId)
        {
            if (target.TryGet(out Damageable damageableComponent))
            {
                damageableComponent.TakeDamageServer(baseDamage, isCriticalHit, weaponElement, weaponId, elementalReactionEffectIncreasePercentage, resPen, isRecursion, clientId);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void ReduceEnemyResistanceRpc(NetworkBehaviourReference target, Element element, float amount)
        {
            if (target.TryGet(out Damageable damageableComponent))
            {
                damageableComponent.ReduceElementalDefenceServer(element, amount);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void EnemyTakeAdditionalDamageAsFollowUpRpc(NetworkBehaviourReference target, Element weaponElement, float duration, float modifier)
        {
            if (target.TryGet(out Damageable damageableComponent))
            {
                damageableComponent.SetTakeAdditionalDamageFromAllSourcesServer(weaponElement, duration, modifier);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void SetEnemyVulnerableRpc(NetworkBehaviourReference target, WeaponEnum weaponId, float time, float percentage)
        {
            if (target.TryGet(out Damageable damageableComponent))
            {
                damageableComponent.SetVulnerableServer(weaponId, time, percentage);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void SetEnemySlowRpc(NetworkBehaviourReference target, float slowTime, float slowAmount)
        {
            if (target.TryGet(out ChaseComponent chaseComponent))
            {
                chaseComponent.SetSlowServer(slowTime, slowAmount);
            }
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void TriggerLifeStealRpc(float calculatedDamage, WeaponEnum weaponId, RpcParams rpcParams)
        {
            var weapon = WeaponManager.instance.GetUnlockedWeapon(weaponId);
            if (weapon.WeaponStatsStrategy == null) return;
            
            var lifeSteal = weapon.WeaponStatsStrategy.GetLifeSteal();
            if (lifeSteal != 0)
                GameManager.instance.playerComponent.TakeDamage(-calculatedDamage * lifeSteal, true, true);

            var healPerHit = weapon.WeaponStatsStrategy.GetHealPerHit(false);
            if (healPerHit != 0)
                GameManager.instance.playerComponent.TakeDamage(-healPerHit, true, true);
        }

        [Rpc(SendTo.Everyone)]
        public void AddEnemyKillRpc(bool isBoss, EnemyTypeEnum enemyTypeValue)
        {
            GameResultData.MonstersKilled++;
            EnemyDiedEvent.Invoke();
            AchievementManager.instance.OnEnemyKilled(isBoss, enemyTypeValue);
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
        public void PauseRpc(bool isPlayerVote)
        {
            CursorManager.instance.ShowCursor();
            if (isPlayerVote)
                GameManager.instance.playerMpComponent.ResetVoteUnpause();
            if (IsClient)
                GuiManager.instance.ToggleWaitingForPlayers(true);
            if (IsHost)
                GuiManager.instance.ToggleWaitingForPlayers(NetworkManager.Singleton.ConnectedClients.Count > 1);
            
            Time.timeScale = 0;
        }

        [Rpc(SendTo.Everyone)]
        public void UnPauseRpc()
        {
            CursorManager.instance.HideCursor();
            GuiManager.instance.ToggleWaitingForPlayers(false);
            Time.timeScale = 1;
        }

        [Rpc(SendTo.Everyone)]
        public void WinRpc()
        {
            GameResultData.IsWin = true;
            FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(true);
        }

        [Rpc(SendTo.Everyone)]
        public void LoseRpc()
        {
            GameResultData.IsWin = false;
            FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(false);
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

            source.SetCameraTarget(target.GetRootCamera().transform);
        }
        
        [Rpc(SendTo.Server)]
        public void FireProjectileRpc(WeaponEnum weaponId, Vector3 spawnPosition, ulong clientId, WeaponPoolEnum weaponPoolId)
        {
            var prefab = WeaponManager.instance.GetWeapon(weaponId).GetProjectile(weaponPoolId);
            var projectile = NetworkObjectPool.Singleton.GetNetworkObject(prefab, spawnPosition, Quaternion.identity);
            var netObj = projectile.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership(clientId);
            FireProjectileRpc(weaponId, weaponPoolId, netObj.GetComponent<NetworkProjectile>(), RpcTarget.Single(clientId, RpcTargetUse.Temp));
        }        
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void FireProjectileRpc(WeaponEnum weaponId, WeaponPoolEnum weaponPoolId, NetworkBehaviourReference objectReference, RpcParams rpcParams)
        {
            if (objectReference.TryGet(out NetworkProjectile networkProjectile))
            {
                var unlockedWeapon = WeaponManager.instance.GetUnlockedWeapon(weaponId);
                unlockedWeapon.SetupProjectile(networkProjectile, weaponPoolId);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void DespawnProjectileRpc(NetworkObjectReference networkObjectReference, WeaponEnum weaponId, WeaponPoolEnum weaponPoolId)
        {
            if (networkObjectReference.TryGet(out var networkObject))
            {
                var prefab = WeaponManager.instance.GetWeapon(weaponId).GetProjectile(weaponPoolId);
                NetworkObjectPool.Singleton.ReturnNetworkObject(networkObject, prefab);
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
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void AttackPlayerRpc(float damage, RpcParams rpcParams)
        {
            GameManager.instance.playerComponent.TakeDamage(damage);
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

        [Rpc(SendTo.Server)]
        public void AttackPlayerRpc(ulong clientId, float damage)
        {
            AttackPlayerRpc(damage, RpcTarget.Single(clientId, RpcTargetUse.Temp));
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

        public void TempStatIncrease(StatEnum statEnum, float buffAmount, float buffTime, string buffId, ulong clientId)
        {
            TempStatIncreaseRpc(statEnum, buffAmount, buffTime, buffId, RpcTarget.Single(clientId, RpcTargetUse.Temp));
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void TempStatIncreaseRpc(StatEnum statEnum, float buffAmount, float buffTime, string buffId, RpcParams rpcParams)
        {
            GameManager.instance.playerStatsComponent.TemporaryStatBoost(buffId, statEnum, buffAmount, buffTime);
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void InvokeDamageDealtEventRpc(NetworkBehaviourReference damageable, float calculatedDamage, bool isRecursion, WeaponEnum weaponId, RpcParams rpcParams)
        {
            if (weaponId == WeaponEnum.Unset)
                return;
            
            if (damageable.TryGet(out Damageable damageableComponent))
            {
                DamageDealtEvent.Invoke(damageableComponent, calculatedDamage, isRecursion, weaponId);
            }
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void WeaponKilledEnemyRpc(WeaponEnum weaponId, RpcParams rpcParams)
        {
            if (weaponId == WeaponEnum.Unset)
                return;
            WeaponManager.instance.GetUnlockedWeapon(weaponId).OnEnemyKilled();
        }
        
        [Rpc(SendTo.SpecifiedInParams)]
        public void InvokeReactionTriggeredEventRpc(NetworkBehaviourReference damageable, ElementalReaction reactionResultReaction, RpcParams rpcParams)
        {
            if (damageable.TryGet(out Damageable damageableComponent))
            {
                ReactionTriggeredEvent.Invoke(reactionResultReaction, damageableComponent);
            }
        }
        
        [Rpc(SendTo.Server)]
        public void AddItemToCoopPlayerListRpc(ulong clientId, ItemEnum itemId)
        {
            PropagateItemToOnlinePlayerListRpc(clientId, itemId, WeaponEnum.Unset);
        }
        
        [Rpc(SendTo.Server)]
        public void AddWeaponToCoopPlayerListRpc(ulong clientId, WeaponEnum weaponId)
        {
            PropagateItemToOnlinePlayerListRpc(clientId, ItemEnum.Unset, weaponId);
        }
        
        [Rpc(SendTo.Everyone)]
        public void PropagateItemToOnlinePlayerListRpc(ulong clientId, ItemEnum itemId, WeaponEnum weaponId)
        {
            MpActivePlayersInGameList.instance.UpdatePlayerItems(clientId, itemId, weaponId);
        }

        [Rpc(SendTo.Everyone, Delivery = RpcDelivery.Reliable)]
        public void AddExperienceRpc(float expAmount)
        {
            GameManager.instance.playerComponent.AddExperience(expAmount);
        }
    }
}