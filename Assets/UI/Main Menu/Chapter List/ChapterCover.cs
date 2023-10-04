using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Chapter_List
{
	public class ChapterCover : MonoBehaviour
	{
		[SerializeField] private GameObject storyLayoutPanel;

		public void Open()
		{
			storyLayoutPanel.SetActive(true);
		}
	}
}