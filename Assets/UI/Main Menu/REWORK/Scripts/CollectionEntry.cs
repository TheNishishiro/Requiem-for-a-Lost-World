using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Collection;
using Interfaces;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class CollectionEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelName;
        [SerializeField] private Image imageIcon;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Material materialBlackAndWhite;
        private IPlayerItem _currentItem;
        private EidolonData _currentEidolon;
        private CharacterData _currentCharacter;

        public void Setup(IPlayerItem collectionItem)
        {
            _currentItem = collectionItem;
            rectTransform.Rotate(new Vector3(0,0,Utilities.RandomDoubleRange(-7f, -3f, 3f, 7f)));
            Refresh();
        }

        public void Setup(EidolonData characterEidolon, CharacterData characterData)
        {
            _currentEidolon = characterEidolon;
            _currentCharacter = characterData;
            rectTransform.Rotate(new Vector3(0,0,Utilities.RandomDoubleRange(-7f, -3f, 3f, 7f)));
            Refresh();
        }

        public void Setup(CharacterData characterData)
        {
            _currentCharacter = characterData;
            rectTransform.Rotate(new Vector3(0,0,Utilities.RandomDoubleRange(-7f, -3f, 3f, 7f)));
            Refresh();
        }

        public void ShowDetails()
        {
            if (_currentItem != null)
                ShowItemDetails();
            else if (_currentEidolon != null)
                ShowShardDetails();
        }

        public void Refresh()
        {
            if (_currentItem != null) RefreshItem();
            else if (_currentEidolon != null) RefreshShard();
            else if (_currentCharacter != null) RefreshCharacter();
        }

        public void VisibleState(CollectionState state)
        {
            var isUnlocked = IsUnlocked();
            var filterUnlockState = state == CollectionState.Collected;
            var isVisible = isUnlocked == filterUnlockState || state == CollectionState.All;
            gameObject.SetActive(isVisible);
        }
        
        private void ShowItemDetails()
        {
            CollectionScreenManager.instance.Display(
                _currentItem.NameField,
                _currentItem.GetDescription(1),
                _currentItem.RequiredAchievementField?.GetDescription(),
                _currentItem.IsUnlocked(SaveFile.Instance),
                _currentItem.ElementField,
                _currentItem.AttackTypeField);
        }
        
        private void ShowShardDetails()
        {
            var isUnlocked = IsShardUnlocked();
            CollectionScreenManager.instance.Display(
                _currentEidolon.EidolonName,
                isUnlocked ? _currentEidolon.GetDescription() + $"<br><br><i><size=70%><color=#D8D8D8>{_currentEidolon.EidolonQuote}</color></size></i>" : _currentEidolon.GetDescription(),
                $"Unlock by obtaining duplicates of {_currentCharacter.Name}",
                isUnlocked);
        }

        private void RefreshItem()
        {
            var isUnlocked = _currentItem.IsUnlocked(SaveFile.Instance);
            imageIcon.material = isUnlocked ? null : materialBlackAndWhite;
            imageIcon.sprite = _currentItem.IconField;
            imageIcon.color = isUnlocked ? Color.white : Color.gray;
            labelName.text = _currentItem.IsUnlocked(SaveFile.Instance) ? _currentItem.NameField : "???";

        }

        private void RefreshShard()
        {
            var isUnlocked = IsShardUnlocked();
            imageIcon.material = isUnlocked ? null : materialBlackAndWhite;
            imageIcon.sprite = _currentEidolon.EidolonTexture;
            imageIcon.color = isUnlocked ? Color.white : Color.gray;
            labelName.text = isUnlocked ? _currentEidolon.EidolonName : "???";
        }

        private void RefreshCharacter()
        {
            var isUnlocked =  SaveFile.Instance.GetCharacterSaveData(_currentCharacter.Id).IsUnlocked;
            imageIcon.material = isUnlocked ? null : materialBlackAndWhite;
            imageIcon.sprite = _currentCharacter.Avatar;
            imageIcon.color = isUnlocked ? Color.white : Color.gray;
            labelName.text = isUnlocked ? _currentCharacter.Name : "???";
        }

        private bool IsUnlocked()
        {
            if (_currentItem != null) 
                return _currentItem.IsUnlocked(SaveFile.Instance);
            if (_currentEidolon != null) 
                return IsShardUnlocked();
            if (_currentCharacter != null)
                return SaveFile.Instance.GetCharacterSaveData(_currentCharacter.Id).IsUnlocked;
            return false;
        }

        private bool IsShardUnlocked()
        {
            var characterSaveData = SaveFile.Instance.GetCharacterSaveData(_currentCharacter.Id);
            var shardIndex = _currentCharacter.Eidolons.FindIndex(x => x == _currentEidolon) + 1;
            return characterSaveData.RankUpLevel >= shardIndex;
        }
    }
}