using System;
using Managers;
using Objects.Characters;
using Objects.Stage;
using UI.In_Game.GUI.Scripts.Managers;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Objects.Players.Scripts
{
	public class LevelComponent : MonoBehaviour
	{
		private float experience = 0;
		private int level = 1;
		[SerializeField] private UpgradePanelManager _upgradePanelManager;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		private int ToLevelUp => level * 1000;

		public int GetLevel()
		{
			return level;
		}
		
		private void Start()
		{
			GuiManager.instance.SetLevelText(level);
			GuiManager.instance.UpdateExperience(experience, ToLevelUp);
		}

		public void AddExperience(float amount)
		{
			experience += amount;
			CheckLevelUp();
			GuiManager.instance.UpdateExperience(experience, ToLevelUp);
		}

		private void CheckLevelUp()
		{
			while (experience >= ToLevelUp)
			{
				LevelUp();
			}
		}

		private void LevelUp()
		{
			experience -= ToLevelUp;
			level++;
			GuiManager.instance.SetLevelText(level);
			_upgradePanelManager.OpenPanel();
			OnLevelUp();
		}

		private void OnLevelUp()
		{
			var levelUpStrategyManager = new PlayerStrategyApplier();
			levelUpStrategyManager.ApplyLevelUpStrategy(GameData.GetPlayerCharacterId(), GameData.GetPlayerCharacterRank(), level, playerStatsComponent);
			AchievementManager.instance.OnLevelUp(this);
		}
	}
}