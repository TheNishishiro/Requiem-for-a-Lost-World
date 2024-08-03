using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Difficulty;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Cameras;
using DefaultNamespace.Data.Modals;
using DefaultNamespace.Data.Settings;
using DefaultNamespace.Extensions;
using JetBrains.Annotations;
using Managers;
using Newtonsoft.Json;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UI.Main_Menu.REWORK.Scripts;
using UI.Shared;
using Unity.VisualScripting;
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
		public List<RuneSaveData> Runes;
		public ConfigurationFile ConfigurationFile;
		public bool IsFirstTutorialCompleted;
		public DifficultyEnum SelectedDifficulty;
		#region Recollection
		public List<CharactersEnum> BannerHistory;
		public CharactersEnum? SelectedCharacterId;
		public CharactersEnum CurrentBannerCharacterId;
		public CharactersEnum CurrentBannerSubCharacterId1;
		public CharactersEnum CurrentBannerSubCharacterId2;
		public CharactersEnum CurrentBannerSubCharacterId3;
		public DateTime? NextBannerChangeDate;
		public int Pity;
		#endregion
		#region Game Stats
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
		#endregion
		#region Rewards
		public bool IsTwitterVisited;
		public bool IsDiscordVisited;
		public bool IsItchIoVisited;
		public bool IsFeedbackLeft;
		#endregion
		#region Game Settings
		public bool IsCoopAllowed;
		public bool IsShortPlayTime;
		#endregion
		public UnityEvent<AchievementEnum> AchievementUnlocked;
		public UnityEvent<CharactersEnum, CharacterRank> OnCharacterUnlocked;
		
		public static SaveFile Instance { get; private set; }
		public double DistanceTraveled { get; set; }
		public ulong ShrinesVisited { get; set; }

		public CameraModes CameraMode;

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
			Runes ??= new List<RuneSaveData>();
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
				ObjectCreationHandling = ObjectCreationHandling.Replace,
				Error = delegate(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
				{
					args.ErrorContext.Handled = true;
				}
			};
			JsonConvert.PopulateObject(json, this, settings);
			BannerHistory ??= new List<CharactersEnum>();
			CharacterSaveData ??= new Dictionary<CharactersEnum, CharacterSaveData>();
			PermUpgradeSaveData ??= new Dictionary<PermUpgradeType, int>();
			Servers ??= new List<ServerData>();
			Runes ??= new List<RuneSaveData>();
			AchievementSaveData ??= new Dictionary<AchievementEnum, bool>();
			Keybindings ??= DefaultKeyBinds();
			ConfigurationFile = (ConfigurationFile ?? new ConfigurationFile().Default()).Update();
			ReadStoryEntries ??= new Dictionary<int, List<int>>();
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
			
			FindFirstObjectByType<AchievementGetDisplay>(FindObjectsInactive.Include).Display(achievementEnum);
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
			if (Gold < 500) return;
            
			Gold -= 500;
			Gems += 200;
			Save();
		}

		public void ExchangeGemsForFragments(CharactersEnum characterId)
		{
			if (Gems < 250) return;

			Gems -= 250;
			GetCharacterSaveData(characterId).AddFragments(1);
			Save();
		}

		public void DiscardRune(RuneSaveData runeSaveData)
		{
			Runes.Remove(runeSaveData);
		}

		public void AddRune(RuneSaveData rune)
		{
			Runes.Add(rune);
		}

		public void MarkOpened(OpenedLinks openedLinks)
		{
			switch (openedLinks)
			{
				case OpenedLinks.Twitter when !IsTwitterVisited:
					ModalManager.instance.Open(ButtonCombination.Yes, "Rewards", $"Thanks for visiting our twitter page, here is your reward of 1000 gems", "Ok!");
					Gems += 1000;
					IsTwitterVisited = true;
					break;
				case OpenedLinks.Discord when !IsDiscordVisited:
					ModalManager.instance.Open(ButtonCombination.Yes, "Rewards", $"Thanks for visiting our discord server, here is your reward of 1000 gems", "Ok!");
					Gems += 1000;
					IsDiscordVisited = true;
					break;
				case OpenedLinks.ItchIo when !IsItchIoVisited:
					ModalManager.instance.Open(ButtonCombination.Yes, "Rewards", $"Thanks for visiting our itch.io page, here is your reward of 1000 gems", "Ok!");
					Gems += 1000;
					IsItchIoVisited = true;
					break;
				case OpenedLinks.Feedback when !IsFeedbackLeft:
					ModalManager.instance.Open(ButtonCombination.Yes, "Rewards", $"Thank you for leaving feedback, here is your reward of 2000 gems", "Ok!");
					Gems += 2000;
					IsFeedbackLeft = true;
					break;
			}
			
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
		public List<RuneSaveData> Runes;
		public List<ServerData> Servers;
		public List<CharactersEnum> BannerHistory;
		public ConfigurationFile ConfigurationFile;
		public CameraModes CameraMode;
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
		public ulong ShrinesVisited;
		public double DistanceTraveled;
		public bool IsFirstTutorialCompleted;
		public bool IsTwitterVisited;
		public bool IsDiscordVisited;
		public bool IsItchIoVisited;
		public bool IsFeedbackLeft;
		public int Pity;
		public DifficultyEnum SelectedDifficulty;
		public bool IsCoopAllowed;
		public bool IsShortPlayTime;
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
			Runes = saveFile.Runes;
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
			ShrinesVisited = saveFile.ShrinesVisited;
			BossKills = saveFile.BossKills;
			SelectedDifficulty = saveFile.SelectedDifficulty;
			TotalLegendaryItemsObtained = saveFile.TotalLegendaryItemsObtained;
			HealAmountInOneGame = saveFile.HealAmountInOneGame;
			TotalAmountHealed = saveFile.TotalAmountHealed;
			DamageTakeInOneGame = saveFile.DamageTakeInOneGame;
			DistanceTraveled = saveFile.DistanceTraveled;
			TotalDamageTaken = saveFile.TotalDamageTaken;
			CurrentBannerCharacterId = saveFile.CurrentBannerCharacterId;
			CurrentBannerSubCharacterId1 = saveFile.CurrentBannerSubCharacterId1;
			CurrentBannerSubCharacterId2 = saveFile.CurrentBannerSubCharacterId2;
			CurrentBannerSubCharacterId3 = saveFile.CurrentBannerSubCharacterId3;
			LastBannerChangeDate = saveFile.NextBannerChangeDate;
			CameraMode = saveFile.CameraMode;
			IsTwitterVisited = saveFile.IsTwitterVisited;
			IsDiscordVisited = saveFile.IsDiscordVisited;
			IsItchIoVisited = saveFile.IsItchIoVisited;
			IsFeedbackLeft = saveFile.IsFeedbackLeft;
			IsCoopAllowed = saveFile.IsCoopAllowed;
			IsShortPlayTime = saveFile.IsShortPlayTime;
		}

	}
}