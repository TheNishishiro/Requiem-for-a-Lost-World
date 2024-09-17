using System;
using NaughtyAttributes;
using Objects.Enemies;
using UnityEngine;

namespace Managers.StageEvents
{
    [Serializable]
    public class EnemySpawnData
    {
        public EnemyData enemy;
        [AllowNesting] [MinValue(0f), MaxValue(1f)]
        public float probability = 1f;
    }
}