using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame
{
	public class LabelTime : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;

		public void UpdateTime(float time)
		{
			text.text = Utilities.FloatToTimeString(time);
		}
	}
}