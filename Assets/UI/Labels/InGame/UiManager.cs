using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Stage;
using UI.Labels.InGame.Minimap;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class UiManager : MonoBehaviour
	{
		[SerializeField] private Image abilityIconImageBox;
		[SerializeField] private Image abilityIconCooldownImageBox;
		[SerializeField] private Image characterAvatarImageBox;
		[SerializeField] private Image expBarFillImage;
		[SerializeField] private Image pauseMenuCharacterImage;
		[SerializeField] private MinimapComponent mapPanel;
		
		public void Awake()
		{
			abilityIconImageBox.sprite = abilityIconCooldownImageBox.sprite = GameData.GetPlayerAbilityIcon();
			characterAvatarImageBox.sprite = GameData.GetPlayerCharacterAvatar();
			expBarFillImage.color = GameData.GetPlayerColorTheme() ?? Color.cyan;
			pauseMenuCharacterImage.sprite = GameData.GetPlayerCharacterArt();
			mapPanel.gameObject.SetActive(false);
		}
	}
}