using Interfaces;
using Objects.Items;
using TMPro;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI.Labels.InGame.PauseMenu
{
	public class UnlockedItemPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI upgradeLevel;
		[SerializeField] private Image icon;
		
		public void SetUpgradeEntry(IPlayerItem playerItem)
		{
			upgradeLevel.text = playerItem.LevelField.ToString();
			icon.sprite = playerItem.IconField;
		}
	}
}