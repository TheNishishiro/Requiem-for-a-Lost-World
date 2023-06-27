using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Characters;
using Objects.Drops;
using Objects.Enemies;
using Objects.Items;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Managers
{
	public class AchievementManager : MonoBehaviour
	{
		public void Awake()
		{
			var instances = FindObjectsOfType<AchievementManager>();
			if (instances.Length > 1)
			{
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(gameObject);
		}

		public void OnStageTimeUpdated(float time)
		{
			var minutes = Mathf.FloorToInt(time / 60);
			var activeCharacterName = GameData.GetPlayerCharacterId().GetName();
			switch (minutes)
			{
				case >= 30:
					SaveFile.Instance.UnlockAchievement($"Survive30MinutesWith{activeCharacterName}");
					break;
				case >= 15:
					SaveFile.Instance.UnlockAchievement($"Survive15MinutesWith{activeCharacterName}");
					break;
			}
		}
		public void OnWeaponUnlocked(WeaponBase weapon, int unlockedCount)
		{
			if (unlockedCount == 6)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Hold6Weapons);
		}
		public void OnItemUnlocked(ItemBase item, int unlockedCount)
		{
			if (unlockedCount == 6)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Hold6Items);
		}
		
		public void OnEnemyKilled(Enemy enemy)
		{
			SaveFile.Instance.EnemiesKilled++;
			if (SaveFile.Instance.EnemiesKilled >= 100000)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Kill100000Enemies);
		}

		public void OnCharacterUnlocked(CharactersEnum characterId, CharacterRank rank)
		{
			if (rank is CharacterRank.S or CharacterRank.SS or CharacterRank.SSS)
				SaveFile.Instance.UnlockAchievement($"Obtain{characterId.GetName()}_{rank}");
		}

		public void OnPull(CharacterData characterData)
		{
			SaveFile.Instance.PullsPerformed++;
			if (SaveFile.Instance.PullsPerformed >= 100)
				SaveFile.Instance.UnlockAchievement(AchievementEnum.PerformGacha100Times);
			
			SaveFile.Instance.UnlockAchievement(AchievementEnum.PerformGacha);
		}
		
		public void OnPickupCollected(PickupEnum pickup)
		{
			if (SaveFile.Instance.PickupsCollected >= 1000 && pickup != PickupEnum.Experience)
			{
				SaveFile.Instance.PickupsCollected++;
				SaveFile.Instance.UnlockAchievement(AchievementEnum.Collect1000Pickups);
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
	}
}