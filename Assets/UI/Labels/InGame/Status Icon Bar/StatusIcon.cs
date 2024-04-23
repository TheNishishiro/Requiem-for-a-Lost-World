using DefaultNamespace;
using DefaultNamespace.Data.Statuses;
using TMPro;
using UI.Labels.InGame.Status_Icon_Bar;
using Unity.VectorGraphics;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private SVGImage svgImage;
    [SerializeField] private Material materialPositive;
    [SerializeField] private Material materialNegative;
    [HideInInspector] public StatusEffectType statusEffectType;

    public void Show(StatusIconPair statusIconPair, StatusEffectType statusEffect, int stackCount)
    {
        svgImage.sprite = statusIconPair.icon;
        svgImage.color = statusIconPair.isNegative ? Utilities.HexToColor("FF3D3D") : Utilities.HexToColor("3CC839");
        statusEffectType = statusEffect;
        label.material = statusIconPair.isNegative ? materialNegative : materialPositive;
        label.text = stackCount.ToString();
        label.gameObject.SetActive(stackCount != 0);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
