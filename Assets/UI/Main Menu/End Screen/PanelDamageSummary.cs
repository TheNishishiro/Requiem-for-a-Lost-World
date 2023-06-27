using Managers;
using TMPro;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.End_Screen
{
	public class PanelDamageSummary : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI damageText;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private CharacterExpBar damageBar;

		public void Setup(DamageSummaryEntry damageSummaryEntry)
		{
			icon.sprite = damageSummaryEntry.Icon;
			nameText.text = damageSummaryEntry.Name;
			damageText.text = damageSummaryEntry.GetShortDamageFormatted();
			
			damageBar.SetValue(damageSummaryEntry.RawDamage, damageSummaryEntry.MaxDamage);
			damageBar.SetColor(CharacterListManager.instance.GetActiveCharacter().ColorTheme);
		}
	}
}