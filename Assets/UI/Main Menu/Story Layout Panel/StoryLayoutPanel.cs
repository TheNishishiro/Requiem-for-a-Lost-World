using System;
using UnityEngine;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryLayoutPanel : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				gameObject.SetActive(false);
			}
		}
	}
}