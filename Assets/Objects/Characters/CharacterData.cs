using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data;
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
		public Sprite Avatar;
		public Sprite CharacterSprite;
		public Sprite signet;
		[TextArea]
		public string PassiveDescription;
		public string AbilityName;
		[TextArea]
		public string AbilityDescription;
		public float AbilityCooldown;
		public CharacterSkillBase SpecialPrefab;
		public CharacterSkillBase AbilityPrefab;
		public Sprite AbilityIcon;
		public Color ColorTheme;
		public int StarRating;
		public WeaponBase StartingWeapon;
		public bool IsPullable;
		public bool UseSpecialBar;
		public bool PickWeaponOnStart;
		public PlayerStats Stats;
		public List<SkillNode> skillNodes;
		public List<EidolonData> Eidolons;
		public List<CharacterLoreEntry> loreEntries;
		
		public IEnumerable<StatsDisplayData> GetStatsList()
		{
			return Stats.GetStatsList();
		}
	}
}