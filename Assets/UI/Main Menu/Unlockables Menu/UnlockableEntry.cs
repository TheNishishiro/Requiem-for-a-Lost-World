using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class UnlockableEntry : MonoBehaviour
{
    [SerializeField] private Image icon;
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
        icon.color = _item.IsUnlocked(_saveFile) ? Color.white : Color.black;
    }
}
