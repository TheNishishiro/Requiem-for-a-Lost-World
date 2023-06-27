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
		[SerializeField] private GachaDisplayPanel characterDisplayPanel;
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
			_saveFile.UnlockCharacter(pullResult);
			OnPull?.Invoke(pullResult);
			saveManager.SaveGame();
			characterDisplayPanel.Clear();
			characterDisplayPanel.Setup(pullResult);
			OpenLootPanel();
		}

		public void OpenLootPanel()
		{
			gameObject.SetActive(true);
			characterListMenu.UpdateCharacterPanels();
		}
		
		public void CloseLootPanel()
		{
			characterDisplayPanel.Clear();
			gameObject.SetActive(false);
		}
		
		public CharacterData DoPull()
		{
			_saveFile.Gems -= (ulong)PullCost;
			return CharacterListManager.instance.GetCharacters().Where(x => x.IsPullable).OrderBy(x => Random.value).FirstOrDefault();
		}
	}
}