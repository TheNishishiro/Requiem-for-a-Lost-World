using Objects.Characters;
using UnityEngine;

namespace UI.Main_Menu.Skill_Tree_Menu
{
    public class SkillTreePanel : MonoBehaviour
    {
        public void Open(CharacterData characterData)
        {
            gameObject.SetActive(true);
        }
    }
}