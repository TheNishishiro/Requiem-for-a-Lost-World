using DefaultNamespace.Data.Statuses;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private SVGImage svgImage;
    [HideInInspector] public StatusEffectType statusEffectType;

    public void Show(Sprite sprite, StatusEffectType statusEffect, int stackCount)
    {
        svgImage.sprite = sprite;
        statusEffectType = statusEffect;
        label.text = stackCount.ToString();
        label.gameObject.SetActive(stackCount != 0);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
