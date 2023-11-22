using System.Collections.Generic;
using System.Linq;
using Objects.Players;
using Objects.Stage;

namespace Objects.Characters
{
    public class CharacterStrategyBase
    {
        public virtual void ApplySkillTree(PlayerStats stats, List<int> unlockedTreeNodeIds)
        {
            foreach (var skillNode in GameData.GetCharacterSkillNodes().Where(x => unlockedTreeNodeIds.Contains(x.nodeId) ))
            {
                stats.Sum(skillNode.stats, 1);
            }
        }
    }
}