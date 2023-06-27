using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame
{
	public class LabelTime : MonoBehaviour
	{
		private TextMeshProUGUI text;

		private void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
		}

		public void UpdateTime(float time)
		{
			text.text = Utilities.FloatToTimeString(time);
		}
	}
}