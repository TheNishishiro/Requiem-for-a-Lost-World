using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Difficulty;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Extensions;
using JetBrains.Annotations;
using Managers;
using Newtonsoft.Json;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UI.Shared;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace DefaultNamespace.Data
{
	public class SaveFile : MonoBehaviour
	{
		private const string SavePath = "./saveFile.json";
		public Dictionary<CharactersEnum, CharacterSaveData> CharacterSaveData;
		public Dictionary<PermUpgradeType, int> PermUpgradeSaveData;
		public Dictionary<AchievementEnum, bool> AchievementSaveData;
		public Dictionary<int, List<int>> ReadStoryEntries { get; set; }
		public ConfigurationFile ConfigurationFile;
		public CharactersEnum? SelectedCharacterId;
		public ulong Gold;
		public ulong Gems;
		public ulong EnemiesKilled;
		public ulong PickupsCollected;
		public ulong PullsPerformed;
		public ulong Deaths;
		public ulong StoryPoints;
		public ulong TimePlayed;
		public ulong BossKills;
		public bool IsFirstTutorialCompleted;
		public int Pity;
		public DifficultyEnum SelectedDifficulty;
		public UnityEvent<AchievementEnum> AchievementUnlocked;
		public UnityEvent<CharactersEnum, CharacterRank> OnCharacterUnlocked;
		
		public static SaveFile Instance { get; private set; }

		private void Awake()
		{
			var saveFiles = FindObjectsOfType<SaveFile>();
			if (saveFiles.Length > 1)
			{
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(gameObject);
			Instance = this;
			Initialize();
			Load();
		}

		public void Initialize()
		{
			CharacterSaveData ??= new Dictionary<CharactersEnum, CharacterSaveData>();
			PermUpgradeSaveData ??= new Dictionary<PermUpgradeType, int>();
			AchievementSaveData ??= new Dictionary<AchievementEnum, bool>();
			ReadStoryEntries ??= new Dictionary<int, List<int>>();
			ConfigurationFile ??= new ConfigurationFile().Default();
			ConfigurationFile.Update();
		}

		public void Save()
		{
			var jsonSave = JsonConvert.SerializeObject(new SaveData(this), Formatting.Indented);
			File.WriteAllText(SavePath, jsonSave);
		}
		
		public void Load()
		{
			if (!File.Exists(SavePath)) return;
			
			var json = File.ReadAllText(SavePath);
			var settings = new JsonSerializerSettings
			{
				MissingMemberHandling = MissingMemberHandling.Ignore,
				Error = delegate(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
				{
					args.ErrorContext.Handled = true;
				}
			};
			var loadedData = JsonConvert.DeserializeObject<SaveData>(json, settings);
			Apply(loadedData);
		}
		
		public void Apply(SaveData saveData)
		{
			Gold = saveData.Gold;
			Gems = saveData.Gems;
			Deaths = saveData.Deaths;
			StoryPoints = saveData.StoryPoints;
			EnemiesKilled = saveData.EnemiesKilled;
			PickupsCollected = saveData.PickupsCollected;
			PullsPerformed = saveData.PullsPerformed;
			TimePlayed = saveData.TimePlayed;
			BossKills = saveData.BossKills;
			IsFirstTutorialCompleted = saveData.IsFirstTutorialCompleted;
			Pity = saveData.Pity;
			SelectedDifficulty = saveData.SelectedDifficulty;
			SelectedCharacterId = saveData.SelectedCharacterId;
			CharacterSaveData = saveData.CharacterSaveData ?? new Dictionary<CharactersEnum, CharacterSaveData>();
			PermUpgradeSaveData = saveData.PermUpgradeSaveData ?? new Dictionary<PermUpgradeType, int>();
			AchievementSaveData = saveData.AchievementSaveData ?? new Dictionary<AchievementEnum, bool>();
			ConfigurationFile = (saveData.ConfigurationFile ?? new ConfigurationFile().Default()).Update();
			ReadStoryEntries = saveData.ReadStoryEntries ?? new Dictionary<int, List<int>>();
		}

		public void AddGameResultData(GameResultData gameResultData)
		{
			Gold += (ulong)gameResultData.Gold;
			Gems += (ulong)gameResultData.Gems;
			StoryPoints += (ulong)(gameResultData.Time.ToMinutes()*3);
			TimePlayed += (ulong)gameResultData.Time.ToMinutes();
			
			AchievementManager.instance.OnGameEnd(this, gameResultData);
		}

		public void AddUpgradeLevel(PermUpgradeType permUpgradeType)
		{
			PermUpgradeSaveData ??= new Dictionary<PermUpgradeType, int>();
			
			if (PermUpgradeSaveData.ContainsKey(permUpgradeType))
			{
				PermUpgradeSaveData[permUpgradeType]++;
			}
			else
			{
				PermUpgradeSaveData.Add(permUpgradeType, 1);
			}
		}

		public CharacterSaveData GetCharacterSaveData(CharactersEnum characterId)
		{
			Initialize();
			return CharacterSaveData.ContainsKey(characterId) ? CharacterSaveData[characterId] : new CharacterSaveData();
		}

		public void UpdateMissingCharacterEntries(List<CharacterData> characterData)
		{
			if (characterData == null) return;

			foreach (var character in characterData.Where(character => !CharacterSaveData.ContainsKey(character.Id)))
			{
				CharacterSaveData.Add(character.Id, new CharacterSaveData());
			}
		}

		public void UpdateMissingAchievementEntries()
		{
			foreach (var achievementEnum in Enum.GetValues(typeof(AchievementEnum)).Cast<AchievementEnum>())
			{
				if (!AchievementSaveData.ContainsKey(achievementEnum))
					AchievementSaveData.Add(achievementEnum, false);
			}
		}

		public void UnlockCharacter(CharacterData pullResult)
		{
			if (!CharacterSaveData.ContainsKey(pullResult.Id))
			{
				CharacterSaveData.Add(pullResult.Id, new CharacterSaveData());
			}
			
			CharacterSaveData[pullResult.Id].Unlock();
			OnCharacterUnlocked?.Invoke(pullResult.Id, CharacterSaveData[pullResult.Id].GetRankEnum());
		}

		public void UnlockAchievement(string achievementEnum)
		{
			UnlockAchievement((AchievementEnum) Enum.Parse(typeof(AchievementEnum), achievementEnum));
		}
		
		public void UnlockAchievement(AchievementEnum achievementEnum)
		{
			if (IsAchievementUnlocked(achievementEnum)) return;

			Gems += 210;
			if (!AchievementSaveData.ContainsKey(achievementEnum))
				AchievementSaveData.Add(achievementEnum, true);
			else
				AchievementSaveData[achievementEnum] = true;
			
			FindObjectOfType<AchievementGetDisplay>(true).Display(achievementEnum);
		}

		public bool IsAchievementUnlocked(AchievementEnum achievementEnum)
		{
			return AchievementSaveData.ContainsKey(achievementEnum) && AchievementSaveData[achievementEnum];
		}

		public bool IsStoryRead(int loreEntryChapterNumber, int loreEntryEntryNumber)
		{
			ReadStoryEntries ??= new Dictionary<int, List<int>>();
			if (!ReadStoryEntries.ContainsKey(loreEntryChapterNumber))
				return false;
			return ReadStoryEntries[loreEntryChapterNumber]?.Exists(x => x == loreEntryEntryNumber) == true;
		}

		public void SaveReadStoryEntry(int loreEntryChapterNumber, int loreEntryEntryNumber)
		{
			ReadStoryEntries ??= new Dictionary<int, List<int>>();
			if (!ReadStoryEntries.ContainsKey(loreEntryChapterNumber))
			{
				ReadStoryEntries.Add(loreEntryChapterNumber, new List<int>());
			}

			ReadStoryEntries[loreEntryChapterNumber] ??= new List<int>();
			ReadStoryEntries[loreEntryChapterNumber].Add(loreEntryEntryNumber);
		}
	}
	
	[Serializable]
	public class SaveData
	{
		public Dictionary<CharactersEnum, CharacterSaveData> CharacterSaveData;
		public Dictionary<PermUpgradeType, int> PermUpgradeSaveData;
		public Dictionary<AchievementEnum, bool> AchievementSaveData;
		public Dictionary<int, List<int>> ReadStoryEntries;
		public ConfigurationFile ConfigurationFile;
		public ulong Gold;
		public ulong Gems;
		public ulong EnemiesKilled;
		public ulong PickupsCollected;
		public ulong PullsPerformed;
		public ulong Deaths;
		public ulong StoryPoints;
		public ulong TimePlayed;
		public ulong BossKills;
		public bool IsFirstTutorialCompleted;
		public int Pity;
		public DifficultyEnum SelectedDifficulty;
		public CharactersEnum? SelectedCharacterId;
		
		public SaveData(){}
		
		public SaveData(SaveFile saveFile)
		{
			CharacterSaveData = saveFile.CharacterSaveData;
			PermUpgradeSaveData = saveFile.PermUpgradeSaveData;
			AchievementSaveData = saveFile.AchievementSaveData;
			ConfigurationFile = saveFile.ConfigurationFile;
			Gold = saveFile.Gold;
			Gems = saveFile.Gems;
			EnemiesKilled = saveFile.EnemiesKilled;
			PickupsCollected = saveFile.PickupsCollected;
			PullsPerformed = saveFile.PullsPerformed;
			Deaths = saveFile.Deaths;
			StoryPoints = saveFile.StoryPoints;
			IsFirstTutorialCompleted = saveFile.IsFirstTutorialCompleted;
			Pity = saveFile.Pity;
			ReadStoryEntries = saveFile.ReadStoryEntries;
			SelectedCharacterId = saveFile.SelectedCharacterId;
			TimePlayed = saveFile.TimePlayed;
			BossKills = saveFile.BossKills;
			SelectedDifficulty = saveFile.SelectedDifficulty;
		}

	}
}