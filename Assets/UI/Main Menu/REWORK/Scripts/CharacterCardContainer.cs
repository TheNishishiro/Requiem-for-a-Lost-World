using System;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class CharacterCardContainer : MonoBehaviour
    {
        private CharacterSelectionScreenManager characterSelectionScreenManager;

        private void Start()
        {
            characterSelectionScreenManager = GetComponentInParent<CharacterSelectionScreenManager>();
        }

        public void UpdateList()
        {
            characterSelectionScreenManager.UpdateListDisplay();
        }
    }
}