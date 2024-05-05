using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Characters;
using Objects.Drops;
using Objects.Enemies;
using Objects.Items;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Managers
{
	public class AchievementManager : MonoBehaviour
	{
		public static AchievementManager instance;
		private int _enemiesKilled;
		private int _bossKills;
		private int _menuScrolls;
		private int _highRarityPickupsObtained;
		private float _healAmountInOneGame;
		private float _damageTakenInOneGame;
		private float _distanceTraveled;
        
		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}
		
		public void ClearPerGameStats()
		{
			_enemiesKilled = 0;
			_highRarityPickupsObtained = 0;
			_healAmountInOneGame = 0;
			_damageTakenInOneGame = 0;
			_menuScrolls = 0;
		}

		public void OnStageTimeUpdated(float time)
		{
			var minutes = Mathf.FloorToInt(time / 60);
			var activeCharacterName = GameData.GetPlayerCharacterData().Id.GetName();
			var activeCharacterAlignment = GameData.GetPlayerCharacterData().Alignment;
			switch (minutes)
			{
				case >= 30:
					SaveFile.Instance.UnlockAchievement($"Survive30MinutesWith{activeCharacterName}");
					if (activeCharacterAlignment == CharacterAlignment.Light)
						SaveFile.Instance.UnlockAchievement(AchievementEnum.Survive30MinutesWithLightCharacter);
					else if (activeCharacterAlignment == CharacterAlignment.Dark)
						SaveFile.Instance.UnlockAchievement(AchievementEnum.Survive30MinutesWithDarkCharacter);
					break;
				case >= 15:
					SaveFile.Instance.UnlockAchievement($"Survive15MinutesWith{activeCharacterName}");
					break;
			}
		}
		public void OnWeaponUnlocked(WeaponBase weapon, int unlockedCount, int rarity)
		{
			if (rarity >= 3)
				SaveFile.Instance.TotalLegendaryItemsObtained++;
			
			if (SaveFile.Instance.TotalLegendaryItemsObtained > 20)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Obtain10HighRarityItems);
			
			if (unlockedCount == 2 && rarity == 5)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.HaveFirst5StarWeapon);
			
			if (unlockedCount == 6)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Hold6Weapons);
		}
		public void OnItemUnlocked(ItemBase item, int unlockedCount, int rarity)
		{
			if (rarity >= 3)
				SaveFile.Instance.TotalLegendaryItemsObtained++;
			if (SaveFile.Instance.TotalLegendaryItemsObtained > 20)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Obtain10HighRarityItems);
			
			if (unlockedCount == 1 && rarity == 5)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.HaveFirst5StarItem);
			
			if (unlockedCount == 6)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Hold6Items);
		}
		
		public void OnEnemyKilled(bool isBoss)
		{
			_enemiesKilled++;
			SaveFile.Instance.EnemiesKilled++;
			if (SaveFile.Instance.EnemiesKilled >= 100000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Kill100000Enemies);
			if (_enemiesKilled >= 1000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Kill1000EnemiesInOneGame);
			if (isBoss)
				SaveFile.Instance.BossKills++;
			if (SaveFile.Instance.BossKills > 50)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Kill500BossEnemies);
		}

		public void OnDeath()
		{
			SaveFile.Instance.UnlockAchievement(AchievementEnum.DieOnce);
			SaveFile.Instance.Deaths++;
			if (SaveFile.Instance.Deaths >= 20)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Die20Times);
		}

		public void OnHealing(float amount)
		{
			_healAmountInOneGame += Math.Abs(amount);
			SaveFile.Instance.HealAmountInOneGame = (ulong)_healAmountInOneGame;
			SaveFile.Instance.TotalAmountHealed = (ulong)(SaveFile.Instance.TotalAmountHealed + _healAmountInOneGame);
			
			if (_healAmountInOneGame >= 1000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Heal1000HealthInOneGame);
			
			if (_healAmountInOneGame > 10000 && _damageTakenInOneGame > 10000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.HealAndTake6000Damage);
		}

		public void OnDamageTaken(float amount)
		{
			_damageTakenInOneGame += amount;
			SaveFile.Instance.DamageTakeInOneGame = (ulong)_damageTakenInOneGame;
			SaveFile.Instance.TotalDamageTaken = (ulong)(SaveFile.Instance.TotalDamageTaken + _damageTakenInOneGame);
			
			
			if (_damageTakenInOneGame >= 1000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Take1000DamageInOneGame);
		}

		public void OnCharacterUnlocked(CharactersEnum characterId, CharacterRank rank)
		{
			if (rank is CharacterRank.E0 or CharacterRank.E5)
				SaveFile.Instance.UnlockAchievement($"Obtain{characterId.GetName()}_{rank}");
		}

		public void OnPull(CharactersEnum characterId)
		{
			SaveFile.Instance.PullsPerformed++;
			if (SaveFile.Instance.PullsPerformed >= 50)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.PerformGacha100Times);
			
			SaveFile.Instance.UnlockAchievement(AchievementEnum.PerformGacha);
		}
		
		public void OnPickupCollected(PickupEnum pickup)
		{
			if (pickup != PickupEnum.Experience)
				SaveFile.Instance.PickupsCollected++;
			
			switch (SaveFile.Instance.PickupsCollected)
			{
				case >= 1000 when pickup != PickupEnum.Experience:
					SaveFile.Instance.UnlockAchievement(AchievementEnum.Collect1000Pickups);
					break;
				case >= 100 when pickup != PickupEnum.Experience:
					SaveFile.Instance.UnlockAchievement(AchievementEnum.Collect100Pickups);
					break;
			}

			switch (pickup)
			{
				case PickupEnum.Map:
					SaveFile.Instance.UnlockAchievement(AchievementEnum.CollectMap);
					break;
				case PickupEnum.Magnet:
					SaveFile.Instance.UnlockAchievement(AchievementEnum.CollectMagnet);
					break;
			}
		}

		public void OnGameEnd(SaveFile saveFile)
		{
			if (saveFile.TimePlayed >= 60 * 5)
			{
				SaveFile.Instance.UnlockAchievement(AchievementEnum.PlayFor5Hours);
			}
		}

		public void OnLevelUp(LevelComponent levelComponent)
		{
			if (PlayerStatsScaler.GetScaler().GetCooldownReductionPercentage() <= 0.45f)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Reach55PercentCdr);
		}

		public void OnCharacterMove(float distanceMoved)
		{
			SaveFile.Instance.DistanceTraveled += distanceMoved;
			if (SaveFile.Instance.DistanceTraveled > 10000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.WalkAThousandSteps);
		}

		public void OnMenuCharacterChange()
		{
			_menuScrolls++;
			if (_menuScrolls >= 69)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.ScrollCharactersTooManyTimes);
		}
	}
}