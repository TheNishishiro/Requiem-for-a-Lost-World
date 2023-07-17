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
		public Sprite TransparentCard;
		public Sprite Avatar;
		public Sprite CharacterSprite;
		public Sprite CharacterGachaArt;
		[TextArea]
		public string PassiveDescription;
		public string AbilityName;
		[TextArea]
		public string AbilityDescription;
		public float AbilityCooldown;
		public CharacterSkillBase AbilityPrefab;
		public Sprite AbilityIcon;
		public bool IsActive;
		public Color ColorTheme;
		public int StarRating;
		public WeaponBase StartingWeapon;
		public bool IsPullable;
		public PlayerStats Stats;
		public List<EidolonData> Eidolons;
		public List<CharacterLoreEntry> loreEntries;
		public bool UseSpecialBar;
		public float SpecialMaxValue;
		public float SpecialIncrementValue;

		public void Activate()
		{
			IsActive = true;
		}

		public void Deactivate()
		{
			IsActive = false;
		}

		public IEnumerable<StatsDisplayData> GetStatsList()
		{
			return Stats.GetStatsList();
		}
	}
}