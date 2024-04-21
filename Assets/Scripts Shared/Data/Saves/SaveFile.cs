using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Difficulty;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Settings;
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
		private const string _saveName = "saveFile.json";
		private static string SavePath => Application.platform == RuntimePlatform.Android ? Path.Combine(Application.persistentDataPath, _saveName) : Path.Combine("./", _saveName);
		public Dictionary<CharactersEnum, CharacterSaveData> CharacterSaveData;
		public Dictionary<PermUpgradeType, int> PermUpgradeSaveData;
		public Dictionary<AchievementEnum, bool> AchievementSaveData;
		public Dictionary<KeyAction, KeyCode> Keybindings;
		public Dictionary<int, List<int>> ReadStoryEntries { get; set; }
		public List<ServerData> Servers;
		public List<CharactersEnum> BannerHistory;
		public ConfigurationFile ConfigurationFile;
		public CharactersEnum? SelectedCharacterId;
		public CharactersEnum CurrentBannerCharacterId;
		public CharactersEnum CurrentBannerSubCharacterId1;
		public CharactersEnum CurrentBannerSubCharacterId2;
		public CharactersEnum CurrentBannerSubCharacterId3;
		public DateTime? NextBannerChangeDate;
		public ulong Gold;
		public ulong Gems;
		public ulong EnemiesKilled;
		public ulong PickupsCollected;
		public ulong PullsPerformed;
		public ulong Deaths;
		public ulong StoryPoints;
		public ulong TimePlayed;
		public ulong BossKills;
		public ulong TotalLegendaryItemsObtained;
		public ulong HealAmountInOneGame;
		public ulong TotalAmountHealed;
		public ulong DamageTakeInOneGame;
		public ulong TotalDamageTaken;
		public bool IsFirstTutorialCompleted;
		public int Pity;
		public DifficultyEnum SelectedDifficulty;
		public UnityEvent<AchievementEnum> AchievementUnlocked;
		public UnityEvent<CharactersEnum, CharacterRank> OnCharacterUnlocked;
		
		public static SaveFile Instance { get; private set; }

		private void Awake()
		{
			var saveFiles = FindObjectsByType<SaveFile>(FindObjectsSortMode.None);
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
			Servers ??= new List<ServerData>();
			BannerHistory ??= new List<CharactersEnum>();
			AchievementSaveData ??= new Dictionary<AchievementEnum, bool>();
			ReadStoryEntries ??= new Dictionary<int, List<int>>();
			ConfigurationFile ??= new ConfigurationFile().Default();
			Keybindings ??= DefaultKeyBinds();
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
			TotalLegendaryItemsObtained = saveData.TotalLegendaryItemsObtained;
			HealAmountInOneGame = saveData.HealAmountInOneGame;
			TotalAmountHealed = saveData.TotalAmountHealed;
			DamageTakeInOneGame = saveData.DamageTakeInOneGame;
			TotalDamageTaken = saveData.TotalDamageTaken;
			IsFirstTutorialCompleted = saveData.IsFirstTutorialCompleted;
			Pity = saveData.Pity;
			CurrentBannerCharacterId = saveData.CurrentBannerCharacterId;    
			CurrentBannerSubCharacterId1 = saveData.CurrentBannerSubCharacterId1;
			CurrentBannerSubCharacterId2 = saveData.CurrentBannerSubCharacterId2;
			CurrentBannerSubCharacterId3 = saveData.CurrentBannerSubCharacterId3;
			NextBannerChangeDate = saveData.LastBannerChangeDate;
			SelectedDifficulty = saveData.SelectedDifficulty;
			SelectedCharacterId = saveData.SelectedCharacterId;
			BannerHistory = saveData.BannerHistory ?? new List<CharactersEnum>();
			CharacterSaveData = saveData.CharacterSaveData ?? new Dictionary<CharactersEnum, CharacterSaveData>();
			PermUpgradeSaveData = saveData.PermUpgradeSaveData ?? new Dictionary<PermUpgradeType, int>();
			Servers = saveData.Servers ?? new List<ServerData>();
			AchievementSaveData = saveData.AchievementSaveData ?? new Dictionary<AchievementEnum, bool>();
			Keybindings = saveData.Keybindings ?? DefaultKeyBinds();
			ConfigurationFile = (saveData.ConfigurationFile ?? new ConfigurationFile().Default()).Update();
			ReadStoryEntries = saveData.ReadStoryEntries ?? new Dictionary<int, List<int>>();
		}

		private Dictionary<KeyAction, KeyCode> DefaultKeyBinds()
		{
			if (Keybindings?.Any() == true)
				return Keybindings;

			var keybindings = new Dictionary<KeyAction, KeyCode>
			{
				{ KeyAction.Ability, KeyCode.Space },
				{ KeyAction.MoveDown, KeyCode.S },
				{ KeyAction.MoveUp, KeyCode.W },
				{ KeyAction.MoveLeft, KeyCode.A },
				{ KeyAction.MoveRight, KeyCode.R },
				{ KeyAction.Accept, KeyCode.Return },
				{ KeyAction.Dash, KeyCode.LeftShift },
				{ KeyAction.Sprint, KeyCode.LeftControl }
			};
			return keybindings;
		}

		public KeyCode GetKeybinding(KeyAction action)
		{
			return Keybindings[action];
		}

		public void AddGameResultData()
		{
			Gold += (ulong)GameResultData.Gold;
			Gems += (ulong)GameResultData.Gems;
			StoryPoints += (ulong)(GameResultData.Time.ToMinutes()*3);
			TimePlayed += (ulong)GameResultData.Time.ToMinutes();
			
			AchievementManager.instance.OnGameEnd(this);
		}

		public void AddUpgradeLevel(PermUpgradeType permUpgradeType)
		{
			PermUpgradeSaveData ??= new Dictionary<PermUpgradeType, int>();
			
			if (!PermUpgradeSaveData.TryAdd(permUpgradeType, 1))
			{
				PermUpgradeSaveData[permUpgradeType]++;
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
				AchievementSaveData.TryAdd(achievementEnum, false);
			}
		}

		public void UnlockCharacter(CharactersEnum characterId)
		{
			if (!CharacterSaveData.ContainsKey(characterId))
			{
				CharacterSaveData.Add(characterId, new CharacterSaveData());
			}
			
			CharacterSaveData[characterId].Unlock();
			OnCharacterUnlocked?.Invoke(characterId, CharacterSaveData[characterId].GetRankEnum());
		}

		public void UnlockAchievement(string achievementEnum)
		{
			UnlockAchievement((AchievementEnum) Enum.Parse(typeof(AchievementEnum), achievementEnum));
		}
		
		public void UnlockAchievement(AchievementEnum achievementEnum)
		{
			if (IsAchievementUnlocked(achievementEnum)) return;

			var achievementData = achievementEnum.GetAchievementValue();
			switch (achievementData.Reward.Key)
			{
				case RewardType.Gems:
					Gems += (ulong)achievementData.Reward.Value;
					break;
				case RewardType.Coins:
					Gold += (ulong)achievementData.Reward.Value;
					break;
				case RewardType.Shards:
					UnlockCharacter((CharactersEnum)(int)achievementData.Reward.Value);
			        break;
			}

			
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

		public void ExchangeGoldForGems()
		{
			Gold -= 500;
			Gems += 200;
			Save();
		}
	}
	
	[Serializable]
	public class SaveData
	{
		public Dictionary<CharactersEnum, CharacterSaveData> CharacterSaveData;
		public Dictionary<PermUpgradeType, int> PermUpgradeSaveData;
		public Dictionary<AchievementEnum, bool> AchievementSaveData;
		public Dictionary<KeyAction, KeyCode> Keybindings;
		public Dictionary<int, List<int>> ReadStoryEntries;
		public List<ServerData> Servers;
		public List<CharactersEnum> BannerHistory;
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
		public ulong TotalLegendaryItemsObtained;
		public ulong HealAmountInOneGame;
		public ulong TotalAmountHealed;
		public ulong DamageTakeInOneGame;
		public ulong TotalDamageTaken;
		public bool IsFirstTutorialCompleted;
		public int Pity;
		public DifficultyEnum SelectedDifficulty;
		public CharactersEnum? SelectedCharacterId;
		public CharactersEnum CurrentBannerCharacterId;
		public CharactersEnum CurrentBannerSubCharacterId1;
		public CharactersEnum CurrentBannerSubCharacterId2;
		public CharactersEnum CurrentBannerSubCharacterId3;
		public DateTime? LastBannerChangeDate;
		
		public SaveData(){}
		
		public SaveData(SaveFile saveFile)
		{
			CharacterSaveData = saveFile.CharacterSaveData;
			PermUpgradeSaveData = saveFile.PermUpgradeSaveData;
			Servers = saveFile.Servers;
			BannerHistory = saveFile.BannerHistory;
			AchievementSaveData = saveFile.AchievementSaveData;
			Keybindings = saveFile.Keybindings;
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
			TotalLegendaryItemsObtained = saveFile.TotalLegendaryItemsObtained;
			HealAmountInOneGame = saveFile.HealAmountInOneGame;
			TotalAmountHealed = saveFile.TotalAmountHealed;
			DamageTakeInOneGame = saveFile.DamageTakeInOneGame;
			TotalDamageTaken = saveFile.TotalDamageTaken;
			CurrentBannerCharacterId = saveFile.CurrentBannerCharacterId;
			CurrentBannerSubCharacterId1 = saveFile.CurrentBannerSubCharacterId1;
			CurrentBannerSubCharacterId2 = saveFile.CurrentBannerSubCharacterId2;
			CurrentBannerSubCharacterId3 = saveFile.CurrentBannerSubCharacterId3;
			LastBannerChangeDate = saveFile.NextBannerChangeDate;
		}

	}
}