using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Stage;
using TMPro;
using UI.Labels.InGame;
using UI.Main_Menu.REWORK.Scripts;
using UI.UI_Elements;
using UnityEngine;
using UnityEngine.UI;

public class GameResultScreenManager : MonoBehaviour, IStackableWindow
{
    [SerializeField] private List<WeaponDamageSummaryCard> damageSummaryCards;
    [Space]
    [BoxGroup("Materials")] [SerializeField] private Material materialDefeatBackground;
    [Space]
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelExpGain;
    [BoxGroup("Labels")] [SerializeField] private TextMeshProUGUI labelLevelUp;
    [Space]
    [BoxGroup("Images")] [SerializeField] private Image imageBackground;
    [BoxGroup("Images")] [SerializeField] private Image imageSlash;
    [BoxGroup("Images")] [SerializeField] private Image imageCharacterCard;
    [BoxGroup("Images")] [SerializeField] private Image imageGlowStripLeft;
    [BoxGroup("Images")] [SerializeField] private Image imageGlowStripRight;
    [Space]
    [BoxGroup("Stat Entries")] [SerializeField] private GameStatsCard statTimeSpendInGame;
    [BoxGroup("Stat Entries")] [SerializeField] private GameStatsCard statGoldEarned;
    [BoxGroup("Stat Entries")] [SerializeField] private GameStatsCard statGemEarned;
    [BoxGroup("Stat Entries")] [SerializeField] private GameStatsCard statKillCount;
    [Space]
    [BoxGroup("Progress Bar")] [SerializeField] private SliderBarComponent barExperience;
    
    private bool _canClose;

    public void Open()
    {
        _canClose = false;
        GameResultData.FinalizeGameResult();
        
        var stage = GameData.GetCurrentStage();
        imageBackground.sprite = stage.backgroundBlur;
        imageBackground.material = GameResultData.IsWin ? null : materialDefeatBackground;
        imageCharacterCard.material = GameResultData.IsWin ? null : materialDefeatBackground;
        imageSlash.gameObject.SetActive(!GameResultData.IsWin);
        imageGlowStripLeft.gameObject.SetActive(GameResultData.IsWin);
        imageGlowStripRight.gameObject.SetActive(GameResultData.IsWin);
        
        var currentCharacter = GameData.GetPlayerCharacterData();
        imageCharacterCard.sprite = currentCharacter.CharacterCard;
        imageGlowStripLeft.color = currentCharacter.ColorTheme;
        imageGlowStripRight.color = currentCharacter.ColorTheme;
        barExperience.SetColor(currentCharacter.ColorTheme);
        SetupDamageCards(currentCharacter.ColorTheme);

        var saveFile = SaveManager.instance.GetSaveFile();
        saveFile.AddGameResultData();
        
        labelExpGain.text = $"+{GameResultData.CharacterExp} EXP";
        var characterSaveData = saveFile.GetCharacterSaveData(currentCharacter.Id);
        var previousLevel = characterSaveData.Level;
        characterSaveData.AddGameResultStats();
        labelLevelUp.gameObject.SetActive(previousLevel < characterSaveData.Level);
        barExperience.SetValue(characterSaveData.Experience, characterSaveData.ExperienceNeeded);
        
        statTimeSpendInGame.Set(Utilities.FloatToTimeString(GameResultData.Time));
        statGoldEarned.Set(GameResultData.Gold);
        statGemEarned.Set(GameResultData.Gems);
        statKillCount.Set(GameResultData.MonstersKilled);

        
        SaveManager.instance.SaveGame();
        AchievementManager.instance.ClearPerGameStats();
        GameResultData.Reset();
        
        StackableWindowManager.instance.OpenWindow(this);
    }

    public void OnOpened()
    {
        _canClose = true;
    }

    public void Close()
    {
        if (!_canClose) return;
        
        StackableWindowManager.instance.CloseWindow(this);
    }

    private void Update()
    {
        if (!IsInFocus) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    private void SetupDamageCards(Color characterTheme)
    {
        var itemDamageOrdered = GameResultData.ItemDamage
            .OrderByDescending(x => x.Value)
            .Select(x => new { Item = x.Key, Damage = x.Value })
            .ToList();
        var maxDamage = itemDamageOrdered.FirstOrDefault()?.Damage;
        foreach (var damageSummaryCard in damageSummaryCards)
        {
            var damageEntry = itemDamageOrdered.FirstOrDefault();
            if (damageEntry != null)
            {
                itemDamageOrdered.Remove(damageEntry);
                damageSummaryCard.Setup(damageEntry.Damage, maxDamage.GetValueOrDefault(), damageEntry.Item.Icon, damageEntry.Item.Name, characterTheme);
                damageSummaryCard.gameObject.SetActive(true);
                continue;
            }
            
            damageSummaryCard.gameObject.SetActive(false);
        }
    }

    public bool IsInFocus { get; set; }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
