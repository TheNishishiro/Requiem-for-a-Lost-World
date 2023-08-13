using System;
using System.Linq;
using DefaultNamespace.Data;
using UnityEngine;

namespace UI.Main_Menu.Story_Layout_Panel
{
	public class StoryLayoutPanel : MonoBehaviour
	{
		[SerializeField] private StoryProgressionComponent _storyProgressionComponent;
		private SaveFile _saveFile;

		private SaveFile saveFile
		{
			get
			{
				if (_saveFile == null)
					_saveFile = FindObjectOfType<SaveFile>();
				return _saveFile;
			}
		}

		
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
			
			_storyProgressionComponent.Refresh(saveFile.StoryPoints, storyTiles.OrderBy(x => x.requiredStoryPoints).FirstOrDefault(x=> saveFile.StoryPoints < x.requiredStoryPoints)?.requiredStoryPoints ?? 0);
			foreach (var storyTile in storyTiles)
			{
				storyTile.Refresh();
			}
		}
	}
}