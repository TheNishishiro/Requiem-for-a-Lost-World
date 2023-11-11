using System.Collections.Generic;
using System.Linq;
using Objects.Characters;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;

namespace UI.Main_Menu.Skill_Tree_Menu
{
    public class SkillTreePanel : MonoBehaviour
    {
        [SerializeField] private List<SkillTree> skillTrees;
        
        public void Open(CharacterData characterData)
        {
            foreach (var skillTree in skillTrees)
            {
                skillTree.gameObject.SetActive(false);
            }
            
            skillTrees.FirstOrDefault(x => x.characterId == characterData.Id)?.Open(characterData);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}