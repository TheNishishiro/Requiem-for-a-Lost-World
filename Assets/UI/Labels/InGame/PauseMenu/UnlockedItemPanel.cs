using Data.Elements;
using Interfaces;
using Objects.Items;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UI.Main_Menu.Character_List_Menu;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Weapons;

namespace UI.Labels.InGame.PauseMenu
{
	public class UnlockedItemPanel : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshProUGUI upgradeLevel;
		[SerializeField] private Image icon;
		[SerializeField] private SVGImage elementIcon;
		[SerializeField] private ElementIconData elementIconData;
		private StatsScrollMenuPanel _statsScrollMenuPanel;
		private IPlayerItem _playerItem;
		
		public void SetUpgradeEntry(IPlayerItem playerItem, StatsScrollMenuPanel statsScrollMenuPanel)
		{
			_statsScrollMenuPanel = statsScrollMenuPanel;
			_playerItem = playerItem;
			upgradeLevel.text = playerItem.LevelField.ToString();
			icon.sprite = playerItem.IconField;

			if (playerItem.ElementField != Element.Disabled)
			{
				elementIcon.gameObject.SetActive(true);
				elementIcon.sprite = elementIconData.GetIcon(playerItem.ElementField);
				elementIcon.color = ElementService.ElementToColor(playerItem.ElementField);
			}
			else
			{
				elementIcon.gameObject.SetActive(false);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_statsScrollMenuPanel.ClearEntries();
			_statsScrollMenuPanel.SetTitle(_playerItem.NameField);
			foreach (var statsDisplayData in _playerItem.GetStatsData())
			{
				_statsScrollMenuPanel.AddEntry(statsDisplayData.Name, statsDisplayData.Value, statsDisplayData.Description);
			}
		}
	}
}