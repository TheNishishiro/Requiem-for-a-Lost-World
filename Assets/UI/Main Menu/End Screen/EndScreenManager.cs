using System;
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
		[SerializeField] private GameResultData gameResultData;
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
			if (gameResultData.IsGameEnd)
			{
				discordManager.SetEndMenu(gameResultData.IsWin);
				AudioListener.volume = 1;
				titleScreen.gameObject.SetActive(false);
				winLoseText.text = gameResultData.IsWin ? "Victory" : "Defeat";
				panel.SetActive(true);
				
				gameResultData.FinalizeGameResult();
				damageSummaryContainer.Setup(gameResultData);
				statsSummaryContainer.Setup(gameResultData);

				var activeCharacter = CharacterListManager.instance.GetActiveCharacter();
				characterImage.sprite = activeCharacter.TransparentCard;
				
				var characterSaveData = _saveFile.GetCharacterSaveData(activeCharacter.Id);
				characterSaveData.AddGameResultStats(gameResultData);
				
				_saveFile.AddGameResultData(gameResultData);
				
				characterListMenu.UpdateCharacterPanels();
				gameResultData.IsGameEnd = false;
				saveManager.SaveGame();
				achievementManager.ClearPerGameStats();
			}
			else
			{
				discordManager.SetMainMenu();
				titleScreen.gameObject.SetActive(true);
			}
            
			gameResultData.Reset();
		}
		
		public void Close()
		{
			statsSummaryContainer.Clear();
			damageSummaryContainer.Clear();
			panel.SetActive(false);
		}
	}
}