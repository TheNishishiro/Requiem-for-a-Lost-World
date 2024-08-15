using System;
using System.Collections;
using System.Collections.Generic;
using Data.Difficulty;
using DefaultNamespace.Attributes;
using DefaultNamespace.Data.Modals;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Stage;
using TMPro;
using UI.Main_Menu.REWORK.Scripts;
using Unity.Netcode;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityTemplateProjects;

public class GameSettingsScreenManager : MonoBehaviour, IStackableWindow
{
    [BoxGroup("Containers")] [SerializeField] private DifficultyContainer containerDifficulties;
    [Space]
    [BoxGroup("Images")] [SerializeField] private Image imageBackground;
    [BoxGroup("Images")] [SerializeField] private Image imageCharacterCard;
    [BoxGroup("Images")] [SerializeField] private Image imageSettingsLine;
    [BoxGroup("Images")] [SerializeField] private Image imageEffectsLine;
    [BoxGroup("Images")] [SerializeField] private Image imageGlowLine;
    [Space]
    [BoxGroup("Buffs")] [SerializeField] private SVGImage svgDifficulty;
    [BoxGroup("Buffs")] [SerializeField] private SVGImage svgCoop;
    [BoxGroup("Buffs")] [SerializeField] private SVGImage svgSpeedUpGameTime;
    [Space]
    [BoxGroup("Assets")] [SerializeField] private Sprite svgLowDifficulty;
    [BoxGroup("Assets")] [SerializeField] private Sprite svgMediumDifficulty;
    [BoxGroup("Assets")] [SerializeField] private Sprite svgHighDifficulty;
    [BoxGroup("Assets")] [SerializeField] private Sprite svgInsaneDifficulty;
    [Space]
    [BoxGroup("Materials")] [SerializeField] private Material materialNeutral;
    [BoxGroup("Materials")] [SerializeField] private Material materialSelectionPositive;
    [BoxGroup("Materials")] [SerializeField] private Material materialSelectionNegative;
    [BoxGroup("Materials")] [SerializeField] private Material materialSelectionNeutral;
    [Space]
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelDifficultyNormal;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelDifficultyHard;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelDifficultyVeryHard;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelDifficultyInsane;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelCoopPlayAllow;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelCoopPlayDeny;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelShortPlayTime;

    public void SetDifficulty(int difficultyId)
    {
        var difficulty = difficultyId != 0 ? (DifficultyEnum)difficultyId : DifficultyEnum.Normal;
        SaveManager.instance.GetSaveFile().SelectedDifficulty = difficulty;
        
        labelDifficultyNormal.fontSharedMaterial = difficulty == DifficultyEnum.Normal ? materialSelectionPositive : materialNeutral;
        labelDifficultyHard.fontSharedMaterial = difficulty == DifficultyEnum.Hard ? materialSelectionNeutral : materialNeutral;
        labelDifficultyVeryHard.fontSharedMaterial = difficulty == DifficultyEnum.VeryHard ? materialSelectionNeutral : materialNeutral;
        labelDifficultyInsane.fontSharedMaterial = difficulty == DifficultyEnum.Insane ? materialSelectionNegative : materialNeutral;

        svgDifficulty.sprite = difficulty switch
        {
            DifficultyEnum.Normal => svgLowDifficulty,
            DifficultyEnum.Hard => svgMediumDifficulty,
            DifficultyEnum.VeryHard => svgHighDifficulty,
            DifficultyEnum.Insane => svgInsaneDifficulty,
            _ => svgDifficulty.sprite
        };
        
        var difficultyData = containerDifficulties.GetData(difficulty);
        GameData.SetCurrentDifficultyData(difficultyData);
    }

    public void SetCoopPreference(bool isAllow)
    {
        SaveManager.instance.GetSaveFile().IsCoopAllowed = isAllow;
        labelCoopPlayAllow.fontSharedMaterial = isAllow ? materialSelectionPositive : materialNeutral;
        labelCoopPlayDeny.fontSharedMaterial = !isAllow ? materialSelectionNegative : materialNeutral;
        svgCoop.gameObject.SetActive(isAllow);

        NetworkingContainer.IsAllowJoins = isAllow;
    }

    public void ToggleShortPlayTime()
    {
        GameSettings.IsShortPlayTime = !GameSettings.IsShortPlayTime;
        SaveManager.instance.GetSaveFile().IsShortPlayTime = GameSettings.IsShortPlayTime;
        labelShortPlayTime.fontSharedMaterial = GameSettings.IsShortPlayTime ? materialSelectionPositive : materialNeutral;
    }

    public void Open()
    {
        var character = CharacterListManager.instance.GetActiveCharacter();
        imageCharacterCard.sprite = character.CharacterCard;
        imageSettingsLine.color = character.ColorTheme;
        imageEffectsLine.color = character.ColorTheme;
        imageGlowLine.color = character.ColorTheme;

        var stage = GameData.GetCurrentStage();
        imageBackground.sprite = stage.backgroundBlur;
        var saveFile = SaveManager.instance.GetSaveFile();
        if (saveFile.IsShortPlayTime != GameSettings.IsShortPlayTime)
            ToggleShortPlayTime();
        SetCoopPreference(saveFile.IsCoopAllowed);
        SetDifficulty((int)saveFile.SelectedDifficulty);
        labelShortPlayTime.fontSharedMaterial = GameSettings.IsShortPlayTime ? materialSelectionPositive : materialNeutral;
        
        StackableWindowManager.instance.OpenWindow(this);
    }

    public void Close()
    {
        StackableWindowManager.instance.CloseWindow(this);
    }

    public void Update()
    {
        if (!IsInFocus) return;
        
        if (Input.GetKeyDown(KeyCode.Return))
            StartLevel();
        
        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }
    
    public void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine(GameData.GetCurrentStage().id));
    }

    private IEnumerator StartLevelCoroutine(StageEnum currentStage)
    {
        try
        {
            ModalManager.instance.Open(ButtonCombination.None, "Loading", "Loading the level...", modalState: ModalState.Info);
            if (!NetworkManager.Singleton.ShutdownInProgress)
                NetworkManager.Singleton.Shutdown();

            while (NetworkManager.Singleton.ShutdownInProgress);

            NetworkingContainer.IsHostPlayer = true;
            AudioManager.instance.PlayButtonConfirmClick();
            SceneManager.LoadScene(currentStage.GetStringValue(), LoadSceneMode.Single);
            SceneManager.LoadScene("Scenes/Essential", LoadSceneMode.Additive);
			
            yield break;
        }
        catch (Exception e)
        {
            ModalManager.instance.Open(ButtonCombination.None, "Loading", $"Failed to load the game: {e.Message}", modalState: ModalState.Error);
        }
    }
    
    public bool IsInFocus { get; set; }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
