using Objects.Stage;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.End_Screen
{
	public class PanelStatsSummary : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;

		public void Setup(string displayText)
		{
			text.text = displayText;
		}
	}
}