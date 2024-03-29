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
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image imageShard;
    [SerializeField] private Image imageShardOverlay;
    [SerializeField] public int shardRank;
    [SerializeField] public ShardScreenManager shardScreenManager;
    [SerializeField] private Material materialDisabled;
    private CharacterData _currentCharacter;
    [HideInInspector] public EidolonData currentShard;
    public float maxTiltAngle = 15f;
    public float idleMoveMagnitude = 10f;    // The magnitude of the idle movement
    public float idleMoveSpeed = 1f;         // The speed of the idle movement
    public float idleRotateSpeed = 30f;      // The speed of the idle rotation
    private Vector3 _idlePositionOffset;      // The offset for the idle position
    private Quaternion _idleRotationOffset; 
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
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

    private void Awake()
    {
        _originalPosition = rectTransform.anchoredPosition;
        _originalRotation = rectTransform.localRotation;
        idleMoveSpeed += GetInstanceID() % 10 / 10.0f;
        idleRotateSpeed += GetInstanceID() % 20 / 10.0f;
    }

    public void Set(CharacterData characterData)
    {
        _currentCharacter = characterData;
        currentShard = characterData.Eidolons[shardRank - 1];
        imageShard.sprite = currentShard.EidolonTexture;
    }
    
    private void Update()
    {
        rectTransform.localRotation = _originalRotation * _idleRotationOffset;
        rectTransform.anchoredPosition3D = _originalPosition + _idlePositionOffset;
        
        _idlePositionOffset = new Vector3
        (
            Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
            Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
            0f
        );

        _idleRotationOffset = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * idleRotateSpeed, maxTiltAngle) - maxTiltAngle / 2);
    }

    public void Select()
    {
        shardScreenManager.MarkSelected(shardRank);
    }
}
