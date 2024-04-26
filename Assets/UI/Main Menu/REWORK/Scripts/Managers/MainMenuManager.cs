using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Managers;
using NUnit.Framework;
using Objects.Stage;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class MainMenuManager : MonoBehaviour, IStackableWindow
    {
        public static MainMenuManager instance;
        [SerializeField] private StageSelectionManager stageSelectionManager;
        [SerializeField] private GameResultScreenManager gameResultScreenManager;
        [SerializeField] private SettingsScreenManager settingsScreenManager;
        [SerializeField] private CharacterSelectionScreenManager characterSelectionScreenManager;
        [SerializeField] private AchievementScreenManager achievementScreenManager;
        [SerializeField] private RecollectionScreenManager recollectionScreenManager;
        [SerializeField] private ShopScreenManager shopScreenManager;
        [SerializeField] private StoryScreenManager storyScreenManager;
        [SerializeField] private CollectionScreenManager collectionScreenManager;
        [SerializeField] private List<MainMenuButton> mainMenuButtons;
        private int selectedIndex;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            
            StackableWindowManager.instance.OpenWindow(this);
            SetSelected(mainMenuButtons.First().GetInstanceID());

            if (GameResultData.IsGameEnd)
            {
                gameResultScreenManager.Open();
            }
        }

        public void Update()
        {
            if (!IsInFocus) return;
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = mainMenuButtons.Count - 1;
                }
                SetSelected(mainMenuButtons[selectedIndex].GetInstanceID());
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedIndex++;
                if (selectedIndex >= mainMenuButtons.Count)
                {
                    selectedIndex = 0;
                }
                SetSelected(mainMenuButtons[selectedIndex].GetInstanceID());
            }
            else if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.JoystickButton10))
            {
                mainMenuButtons[selectedIndex].Execute();
            }
        }

        public void SetSelected(int instanceId)
        {
            selectedIndex = mainMenuButtons.FindIndex(x => x.GetInstanceID() == instanceId);
            mainMenuButtons.ForEach(x => x.SetFocused(false));
            mainMenuButtons[selectedIndex].SetFocused(true);
        }

        public void OpenStageSelection()
        {
            stageSelectionManager.Open();
        }

        public void OpenCoopCharacterSelection()
        {
            characterSelectionScreenManager.Open(true);
        }

        public void OpenSettings()   
        {
            settingsScreenManager.Open();
        }

        public void OpenAchievements()   
        {
            achievementScreenManager.Open();
        }

        public void OpenGamba()   
        {
            recollectionScreenManager.Open();
        }

        public void OpenShop()   
        {
            shopScreenManager.Open();
        }

        public void OpenStory()   
        {
            storyScreenManager.Open();
        }

        public void OpenCollection()   
        {
            collectionScreenManager.Open();
        }
        
        public void QuitApplication()
        {
            Application.Quit();
        }

        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}