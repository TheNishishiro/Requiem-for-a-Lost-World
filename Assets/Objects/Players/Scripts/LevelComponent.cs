using System;
using Managers;
using Objects.Characters;
using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Rendering;

namespace Objects.Players.Scripts
{
	public class LevelComponent : MonoBehaviour
	{
		private int experience = 0;
		private int level = 1;
		[SerializeField] private UpgradePanelManager _upgradePanelManager;
		[SerializeField] private ExperienceBar experienceBar;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		private int ToLevelUp => level * 1000;

		public int GetLevel()
		{
			return level;
		}
		
		private void Awake()
		{
			experienceBar.SetLevelText(level);
			experienceBar.UpdateSlider(experience, ToLevelUp);
		}

		public void AddExperience(int amount)
		{
			experience += (int)(amount * playerStatsComponent.GetExperienceIncrease());
			CheckLevelUp();
			experienceBar.UpdateSlider(experience, ToLevelUp);
		}

		private void CheckLevelUp()
		{
			if (experience < ToLevelUp) return;
			LevelUp();
		}

		private void LevelUp()
		{
			experience -= ToLevelUp;
			level++;
			experienceBar.SetLevelText(level);
			_upgradePanelManager.OpenPanel();
			OnLevelUp();
		}

		private void OnLevelUp()
		{
			var levelUpStrategyManager = new PlayerStrategyApplier();
			levelUpStrategyManager.ApplyLevelUpStrategy(GameData.GetPlayerCharacterId(), GameData.GetPlayerCharacterRank(), level, playerStatsComponent);
		}
	}
}