using System;
using System.Linq;
using DefaultNamespace.Data;
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

		private void OnEnable()
		{
			var storyTiles = FindObjectsOfType<StoryTile>();
			
			foreach (var storyTile in storyTiles)
			{
				storyTile.Refresh();
			}
		}
	}
}