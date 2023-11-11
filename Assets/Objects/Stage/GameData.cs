using System.Collections.Generic;
using DefaultNamespace.Data;
using Managers;
using NUnit.Framework;
using Objects.Characters;
using Objects.Players;
using Objects.Players.PermUpgrades;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Objects.Stage
{
	public class GameData
	{
		public static Sprite GetPlayerAbilityIcon()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.AbilityIcon;
		}

		public static Sprite GetPlayerCharacterAvatar()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.Avatar;
		}
		
		public static Color? GetPlayerColorTheme()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.ColorTheme;
		}
		
		public static CharacterData GetPlayerCharacterData()
		{
			return CharacterListManager.instance?.GetActiveCharacter();
		}
		
		public static CharacterSaveData GetPlayerCharacterSaveData()
		{
			return CharacterListManager.instance?.GetActiveCharacterData();
		}
		
		public static WeaponBase GetPlayerCharacterStartingWeapon()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.StartingWeapon;
		}
		
		public static CharactersEnum GetPlayerCharacterId()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.Id ?? CharactersEnum.Maid;
		}
		
		public static CharacterRank GetPlayerCharacterRank()
		{
			return CharacterListManager.instance?.GetActiveRankCharacter() ?? CharacterRank.E0;
		}
		
		public static float GetCharacterSkillCooldown()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.AbilityCooldown ?? 1f;
		}
		
		public static CharacterSkillBase GetSkillPrefab()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.AbilityPrefab;
		}
		
		public static CharacterSkillBase GetSpecialPrefab()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.SpecialPrefab;
		}

		public static Sprite GetPlayerSprite()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.CharacterSprite;
		}

		public static PlayerStats GetPlayerStartingStats()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.Stats ?? new PlayerStats();
		}

		public static List<int> GetUnlockedSkillTreeNodeIds()
		{
			return GetPlayerCharacterSaveData()?.unlockedSkillPoints ?? new List<int>();
		}

		public static IEnumerable<PermUpgrade> GetPermUpgrades()
		{
			return PermUpgradeListManager.instance?.GetUpgrades() ?? new List<PermUpgrade>();
		}

		public static List<SkillNode> GetCharacterSkillNodes()
		{
			return CharacterListManager.instance?.GetActiveCharacter()?.skillNodes ?? new List<SkillNode>();
		}
	}
}