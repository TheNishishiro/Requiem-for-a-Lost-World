using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Objects.Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class CharacterListManager : MonoBehaviour
	{
		public static CharacterListManager instance;
		private SaveFile _saveFile;
		
		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
			if (instance == null)
				instance = this;
		}
		
		[SerializeField] private List<CharacterData> characters;
		public List<CharacterData> GetCharacters()
		{
			return characters;
		}

		public CharacterData GetActiveCharacter()
		{
			return characters.FirstOrDefault(x => x.Id == (_saveFile.SelectedCharacterId ?? CharactersEnum.Nishi));
		}

		public CharacterSaveData GetActiveCharacterData()
		{
			var character = GetActiveCharacter();
			return _saveFile.GetCharacterSaveData(character.Id);
		}
		
		public int GetCharactersCount()
		{
			return characters.Count;
		}
		
		public CharactersEnum GetCharacterIdByIndex(int index)
		{
			return characters[index].Id;
		}
		
		public int GetCharacterIndexById(CharactersEnum characterId)
		{
			return characters.FindIndex(x => x.Id == characterId);
		}

		public CharacterData GetRandomCharacter()
		{
			return characters.OrderBy(x => Random.value).FirstOrDefault();
		}

		public CharacterData GetRandomUnlockedCharacter()
		{
			var characterId = _saveFile.CharacterSaveData
				.Where(x => x.Value.IsUnlocked)
				.OrderBy(x => Random.value)
				.Select(x => x.Key)
				.FirstOrDefault();
			
			return characters.Find(x => x.Id == characterId);
		}

		public CharacterData GetCharacter(CharactersEnum characterId)
		{
			return characters.FirstOrDefault(x => x.Id == characterId);
		}

		public CharacterRank GetActiveRankCharacter()
		{
			var activeCharacterId = GetActiveCharacter().Id;
			if (_saveFile.CharacterSaveData.ContainsKey(activeCharacterId))
				return (CharacterRank) _saveFile.CharacterSaveData[activeCharacterId].RankUpLevel;
			return CharacterRank.E0;
		}
	}
}