using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.Skill_Tree_Menu
{
    public class SkillTreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public SkillNode skillNode;
        public List<SkillTreeNode> nextNodes;
        public List<SkillTreeNode> parentNodes = new ();
        public bool isEnabled;
        public bool isUpgraded;
        [SerializeField] private Image backGlow;
        [SerializeField] private Image frontGlow;
        private CharactersEnum _charactersEnum;
        private NodeDescriptionPanel _descriptionPanel;
        private static readonly int glowColorShaderProperty = Shader.PropertyToID("_GlowColor");

        private NodeDescriptionPanel DescriptionPanel
        {
            get
            {
                if (_descriptionPanel == null)
                    _descriptionPanel = FindFirstObjectByType<NodeDescriptionPanel>();
                return _descriptionPanel;
            }
        }

        
        public void Setup(CharactersEnum characterId, Color frontColor, Color backColor)
        {
            _charactersEnum = characterId;
            UpdateGlow(backGlow.material, backColor);
            UpdateGlow(frontGlow.material, frontColor);
        }
        
        public void Refresh()
        {
            backGlow.gameObject.SetActive(isUpgraded);
            frontGlow.sprite = skillNode.icon;
            frontGlow.gameObject.SetActive(isEnabled);
        }

        public void MarkUpgraded()
        {
            isEnabled = true;
            isUpgraded = true;
        }

        public void CalculateEnabled()
        {
            isEnabled = parentNodes == null || parentNodes.All(x => x.isUpgraded);
            Refresh();
        }

        public void OnUpgrade()
        {
            var characterData = SaveFile.Instance.CharacterSaveData[_charactersEnum];
            if (characterData.skillPoints <= 0 || isUpgraded || !isEnabled) return;

            characterData.skillPoints--;
            characterData.GetUnlockedSkillPoints().Add(skillNode.nodeId);
            isUpgraded = true;
            AudioManager.instance.PlayButtonConfirmClick();
            Refresh();

            if (nextNodes == null) return;
            foreach (var childNode in nextNodes)
            {
                childNode.CalculateEnabled();
                childNode.Refresh();
            }
            
            SaveManager.instance.SaveGame();
        }

        private void UpdateGlow(Material material, Color color)
        {
            if (!material.HasProperty(glowColorShaderProperty)) return;

            material.SetColor(glowColorShaderProperty, color);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            DescriptionPanel.Open(skillNode.GetDescription());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DescriptionPanel.Clear();
        }
    }
}