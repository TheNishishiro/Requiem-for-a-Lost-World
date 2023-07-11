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
		[SerializeField] private GameObject characterInfoPanelPrefab;
		[SerializeField] private RankDisplayPanel rankDisplayPanel;
		[SerializeField] private ScrollRect scrollRect;
		[SerializeField] private LorePanelManager lorePanelManager;
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

			foreach (var characterInfoPanel in FindObjectsOfType<CharacterInfoPanel>())
			{
				characterInfoPanel.gameObject.SetActive(false);
			}
		}

		private void LoadCharacters()
		{
			_characterListPanels = new List<CharacterListPanel>();
			foreach (var character in CharacterListManager.instance.GetCharacters())
			{
				var listPanel = Instantiate(cardPrefab, transform);
				var infoPanel = Instantiate(characterInfoPanelPrefab, transform);
				var infoPanelComponent = infoPanel.GetComponent<CharacterInfoPanel>();
				infoPanelComponent.SetRankDisplayPanelReference(rankDisplayPanel, lorePanelManager);
				infoPanel.SetActive(false);
					
                var listPanelComponent = listPanel.GetComponent<CharacterListPanel>();
                listPanelComponent.Setup(character, infoPanelComponent, scrollRect);
				_characterListPanels.Add(listPanelComponent);
			}
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				var activeIndex = _characterListPanels.FindIndex(x => x.characterData.IsActive);
				while (activeIndex-- > 0)
				{
					var _characterSaveData = _saveFile.GetCharacterSaveData(_characterListPanels[activeIndex].characterData.Id);
					if (_characterSaveData.IsUnlocked)
					{
						_characterListPanels[activeIndex].OnClick();
						break;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				var activeIndex = _characterListPanels.FindIndex(x => x.characterData.IsActive);
				while (activeIndex++ < _characterListPanels.Count-1)
				{
					var _characterSaveData = _saveFile.GetCharacterSaveData(_characterListPanels[activeIndex].characterData.Id);
					if (_characterSaveData.IsUnlocked)
					{
						_characterListPanels[activeIndex].OnClick();
						break;
					}
				}
			}
		}
	}
}