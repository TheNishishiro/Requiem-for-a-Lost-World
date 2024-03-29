using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Main_Menu.REWORK.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image imageTopLine;
    [SerializeField] private Image imageBottomLine;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private Button button;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material idleMaterial;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        MainMenuManager.instance.SetSelected(GetInstanceID());
    }

    public void SetFocused(bool isFocused)
    {
        imageTopLine.gameObject.SetActive(isFocused);
        imageBottomLine.gameObject.SetActive(isFocused);
        labelText.fontSharedMaterial = isFocused ? hoverMaterial : idleMaterial;
    }

    public void Execute()
    {
        button.onClick.Invoke();
    }
}
