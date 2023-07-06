using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using Objects.Characters;
using Objects.Players;
using TMPro;
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
		[SerializeField] private GameObject weaponIcon;
		[SerializeField] private TextMeshProUGUI weaponName;
		[SerializeField] private GameObject skillIcon;
		[SerializeField] private TextMeshProUGUI skillName;
		[SerializeField] private TextMeshProUGUI passiveDescription;
		[SerializeField] private Image characterImage;
		[SerializeField] private Image separationLine1;
		[SerializeField] private Image separationLine2;
		[SerializeField] private Image separationLine3;
		[SerializeField] private Image separationLine4;
		[SerializeField] private GameObject statsPanel;
		[SerializeField] private GameObject RankDisplayPanel;
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
			characterImage.sprite = characterData.TransparentCard;
			separationLine1.color = separationLine2.color = separationLine3.color = separationLine4.color = characterData.ColorTheme;
			killCountTextField.text = Utilities.GetShortNumberFormatted(characterSaveData.KillCount);
			highestLevelTextField.text = characterSaveData.HighestInGameLevel.ToString();
			weaponIcon.GetComponent<Image>().sprite = characterData.StartingWeapon?.Icon;
			weaponIcon.GetComponent<Tooltip>().message = characterData.StartingWeapon?.Description;
			weaponName.text = characterData.StartingWeapon?.Name;
			skillIcon.GetComponent<Image>().sprite = characterData.AbilityIcon;
			skillIcon.GetComponent<Tooltip>().message = characterData.AbilityDescription;
			skillName.text = characterData.AbilityName;
			passiveDescription.text = characterData.PassiveDescription;
			
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			var characterStats = new PlayerStats(characterData.Stats);
			foreach (var statEntry in characterStats.GetStatsList())
			{
				statsPanelComponent.AddEntry(statEntry.Name, statEntry.Value);
			}
		}

		public void OpenShardsMenu()
		{
			RankDisplayPanel.GetComponent<RankDisplayPanel>().Open(_characterData, _characterSaveData.GetRankEnum());
		}

		public void Close()
		{
			var statsPanelComponent = statsPanel.GetComponent<StatsScrollMenuPanel>();
			statsPanelComponent.ClearEntries();
			gameObject.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}
	}
}