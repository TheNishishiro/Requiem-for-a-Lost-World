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