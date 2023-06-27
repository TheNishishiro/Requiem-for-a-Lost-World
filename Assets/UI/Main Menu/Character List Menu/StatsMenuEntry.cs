using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Character_List_Menu
{
	public class StatsMenuEntry : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI statName;
		[SerializeField] private TextMeshProUGUI statValue;
		
		public void Setup(string name, string value)
		{
			statName.text = name;
			statValue.text = value;
		}
	}
}