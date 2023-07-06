using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Data;
using Objects.Characters;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;

namespace Managers
{
	public class SaveManager : MonoBehaviour
	{
		private static bool _isFirstLoad = true;
		[SerializeField] private CharacterListMenu characterListMenu;
		private SaveFile _saveData;

		private void Awake()
		{
			_saveData = FindObjectOfType<SaveFile>();
			Time.timeScale = 1f;
			if (_isFirstLoad)
			{
				_isFirstLoad = false;
				LoadGame();
			}
		}

		public void SaveGame()
		{
			_saveData.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			_saveData.UpdateMissingAchievementEntries();
			_saveData.Save();
		}

		public void LoadGame()
		{
			_saveData.Initialize();
			_saveData.Load();
			_saveData.UpdateMissingCharacterEntries(CharacterListManager.instance.GetCharacters());
			_saveData.UpdateMissingAchievementEntries();
			if (!_saveData.CharacterSaveData[CharactersEnum.Chitose].IsUnlocked)
				_saveData.CharacterSaveData[CharactersEnum.Chitose].Unlock();
			characterListMenu.UpdateCharacterPanels();
		}
	}
}