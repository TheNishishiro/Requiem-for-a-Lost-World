using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class CharacterListMenu : MonoBehaviour
	{
		[SerializeField] private GameObject cardPrefab;
		[SerializeField] private CharacterInfoPanel characterInfoPanel;
		private SaveFile _saveFile;
		private List<CharacterListPanel> _characterListPanels;

		private void Start()
		{
			_saveFile = FindObjectOfType<SaveFile>();
			LoadCharacters();
		}

		public void UpdateCharacterPanels()
		{
			if (_characterListPanels == null) return;
			
			foreach (var characterListPanel in _characterListPanels)
			{
				characterListPanel.UpdateDisplayInfo();
			}
		}

		private void LoadCharacters()
		{
			_characterListPanels = new List<CharacterListPanel>();
			var rectTransform = GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(CharacterListManager.instance.GetCharactersCount() * 350, rectTransform.sizeDelta.y);
			
			foreach (var character in CharacterListManager.instance.GetCharacters())
			{
				var listPanel = Instantiate(cardPrefab, transform).GetComponent<CharacterListPanel>().Setup(character);
				_characterListPanels.Add(listPanel);
			}
		}

		public void ActivateCharacter(CharacterData characterData)
		{
			foreach (var character in CharacterListManager.instance.GetCharacters())
			{
				character.Deactivate();
			}
			
			characterData.Activate();
			characterInfoPanel.gameObject.SetActive(true);
			characterInfoPanel.SetCharacterData(characterData, _saveFile.GetCharacterSaveData(characterData.Id));
		}
	}
}