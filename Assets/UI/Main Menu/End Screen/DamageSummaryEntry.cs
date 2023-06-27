using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.End_Screen
{
	public class DamageSummaryEntry
	{
		public Sprite Icon;
		public float RawDamage;
		public float MaxDamage;
		public string Name;
		
		public string GetShortDamageFormatted()
		{
			return Utilities.GetShortNumberFormatted(RawDamage);
		}
	}
}