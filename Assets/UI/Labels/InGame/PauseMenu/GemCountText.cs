using Objects.Stage;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame.PauseMenu
{
	public class GemCountText : MonoBehaviour
	{
		[SerializeField] private GameResultData gameResultData;
		private TextMeshProUGUI text;

		private void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
		}

		public void Update()
		{
			if (Time.frameCount % 10 != 0)
				return;
			
			text.text = $"{gameResultData.Gems}";
		}
	}
}