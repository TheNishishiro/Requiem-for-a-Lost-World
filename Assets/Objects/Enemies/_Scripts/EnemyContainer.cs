using System.Collections.Generic;
using UnityEngine;

namespace Objects.Enemies
{
    [CreateAssetMenu]
    public class EnemyContainer : ScriptableObject
    {
        [SerializeField] public List<EnemyData> possibleEnemies;
    }
}