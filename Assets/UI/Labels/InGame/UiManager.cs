using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Stage;
using UI.Labels.InGame.Minimap;
using UnityEngine;
using UnityEngine.InputSystem;
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
		[SerializeField] private GameObject uiControlsCanvas;
		
		
		public void Awake()
		{
			abilityIconImageBox.sprite = abilityIconCooldownImageBox.sprite = GameData.GetPlayerAbilityIcon();
			characterAvatarImageBox.sprite = GameData.GetPlayerCharacterAvatar();
			expBarFillImage.color = GameData.GetPlayerColorTheme() ?? Color.cyan;
			mapPanel.gameObject.SetActive(false);
			uiControlsCanvas.SetActive(Touchscreen.current != null);
		}
	}
}