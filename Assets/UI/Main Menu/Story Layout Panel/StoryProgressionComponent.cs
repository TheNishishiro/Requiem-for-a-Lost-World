using System;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryProgressionComponent : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _storyPointsText;

		public void Refresh(ulong currentPoints, ulong pointRequired)
		{
			_storyPointsText.text = $"{currentPoints}/{pointRequired}";
		}
	}
}