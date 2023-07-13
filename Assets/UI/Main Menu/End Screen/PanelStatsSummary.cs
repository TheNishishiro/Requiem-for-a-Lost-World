using Objects.Stage;
using TMPro;
using UI.Shared.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.End_Screen
{
	public class PanelStatsSummary : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private DimPanel dimPanel;
        [SerializeField] private EaseInOutAnimator easeInOutAnimator;

		public void Setup(string displayText, bool isDimmed, float animationDelay)
		{
			text.text = displayText;
			dimPanel.SetDim(isDimmed);
			easeInOutAnimator.ShowPanel(animationDelay);
		}
	}
}