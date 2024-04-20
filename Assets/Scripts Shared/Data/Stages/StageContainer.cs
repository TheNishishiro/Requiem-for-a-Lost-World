using System.Collections.Generic;
using Data.Difficulty;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Data.Stages
{
    [CreateAssetMenu]
    public class StageContainer : ScriptableObject
    {
        public List<StageDefinition> stageDefinitions;
        
        public StageDefinition GetData(StageEnum stageEnum)
        {
            return stageDefinitions.Find(x => x.id == stageEnum);
        }

        public StageDefinition GetData(int index)
        {
            return stageDefinitions[index];
        }

        public int Count()
        {
            return stageDefinitions?.Count ?? 0;
        }
    }
}