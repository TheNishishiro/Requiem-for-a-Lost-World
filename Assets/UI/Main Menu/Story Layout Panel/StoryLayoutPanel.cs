using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using NUnit.Framework;
using UnityEngine;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryLayoutPanel : MonoBehaviour
	{
		[SerializeField] private List<StoryTile> storyTiles = new();
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				gameObject.SetActive(false);
			}
		}

		private void OnEnable()
		{
			foreach (var storyTile in storyTiles)
			{
				storyTile.Refresh();
			}
		}
	}
}