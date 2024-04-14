using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using Objects.Characters;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class ShardScreenManager : MonoBehaviour, IStackableWindow
    {
        [SerializeField] private List<CharacterShard> characterShards;
        [SerializeField] private TextMeshProUGUI labelShardId;
        [SerializeField] private TextMeshProUGUI labelTitle;
        [SerializeField] private TextMeshProUGUI labelDescription;
        [SerializeField] private TextMeshProUGUI labelQuote;
        [SerializeField] private TextMeshProUGUI labelFragmentsRequired;
        [SerializeField] private SVGImage lockIcon;
        private int _selectedRank;

        public void Open(CharacterData characterData, int characterRank)
        {
            foreach (var characterShard in characterShards)
            {
                characterShard.Set(characterData);
                characterShard.IsUnlocked = characterShard.shardRank <= characterRank;
            }

            if (characterRank > 0)
                MarkSelected(1);

            labelFragmentsRequired.text = $"{SaveFile.Instance.CharacterSaveData[characterData.Id].Fragments}/50";
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }

        private void Update()
        {
            if (!IsInFocus) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedRank--;
                if (_selectedRank < 1)
                    _selectedRank = characterShards.Count;

                MarkSelected(_selectedRank);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedRank++;
                if (_selectedRank > characterShards.Count)
                    _selectedRank = 1;

                MarkSelected(_selectedRank);
            }
        }

        public void UnselectAll()
        {
            foreach (var characterShard in characterShards)
            {
                characterShard.IsSelected = true;
            }
            
            lockIcon.gameObject.SetActive(false);
            labelTitle.text = string.Empty;
            labelDescription.text = string.Empty;
            labelQuote.text = string.Empty;
        }

        public void MarkSelected(int shardRank)
        {
            _selectedRank = shardRank;
            foreach (var characterShard in characterShards)
            {
                characterShard.IsSelected = characterShard.shardRank == shardRank;
                
                if (characterShard.shardRank != shardRank) continue;
                lockIcon.gameObject.SetActive(!characterShard.IsUnlocked);
                labelDescription.color = characterShard.IsUnlocked ? Color.white : Color.gray;

                labelShardId.text = shardRank switch
                {
                    1 => "I",
                    2 => "II",
                    3 => "III",
                    4 => "IV",
                    5 => "V",
                    _ => labelShardId.text
                };

                labelTitle.text = characterShard.currentShard.EidolonName;
                labelDescription.text = characterShard.currentShard.GetDescription();
                labelQuote.text = string.Empty;
                
                if (!characterShard.IsUnlocked) continue;
                characterShard.IsSelected = true;
                labelQuote.text = characterShard.currentShard.EidolonQuote;
            }
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}