using DefaultNamespace;
using TMPro;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class WeaponDamageSummaryCard : MonoBehaviour
    {
        [SerializeField] private CharacterExpBar barDamage;
        [SerializeField] private Image imageWeaponIcon;
        [SerializeField] private TextMeshProUGUI textWeaponName;
        [SerializeField] private TextMeshProUGUI textWeaponDamage;

        public void Setup(float damageDealt, float highestDamage, Sprite icon, string weaponName, Color themeColor)
        {
            barDamage.SetValue(damageDealt, highestDamage);
            barDamage.SetColor(themeColor);
            imageWeaponIcon.sprite = icon;
            textWeaponName.text = weaponName;
            textWeaponDamage.text = Utilities.GetShortNumberFormatted(damageDealt);
        }
    }
}