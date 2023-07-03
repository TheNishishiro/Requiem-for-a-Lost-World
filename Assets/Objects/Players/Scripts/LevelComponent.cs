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
			var activeCharacter = GameData.GetPlayerCharacterId();
			var rankup = GameData.GetPlayerCharacterRank();
			
			switch (activeCharacter)
			{
				case CharactersEnum.Chitose:
					ChitoseLevelUp(rankup);
					break;
				case CharactersEnum.Maid:
					MaidLevelUp(rankup);
					break;
				case CharactersEnum.Amelia_BoD:
					AmeliaLevelUp(rankup);
					break;
				case CharactersEnum.David_BoF:
					DavidLevelUp(rankup);
					break;
				case CharactersEnum.Arika_BoV:
					ArikaLevelUp(rankup);
					break;
			}
		}

		private void ChitoseLevelUp(CharacterRank rank)
		{
			var reduction = rank >= CharacterRank.E5 ? 5 : 0;
					
			if (level % (10 - reduction) == 0)
			{
				playerStatsComponent.IncreaseSpeed(0.05f);
			}

			if (rank >= CharacterRank.E3 && level % (20 - reduction) == 0)
			{
				playerStatsComponent.IncreaseCooldownReductionPercentage(0.08f);
			}
		}

		private void MaidLevelUp(CharacterRank rank)
		{
			var maxCritRate = rank >= CharacterRank.E3 ? 70 : 40;
			
			if (level <= maxCritRate && level % 10 == 0)
			{
				playerStatsComponent.IncreaseCritRate(0.05f);
			}

			if (rank >= CharacterRank.E5 && level % 5 == 0)
			{
				playerStatsComponent.IncreaseCritDamage(0.03f);
			}
		}

		public void AmeliaLevelUp(CharacterRank rank)
		{
			if (level <= 45 && level % 15 == 0)
			{
				playerStatsComponent.IncreaseAttackCount(1);
			}

			if (level % 15 == 0 && rank < CharacterRank.E3)
			{
				playerStatsComponent.IncreaseEnemyHealth(0.07f);
			}
			
			if (level % 13 == 0 && rank >= CharacterRank.E5)
			{
				playerStatsComponent.IncreaseFlatDamage(1);
			}
		}

		private void DavidLevelUp(CharacterRank rank)
		{
			if (level % 12 == 0 && rank >= CharacterRank.E3)
			{
				playerStatsComponent.IncreaseDamageIncreasePercentage(0.05f);
			}
		}

		private void ArikaLevelUp(CharacterRank rank)
		{
			if (level % 20 == 0)
			{
				var amount = rank >= CharacterRank.E3 ? 0.07f : 0.1f;
				playerStatsComponent.IncreaseProjectileSize(amount);
			}
		}
	}
}