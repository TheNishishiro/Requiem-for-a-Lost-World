using System;
using System.Text;
using Data.Difficulty;
using DefaultNamespace.Data;
using Managers;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Level_Selection
{
    public class DifficultySelectionPanel : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private TMP_Dropdown difficultyDropdown;
        [SerializeField] private TextMeshProUGUI difficultyDescriptionField;
        [SerializeField] private DifficultyContainer difficultyContainer;

        public void Save()
        {
            saveManager.GetSaveFile().SelectedDifficulty = (DifficultyEnum) difficultyDropdown.value;
            saveManager.SaveGame();
            UpdateDescription();
        }

        private void OnEnable()
        {
            difficultyDropdown.value = (int) saveManager.GetSaveFile().SelectedDifficulty;
            UpdateDescription();
        }

        private void UpdateDescription()
        {
            difficultyDescriptionField.text = difficultyContainer.GetData(saveManager.GetSaveFile().SelectedDifficulty).Description;
        }
    }
}