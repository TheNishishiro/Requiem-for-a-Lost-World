using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Interfaces;
using TMPro;
using UI.In_Game.GUI.Scripts;
using UI.Labels.InGame.LevelUpScreen;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UpgradePanel = UI.Labels.InGame.UpgradeScreen.UpgradePanel;

namespace Managers
{
	public class ChestPanelManager : MonoBehaviour, IQueueableWindow
	{
		public static ChestPanelManager instance;
		[SerializeField] private List<UpgradePanel> upgradePanels;
		[SerializeField] private List<CompactUpgradePanel> compactUpgradePanels;
		[SerializeField] private WeaponManager weaponManager;
		[SerializeField] private GameObject panel;
		[SerializeField] private TextMeshProUGUI labelDescription;
		[SerializeField] private ParticleSystem highlightParticles;
		[SerializeField] private Image flash;
		private static bool UseCompactChestMenu => SaveFile.Instance.ConfigurationFile.UseCompactChestMenu ||
		                                           (NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient && (!NetworkManager.Singleton.IsHost || (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClientsList.Count > 1)));
		
		private void Start()
		{
			if (instance == null)
				instance = this;
		}

		public void Update()
		{
			if (!panel.activeSelf)
				return;
			
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
				ClosePanel();
		}

		public void OpenPanel()
		{
			QueueableWindowManager.instance.QueueWindow(this);
		}
		
		public void ClosePanel()
		{
			QueueableWindowManager.instance.DeQueueWindow();
		}
		
		public void HideButtons()
		{
			foreach (var upgradePanel in upgradePanels)
				upgradePanel.gameObject.SetActive(false);
		}

		public void Open()
		{
			var upgradeEntries = new List<UpgradeEntry>();
			upgradeEntries.AddRange(weaponManager.GetWeaponUpgrades());
			upgradeEntries.AddRange(weaponManager.GetItemUpgrades());

			var chance = Random.value;
			var amount = chance switch
			{
				< 0.01f => 5,
				< 0.1f => 3,
				_ => 1
			};

			upgradeEntries = upgradeEntries.OrderBy(x => Random.value).Take(amount).ToList();
			if (upgradeEntries.Count == 0)
			{
				var player = GameManager.instance.playerComponent;
				player.AddGold(Random.Range(1, 50));
				player.AddGems(Random.Range(1, 10));
				ClosePanel();
				return;
			}

			panel.SetActive(!UseCompactChestMenu);

			UpgradeEntry highestRarity = null;
			for (var i = 0; i < upgradeEntries.Count; i++)
			{
				upgradeEntries[i].BoostRarity();
				if (highestRarity == null || upgradeEntries[i].Rarity > highestRarity.Rarity)
					highestRarity = upgradeEntries[i];

				if (UseCompactChestMenu)
				{
					compactUpgradePanels[i].Open(upgradeEntries[i]);
				}
				else
				{
					upgradePanels[i].gameObject.SetActive(true);
					upgradePanels[i].SetUpgradeData(upgradeEntries[i]);
				}
				upgradeEntries[i].LevelUp(weaponManager);
			}
			labelDescription.text = upgradeEntries[0].GetUnlockDescription();

			var p = highlightParticles.main;
			if (highestRarity != null)
			{
				var color = highestRarity.GetUpgradeColor();
				p.startColor = new Color(color.r, color.g, color.b, 40f/255f);
				flash.color = new Color(MathF.Max(200f / 255f, color.r), MathF.Max(200f / 255f, color.g), MathF.Max(200f / 255f, color.b));
			}
			
			if (UseCompactChestMenu)
				ClosePanel();
		}

		public void Close()
		{
			HideButtons();
			panel.SetActive(false);
		}

		public void ShowDescription(string description)
		{
			labelDescription.text = description;
		}
	}
}