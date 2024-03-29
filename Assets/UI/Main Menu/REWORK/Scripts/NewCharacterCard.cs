using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NewCharacterCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Material grayScaleMaterial;
    
    public void Setup(bool isUnlocked, Sprite image)
    {
        cardImage.sprite = image;
        cardImage.material = isUnlocked ? null: grayScaleMaterial;
        const float colorRatio = 80f / 255f;
        cardImage.color = new Color(colorRatio, colorRatio, colorRatio);
        lockImage.gameObject.SetActive(!isUnlocked);
    }
}
