using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using Objects.Players;
using TMPro;
using UI.Main_Menu.Skill_Tree_Menu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class CharacterInfoPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI nameTextField;
		[SerializeField] private TextMeshProUGUI titleTextField;
		[SerializeField] private TextMeshProUGUI levelTextField;
		[SerializeField] private TextMeshProUGUI expTextField;
		[SerializeField] private TextMeshProUGUI killCountTextField;
		[SerializeField] private TextMeshProUGUI highestLevelTextField;
		[SerializeField] private CharacterExpBar expSlider;
		[SerializeField] private StarterEntry startingWeapon;
		[SerializeField] private StarterEntry startingSkill;
		[SerializeField] private GameObject startingSkillInfoPanel;
		[SerializeField] private TextMeshProUGUI passiveDescription;
		[SerializeField] private Image characterImage;
		[SerializeField] private Image separationLine1;
		[SerializeField] private Image separationLine2;
		[SerializeField] private Image separationLine3;
		[SerializeField] private Image separationLine4;
		[SerializeField] private Image separationLine5;
		[SerializeField] private Image separationLine6;
		[SerializeField] private Image separationLine7;
		[SerializeField] private Image separationLine8;
		[SerializeField] private GameObject statsPanel;
		[SerializeField] private Image statsScrollbar;
		[SerializeField] private Image passiveScrollbar;
		private RankDisplayPanel _rankDisplayPanel;
		private LorePanelManager _lorePanelManager;
		private CharacterData _characterData;
		private CharacterSaveData _characterSaveData;
        
		public void SetCharacterData(CharacterData characterData, CharacterSaveData characterSaveData)
		{
			_characterData = characterData;
			_characterSaveData = characterSaveData;
			nameTextField.text = characterData.Name;
			titleTextField.text = characterData.Title;
			levelTextField.text = characterSaveData.Level.ToString();
			expTextField.text = $"EXP ► {(characterSaveData.Experience / (float) characterSaveData.ExperienceNeeded):P}";
			expSlider.SetValue(characterSaveData.Experience, characterSaveData.ExperienceNeeded);
			separationLine1.color = separationLine2.color = separationLine3.color = separationLine4.color = separationLine5.color = separationLine6.color = 
				separationLine7.color = separationLine8.color = characterData.ColorTheme;
			passiveScrollbar.color = statsScrollbar.color = new Color(characterData.ColorTheme.r, characterData.ColorTheme.g, characterData.ColorTheme.b, 0.5f);
			killCountTextField.text = Utilities.GetShortNumberFormatted(characterSaveData.KillCount);
			highestLevelTextField.text = characterSaveData.HighestInGameLevel.ToString();
			startingWeapon.Set(characterData.StartingWeapon.Icon, characterData.StartingWeapon.Name, characterData.GetWeaponDescription(_characterSaveData.RankUpLevel));
			startingSkill.Set(characterData.AbilityIcon, characterData.AbilityName, characterData.GetAbilityDescription(_characterSaveData.RankUpLevel));
			passiveDescription.text = characterData.GetPassiveDescription(_characterSaveData.RankUpLevel);
			startingSkillInfoPanel.SetActive(false);
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			var characterStats = new PlayerStats(characterData.Stats);
			foreach (var statEntry in characterStats.GetStatsList())
			{
				statsPanelComponent.AddEntry(statEntry.Name, statEntry.Value, statEntry.Description);
			}
		}
		
		public void SetRankDisplayPanelReference(RankDisplayPanel rankDisplayPanel, LorePanelManager lorePanelManager)
		{
			_rankDisplayPanel = rankDisplayPanel;
			_lorePanelManager = lorePanelManager;
		}

		public void OpenShardsMenu()
		{
			AudioManager.instance.PlayButtonSimpleClick();
			_rankDisplayPanel.Open(_characterData, _characterSaveData.GetRankEnum());
		}

		public void OpenSkillTreeMenu()
		{
			AudioManager.instance.PlayButtonSimpleClick();
			FindFirstObjectByType<SkillTreePanel>(FindObjectsInactive.Include).Open(_characterData);
		}
		
		public void OpenStoryMenu()
		{
			_lorePanelManager.Open();
		}

		public void Close()
		{
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			gameObject.SetActive(false);
		}
	}
}