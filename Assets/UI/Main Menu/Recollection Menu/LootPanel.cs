using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using TMPro;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.Recollection_Menu
{
	public class LootPanel : MonoBehaviour
	{
		[SerializeField] private CharacterListMenu characterListMenu;
		[SerializeField] private CardComponent cardComponent;
		[SerializeField] private int PullCost = 200;
		[SerializeField] private SaveManager saveManager;
		[SerializeField] private UnityEvent<CharacterData> OnPull;
		private SaveFile _saveFile;
		
		public void Setup(SaveFile saveFile)
		{
			_saveFile = saveFile;
		}
		
		public void SinglePull()
		{
			if (gameObject.activeSelf)
			{
				return;
			}
			
			if (_saveFile.Gems < (ulong)PullCost)
			{
				return;
			}
			
			gameObject.SetActive(true);
			var pullResult = DoPull();
			var characterSaveData = _saveFile.GetCharacterSaveData(pullResult.Id);
			var pullColor = GetPullColor(characterSaveData);
			_saveFile.UnlockCharacter(pullResult);
			OnPull?.Invoke(pullResult);
			saveManager.SaveGame();

			cardComponent.SetColor(pullColor);
			cardComponent.SetCharacter(pullResult);
			cardComponent.gameObject.SetActive(true);
			
			OpenLootPanel();
		}

		public void OpenLootPanel()
		{
			gameObject.SetActive(true);
			characterListMenu.UpdateCharacterPanels();
		}
		
		public void CloseLootPanel()
		{
			cardComponent.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
		
		public CharacterData DoPull()
		{
			_saveFile.Gems -= (ulong)PullCost;
			
			var characters = CharacterListManager.instance.GetCharacters().Where(x => x.IsPullable).ToList();
			if (_saveFile.Pity < 10)
			{
				_saveFile.Pity++;
				var character = characters.OrderBy(x => Random.value).FirstOrDefault();
				var characterSaveData = _saveFile.GetCharacterSaveData(character.Id);
				if (!characterSaveData.IsUnlocked)
					_saveFile.Pity = 0;

				return character;
			}

			var unlockedCharacters = _saveFile.CharacterSaveData.Where(x => x.Value?.IsUnlocked == true).Select(x => x.Key).ToList();
			var lockedCharacters = characters.Where(x => !unlockedCharacters.Contains(x.Id)).ToList();
			if (!lockedCharacters.Any())
			{
				return characters.OrderBy(x => Random.value).FirstOrDefault();
			}

			_saveFile.Pity = 0;
			return lockedCharacters.OrderBy(x => Random.value).FirstOrDefault();
		}

		public Color GetPullColor(CharacterSaveData characterSaveData)
		{
			if (!characterSaveData.IsUnlocked)
				return new Color(190/255.0f, 140/255.0f, 0);
			if (characterSaveData.GetRankEnum() < CharacterRank.E5)
				return new Color(190/255.0f, 0, 190/255.0f);
			return new Color(0, 110/255.0f, 190/255.0f);
		}
	}
}