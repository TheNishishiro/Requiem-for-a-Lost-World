using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockableEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject lockIcon;
    private TextMeshProUGUI _displayText;
    private IPlayerItem _item;
    private SaveFile _saveFile;

    public void SetItem(IPlayerItem item, SaveFile saveFile)
    {
        _item = item;
        _saveFile = saveFile;
        icon.sprite = _item.IconField;
        UpdateLock();
    }

    public void UpdateLock()
    {
        var isItemUnlocked = _item.IsUnlocked(_saveFile);
        
        lockIcon.SetActive(!isItemUnlocked);
        icon.color = isItemUnlocked ? Color.white : Color.black;
    }
    
    public void SetDisplayText(TextMeshProUGUI text)
    {
        _displayText = text;
    } 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item.IsUnlocked(_saveFile))
            _displayText.text = _item.NameField + "<size=-15>\n" + _item.GetDescription(1) + "</size>";
        else
            _displayText.text = "???";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _displayText.text = null;
    }
}
