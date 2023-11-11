using Objects.Items;
using UnityEngine;

namespace Objects.Characters
{
    [CreateAssetMenu]
    public class SkillNode : ScriptableObject
    {
        public int nodeId;
        public string description;
        public Sprite icon;
        public ItemStats stats;

        public string GetDescription()
        {
            return stats.GetDescription(description, 1);
        }
    }
}