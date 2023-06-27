using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using StarterAssets;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
	public class UpgradePanelManager : MonoBehaviour
	{
		[SerializeField] private GameObject panel;
		[SerializeField] private List<UpgradePanel> upgradeButtons;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private CursorManager cursorManager;

		private void Start()
		{
			HideButtons();
		}

		public void OpenPanel()
		{
			var upgradeEntries = weaponManager.GetUpgrades().OrderBy(x => Random.value).Take(Random.Range(3, 4)).ToList();
			if (upgradeEntries.Count == 0)
				return;
			
			cursorManager.ShowCursor();
			pauseManager.PauseGame();
			Clean();
			panel.SetActive(true);
			
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradeButtons[i].gameObject.SetActive(true);
				upgradeButtons[i].Set(upgradeEntries[i]);
			}
		}

		public void ClosePanel()
		{
			cursorManager.HideCursor();
			HideButtons();
			pauseManager.UnPauseGame();
			panel.SetActive(false);
		}

		public void Clean()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.Clean();
		}

		public void HideButtons()
		{
			foreach (var upgradeButton in upgradeButtons)
				upgradeButton.gameObject.SetActive(false);
		}
	}
}