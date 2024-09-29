using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Locale;
using Managers;
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
		public LocalizedText Description;
		public LocalizedText SkillName;
		public LocalizedText SkillDescription;
		public float AbilityCooldown;
		public CharacterSkillBase SpecialPrefab;
		public CharacterSkillBase AbilityPrefab;
		public CharacterAchievementListener AchievementListenerPrefab;
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
		private string BonusSeparator => "\n\n" + new string('\u2500', 5) + $"» {"Bonus Stats".Translate()} «"  + new string('\u2500', 5);

		public string GetWeaponDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(StartingWeapon.GetDescription(1));
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.WeaponUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.GetWeaponUpgradeDescription());
			}
			
			return sb.ToString();
		}

		public string GetAbilityDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(AbilityDescription.Translate());
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.SkillUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.GetSkillUpgradeDescription());
			}
			
			return sb.ToString();
		}

		public string GetPassiveDescription(int maxUnlockedEidolon)
		{
			var sb = new StringBuilder(PassiveDescription.Translate());
			var unlockedShards = Eidolons.Take(maxUnlockedEidolon).Where(x => !string.IsNullOrWhiteSpace(x.PassiveUpgradeDescription)).ToList();
			if (unlockedShards.Any())
				sb.AppendLine(BonusSeparator);

			foreach (var shard in unlockedShards)
			{
				sb.AppendLine("\nº " + shard.GetPassiveUpgradeDescription());
			}
			
			return sb.ToString();
		}
	}
}