using System;
using System.Collections.Generic;
using Data.Elements;
using Unity.Netcode;

namespace Objects.Enemies
{
    [Serializable]
    public record EnemyNetworkStats : INetworkSerializable
    {
        public float hp;
        public float damage;
        public float speed;
        public bool allowFlying;
        public float groundOffset;
        public bool isBossEnemy;
        public int expDrop;
        public EnemyTypeEnum enemyType;
        public NetworkList<ElementStatsSimple> elementStats = new ();

        public EnemyNetworkStats()
        {
        }

        public EnemyNetworkStats(EnemyData enemyData)
        {
            hp = enemyData.stats.hp;
            damage = enemyData.stats.damage;
            speed = enemyData.stats.speed;
            allowFlying = enemyData.allowFlying;
            groundOffset = enemyData.groundOffset;
            isBossEnemy = enemyData.isBossEnemy;
            expDrop = enemyData.ExpDrop;
            enemyType = enemyData.enemyType;
            foreach (var elementStat in enemyData.stats.elementStats)
            {
                elementStats.Add(new ElementStatsSimple()
                {
                    element = elementStat.element,
                    damageReduction = elementStat.damageReduction
                });
            }
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref hp);
            serializer.SerializeValue(ref damage);
            serializer.SerializeValue(ref speed);
            serializer.SerializeValue(ref allowFlying);
            serializer.SerializeValue(ref groundOffset);
            serializer.SerializeValue(ref isBossEnemy);
            serializer.SerializeValue(ref expDrop);
            serializer.SerializeValue(ref enemyType);
        }
    }
}