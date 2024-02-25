using System;
using NaughtyAttributes;
using UnityEngine.Serialization;

namespace Data.Difficulty
{
    [Serializable]
    public class DifficultyData
    {
        public DifficultyEnum Difficulty;
        [ResizableTextArea]
        public string Description;

        public float RewardModifier = 1;
        public float PickUpValueModifier = 1;
        public float ExperienceGainModifier = 1;
        public float EnemyHealthModifier = 1;
        public float EnemyDamageModifier = 1;
        public float EnemySpawnRateModifier = 1;
        public float EnemyCapacityModifier = 1;
        public float EnemySpeedModifier = 1;
        public double ItemAttractionModifier = 1;
    }
}