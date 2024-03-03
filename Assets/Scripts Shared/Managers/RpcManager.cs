using System;
using DefaultNamespace;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;

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
        public void AddEnemyKillRpc()
        {
            gameResultData.MonstersKilled++;
        }
    }
}