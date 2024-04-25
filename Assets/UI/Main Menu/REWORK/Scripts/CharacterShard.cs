using System.Collections;
using System.Collections.Generic;
using Objects.Characters;
using UI.Main_Menu.REWORK.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterShard : MonoBehaviour
{
    [SerializeField] private Image imageShard;
    [SerializeField] private Image imageShardOverlay;
    [SerializeField] public int shardRank;
    [SerializeField] public ShardScreenManager shardScreenManager;
    [SerializeField] private Material materialDisabled;
    private CharacterData _currentCharacter;
    [HideInInspector] public EidolonData currentShard;
    
    private bool _isUnlocked;
    private bool _isSelected;

    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            _isUnlocked = value;
            imageShard.material = _isUnlocked ? null : materialDisabled;
            imageShard.color = _isUnlocked ? Color.white : new Color(0.2f, 0.2f, 0.2f);
            imageShardOverlay.color = _isUnlocked ? _currentCharacter.ColorTheme : Color.gray * 0.7f;
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (_isUnlocked)
            {
                imageShard.color = _isSelected ? Color.white : new Color(0.5f, 0.5f, 0.5f);
                imageShardOverlay.color = _isSelected ? _currentCharacter.ColorTheme : _currentCharacter.ColorTheme * 0.7f;
            }
            else
            {
                imageShard.color = _isSelected ?  new Color(0.5f, 0.5f, 0.5f) : new Color(0.2f, 0.2f, 0.2f);
                imageShardOverlay.color = Color.gray * 0.7f;
            }
        }
    }

    public void Set(CharacterData characterData)
    {
        _currentCharacter = characterData;
        currentShard = characterData.Eidolons[shardRank - 1];
        imageShard.sprite = currentShard.EidolonTexture;
    }

    public void Select()
    {
        shardScreenManager.MarkSelected(shardRank);
    }
}
