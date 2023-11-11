using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using Objects.Characters;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI.Main_Menu.Skill_Tree_Menu
{
    public class SkillTree : MonoBehaviour
    {
        [SerializeField] private SkillTreeNode rootNode;
        [SerializeField] private GameObject connectionPrefab;
        [SerializeField] public CharactersEnum characterId;
        [ColorUsage(true, true)]
        [SerializeField] public Color backColor;
        [ColorUsage(true, true)]
        [SerializeField] public Color frontColor;
        [ColorUsage(true, true)]
        [SerializeField] public Color lineColor;
        [SerializeField] private Image signetImage;
        [ColorUsage(true, true)]
        [SerializeField] public Color signetColor;
        
        private Vector3 _offset = new (0, 0, -100);
        private bool _loaded;
        private static readonly int LineRendererColorProperty = Shader.PropertyToID("_Color");

        public void Open(CharacterData characterData)
        {
            gameObject.SetActive(true);
            signetImage.sprite = characterData.signet;
            signetImage.material.SetColor(LineRendererColorProperty, signetColor);
            
            if (_loaded) 
            {
                LoadNodeState(rootNode, rootNode.nextNodes);
                return;
            }

            var characterSaveData = SaveFile.Instance.CharacterSaveData[characterId];
            var unlockedEntries = characterSaveData.GetUnlockedSkillPoints();
            TraverseNodes(rootNode, rootNode.nextNodes, unlockedEntries);
            LoadNodeState(rootNode, rootNode.nextNodes);
            _loaded = true;
        }

        private void TraverseNodes(SkillTreeNode node, List<SkillTreeNode> child, List<int> unlockedEntries)
        {
            if (unlockedEntries.Contains(node.skillNode.nodeId))
                node.MarkUpgraded();
            
            if (child == null)
                return;

            var rootNodeRect = node.GetComponent<RectTransform>();
            foreach (var childNodes in child)
            {
                var childNodeRect = childNodes.GetComponent<RectTransform>();
                childNodes.parentNodes.Add(node);
                CreateConnection(rootNodeRect.TransformPoint(rootNodeRect.rect.center), childNodeRect.TransformPoint(childNodeRect.rect.center));
                TraverseNodes(childNodes, childNodes.nextNodes, unlockedEntries);
            }
        }

        private void LoadNodeState(SkillTreeNode node, List<SkillTreeNode> child)
        {
            node.CalculateEnabled();
            node.Setup(characterId, frontColor, backColor);
            node.Refresh();
            if (child == null)
                return;
            
            foreach (var childNodes in child)
            {
                LoadNodeState(childNodes, childNodes.nextNodes);
            }
        }
        
        private void CreateConnection(Vector3 startPosition, Vector3 endPosition) {
            var connection = Instantiate(connectionPrefab, startPosition, Quaternion.identity);
            connection.transform.SetParent(transform, false);
            var lineRenderer = connection.GetComponent<LineRenderer>();
            
            lineRenderer.SetPositions(new[] { startPosition + _offset, endPosition + _offset });
            lineRenderer.material.SetColor(LineRendererColorProperty, lineColor);
        }
    }
}