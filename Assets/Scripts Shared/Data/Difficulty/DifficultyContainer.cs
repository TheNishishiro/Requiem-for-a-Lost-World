using System.Collections.Generic;
using UnityEngine;

namespace Data.Difficulty
{
    [CreateAssetMenu]
    public class DifficultyContainer : ScriptableObject
    {
        public List<DifficultyData> difficultyDefinitions;
        
        public DifficultyData GetData(DifficultyEnum difficulty)
        {
            return difficultyDefinitions.Find(x => x.Difficulty == difficulty);
        }
    }
}