using Managers;
using TMPro;
using UI.Main_Menu.Character_List_Menu;
using UI.Shared.Utilities;
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
		[SerializeField] private EaseInOutAnimator easeInOutAnimator;
		[SerializeField] private DimPanel dimPanel;

		public void Setup(DamageSummaryEntry damageSummaryEntry, bool isDimmed, float animationDelay)
		{
			icon.sprite = damageSummaryEntry.Icon;
			nameText.text = damageSummaryEntry.Name;
			damageText.text = damageSummaryEntry.GetShortDamageFormatted();
			
			damageBar.SetValue(damageSummaryEntry.RawDamage, damageSummaryEntry.MaxDamage);
			damageBar.SetColor(CharacterListManager.instance.GetActiveCharacter().ColorTheme);
			dimPanel.SetDim(isDimmed);
			easeInOutAnimator.ShowPanel(animationDelay);
		}
	}
}