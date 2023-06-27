using System;
using DefaultNamespace.Data.Achievements;
using TMPro;
using UnityEngine;

namespace UI.Shared
{
	public class AchievementGetDisplay : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI achievementText;
		[SerializeField] private GameObject achievementGetPanel;
		private float _displayTime;
		
		public void Display(AchievementEnum achievement)
		{
			_displayTime = 5f;
			achievementGetPanel.SetActive(true);
			achievementText.text = achievement.GetDescription();
		}

		private void Update()
		{
			if (_displayTime > 0)
			{
				_displayTime -= Time.deltaTime;
				return;
			}
			
			Hide();
		}
		
		private void Hide()
		{
			achievementGetPanel.SetActive(false);
		}
	}
}