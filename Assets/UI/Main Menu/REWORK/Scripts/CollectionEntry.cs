using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Collection;
using Interfaces;
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

        public void Setup(IPlayerItem collectionItem)
        {
            _currentItem = collectionItem;
            rectTransform.Rotate(new Vector3(0,0,Utilities.RandomDoubleRange(-7f, -3f, 3f, 7f)));
            Refresh();
        }

        public void ShowDetails()
        {
            CollectionScreenManager.instance.Display(_currentItem);
        }

        public void Refresh()
        {
            var isUnlocked = _currentItem.IsUnlocked(SaveFile.Instance);
            imageIcon.material = isUnlocked ? null : materialBlackAndWhite;
            imageIcon.sprite = _currentItem.IconField;
            imageIcon.color = isUnlocked ? Color.white : Color.gray;
            labelName.text = _currentItem.IsUnlocked(SaveFile.Instance) ? _currentItem.NameField : "???";
        }

        public void VisibleState(CollectionState state)
        {
            var isUnlocked = _currentItem.IsUnlocked(SaveFile.Instance);
            var filterUnlockState = state == CollectionState.Collected;
            var isVisible = isUnlocked == filterUnlockState || state == CollectionState.All;
            gameObject.SetActive(isVisible);
        }
    }
}