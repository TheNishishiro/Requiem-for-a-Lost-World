using Objects.Stage;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame
{
	public class LabelGoldCount : MonoBehaviour
	{
		private TextMeshProUGUI text;

		private void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
		}

		public void Update()
		{
			if (Time.frameCount % 60 == 0)
				text.text = $"{GameResultData.Gold}";
		}
	}
}