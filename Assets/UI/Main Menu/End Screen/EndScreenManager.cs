using System;
using Data.Difficulty;
using DefaultNamespace.Data;
using Managers;
using Objects.Stage;
using TMPro;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.End_Screen
{
	public class EndScreenManager : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private DamageSummaryContainer damageSummaryContainer;
		[SerializeField] private StatsSummaryContainer statsSummaryContainer;
		[SerializeField] private CharacterListMenu characterListMenu;
		[SerializeField] private TextMeshProUGUI winLoseText;
		[SerializeField] private Image characterImage;
		[SerializeField] private SaveManager saveManager;
		[SerializeField] private AchievementManager achievementManager;
		[SerializeField] private TitleScreen.TitleScreen titleScreen;
		private SaveFile _saveFile;
		
		private void Start()
		{
			var discordManager = FindObjectOfType<DiscordManager>();
			_saveFile = FindObjectOfType<SaveFile>();
			if (GameResultData.IsGameEnd)
			{
				//titleScreen.gameObject.SetActive(false);
				winLoseText.text = GameResultData.IsWin ? "Victory" : "Defeat";
				panel.SetActive(true);
				
				GameResultData.FinalizeGameResult();
				damageSummaryContainer.Setup();
				statsSummaryContainer.Setup();

				var activeCharacter = CharacterListManager.instance.GetActiveCharacter();
				
				var characterSaveData = _saveFile.GetCharacterSaveData(activeCharacter.Id);
				characterSaveData.AddGameResultStats();
				
				_saveFile.AddGameResultData();
				
				characterListMenu.UpdateCharacterPanels();
				GameResultData.IsGameEnd = false;
				saveManager.SaveGame();
				achievementManager.ClearPerGameStats();
				discordManager.SetEndMenu(GameResultData.IsWin);
			}
			else
			{
				//titleScreen.gameObject.SetActive(true);
				discordManager.SetMainMenu();
			}
            
			GameResultData.Reset();
		}
		
		public void Close()
		{
			statsSummaryContainer.Clear();
			damageSummaryContainer.Clear();
			panel.SetActive(false);
		}
	}
}