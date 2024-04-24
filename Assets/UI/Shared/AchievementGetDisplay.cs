using System;
using DefaultNamespace.Data.Achievements;
using TMPro;
using UnityEngine;

namespace UI.Shared
{
	public class AchievementGetDisplay : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI achievementText;
		[SerializeField] private Animator animator;
		private float _displayTime;
		
		public void Display(AchievementEnum achievement)
		{
			_displayTime = 5f;
			gameObject.SetActive(true);
			achievementText.text = achievement.GetTitle();
		}

		private void Update()
		{
			if (_displayTime > 0)
			{
				_displayTime -= Time.deltaTime;
				return;
			}
			
			animator.SetTrigger("Hide");
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}