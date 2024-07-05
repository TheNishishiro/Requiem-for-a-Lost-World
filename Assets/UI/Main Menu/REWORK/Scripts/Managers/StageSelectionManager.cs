using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data.Stages;
using Interfaces;
using Managers;
using Objects.Stage;
using TMPro;
using UI.Main_Menu.REWORK.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectionManager : MonoBehaviour, IStackableWindow
{
    [SerializeField] private CharacterSelectionScreenManager characterSelectionScreenManager;
    [SerializeField] private StageContainer stageContainer;
    [SerializeField] private List<StageCard> stageCards;
    [SerializeField] private Image imageStageBanner;
    [SerializeField] private Image imageBackground;
    [SerializeField] private TextMeshProUGUI labelStageTitle;
    [SerializeField] private Animator animatorChangeStage;
    private bool IsLockedByAnimation => !animatorChangeStage.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    
    private int _selectedIndex;
    private const float KeyHoldDelay = 0.4f;
    private float _keyNextActionTime = 0f;

    private void Update()
    {
        if (!IsInFocus) return;

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            _keyNextActionTime = 0;

        if (IsLockedByAnimation)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Close();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OpenCharacterSelection();
            return;
        }

        if (Time.time >= _keyNextActionTime)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                NextStage();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                PreviousStage();
            }
        }
    }

    public void NextStage()
    {
        _selectedIndex++;
        if (_selectedIndex >= stageContainer.Count())
        {
            _selectedIndex = 0;
        }

        AudioManager.instance.PlayButtonClick();
        _keyNextActionTime = Time.time + KeyHoldDelay;
        animatorChangeStage.SetTrigger("ChangeBackward");
    }

    public void PreviousStage()
    {
        _selectedIndex--;
        if (_selectedIndex < 0)
        {
            _selectedIndex = stageContainer.Count() - 1;
        }

        AudioManager.instance.PlayButtonClick();
        _keyNextActionTime = Time.time + KeyHoldDelay;
        animatorChangeStage.SetTrigger("ChangeForward");
    }

    public void UpdateListDisplay()
    {
        var cardIndex = -1;
        var cardsOnTheSideCount = stageCards.Count / 2;
        for (var i = _selectedIndex - cardsOnTheSideCount; i <= _selectedIndex + cardsOnTheSideCount; i++)
        {
            cardIndex++;
            var stageIndex = ((i % stageContainer.Count()) + stageContainer.Count()) % stageContainer.Count();

            if (stageIndex == _selectedIndex)
            {
                if (stageContainer.Count() > 0)
                {
                    var stageData = stageContainer.GetData(stageIndex);
                    imageStageBanner.sprite = stageData.background;
                    imageBackground.sprite = stageData.background;
                    labelStageTitle.text = stageData.title;
                }
            }

            if (stageCards.Count > cardIndex)
                stageCards[cardIndex].Setup(stageContainer.GetData(stageIndex));
        }
    }
    
    public void Open()
    {
        UpdateListDisplay();
        StackableWindowManager.instance.OpenWindow(this);
    }

    public void Close()
    {
        if (IsLockedByAnimation) return;
        
        StackableWindowManager.instance.CloseWindow(this);
    }

    public void OpenCharacterSelection()
    {
        if (IsLockedByAnimation) 
            return;
        
        GameData.SetCurrentStage(stageContainer.GetData(_selectedIndex));
        characterSelectionScreenManager.Open(false);
    }

    public bool IsInFocus { get; set; }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
