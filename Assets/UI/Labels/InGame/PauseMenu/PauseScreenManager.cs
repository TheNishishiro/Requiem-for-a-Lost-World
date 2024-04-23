using System.Globalization;
using Interfaces;
using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Main_Menu.Character_List_Menu;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame.PauseMenu
{
	public class PauseScreenManager : MonoBehaviour, IQueueableWindow
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private Image characterArt;
		[SerializeField] private Image themeLine;
		[SerializeField] private CharacterStatsEntry statEntryHealth;
		[SerializeField] private CharacterStatsEntry statEntryDamage;
		[SerializeField] private CharacterStatsEntry statEntryCooldown;
		[SerializeField] private CharacterStatsEntry statEntryCrit;
		[SerializeField] private CharacterStatsEntry statEntryDot;
		[SerializeField] private CharacterStatsEntry statEntryRegen;
		[SerializeField] private CharacterStatsEntry statEntryArmor;
		[SerializeField] private CharacterStatsEntry statEntryLuck;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				ToggleMenu();
		}

		public void ToggleMenu()
		{
			if (!QueueableWindowManager.instance.IsQueueEmpty() && panel.activeInHierarchy)
			{
				CloseMenu();
				return;
			}

			if (!QueueableWindowManager.instance.IsQueueEmpty()) return;
			
			if (!panel.activeInHierarchy)
				OpenMenu();
			else
				CloseMenu();
		}

		public void CloseMenu()
		{
			QueueableWindowManager.instance.DeQueueWindow();
		}
		
		public void OpenMenu()
		{
			QueueableWindowManager.instance.QueueWindow(this);
		}

		public void Open()
		{
			characterArt.sprite = GameData.GetPlayerCharacterData().GatchaArt;
			themeLine.color = GameData.GetPlayerCharacterData().ColorTheme;
			
			var statsScaler = PlayerStatsScaler.GetScaler();
			statEntryHealth.Set(statsScaler.GetMaxHealth().ToString(CultureInfo.InvariantCulture));
			statEntryArmor.Set($"{statsScaler.GetArmor()*100:N0}%");
			statEntryDamage.Set($"{statsScaler.GetDamage()} | {(statsScaler.GetDamageIncreasePercentage() - 1)*100:N0}%");
			statEntryCooldown.Set($"{statsScaler.GetCooldownReduction()*100:F}s | {(1 - statsScaler.GetCooldownReductionPercentage())*100:N0}%");
			statEntryCrit.Set($"\u2684 {statsScaler.GetCritRate()*100:N0}% | \u2694 {(statsScaler.GetCritDamage() - 1)*100:N0}%");
			statEntryDot.Set($"\u2694 {statsScaler.GetDamageOverTime():N0} | \u23f2 {(1 -statsScaler.GetDamageOverTimeDurationIncreasePercentage())*100:N0}% | \u23f6 {(1 - statsScaler.GetDamageOverTimeFrequencyReductionPercentage())*100:N0}%");
			statEntryLuck.Set($"{statsScaler.GetLuck()*100:N0}%");
			statEntryRegen.Set($"{statsScaler.GetHealthRegeneration()} /s");
			
			panel.SetActive(true);
		}

		public void Close()
		{
			panel.SetActive(false);
		}
	}
}