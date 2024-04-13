using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace.Data;
using NaughtyAttributes;
using Objects.Players;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Characters
{
	[CreateAssetMenu]
	public class CharacterData : ScriptableObject
	{
		public CharactersEnum Id;
		public string Name;
		public string Title;
		public Sprite CharacterCard;
		public Sprite GatchaArt;
		public Vector2 GachaSubArtOffset;
		public Sprite Avatar;
		public Sprite CharacterSprite;
		public Sprite signet;
		public AudioClip nameAnnouncer;
		[ResizableTextArea]
		public string PassiveDescription;
		public string AbilityName;
		[ResizableTextArea]
		public string AbilityDescription;
		public float AbilityCooldown;
		public CharacterSkillBase SpecialPrefab;
		public CharacterSkillBase AbilityPrefab;
		public Sprite AbilityIcon;
		public Color ColorTheme;
		public CharacterAlignment Alignment;
		public int StarRating;
		public WeaponBase StartingWeapon;
		public bool IsPullable;
		public bool UseSpecialBar;
		public bool PickWeaponOnStart;
		public PlayerStats Stats;
		public List<SkillNode> skillNodes;
		public List<EidolonData> Eidolons;
		public List<CharacterLoreEntry> loreEntries;
		private string BonusSeparator => "\n\n" + new string('\u2500', 9) + "» Bonus stats «" + new string('\u2500', 9);

		public IEnumerable<StatsDisplayData> GetStatsList()
		{
			return Stats.GetStatsList();
		}

		public string GetWeaponDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(StartingWeapon.GetDescription(1));
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.WeaponUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.WeaponUpgradeDescription);
			}
			
			return sb.ToString();
		}

		public string GetAbilityDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(AbilityDescription);
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.SkillUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.SkillUpgradeDescription);
			}
			
			return sb.ToString();
		}

		public string GetPassiveDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(PassiveDescription);
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.PassiveUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.PassiveUpgradeDescription);
			}
			
			return sb.ToString();
		}
	}
}