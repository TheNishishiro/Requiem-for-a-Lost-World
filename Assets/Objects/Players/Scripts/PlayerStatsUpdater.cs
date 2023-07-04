using System;
using System.Collections.Generic;
using Interfaces;
using Objects.Characters;
using Objects.Characters.Amelia;
using Objects.Characters.Chitose;

namespace Objects.Players.Scripts
{
	public class PlayerStatsUpdater
	{
		private readonly Dictionary<CharactersEnum, ICharacterStrategy> _characterStrategies = new()
		{
			{ CharactersEnum.Chitose, new ChitoseStrategy() },
			{ CharactersEnum.Amelia_BoD, new AmeliaStrategy() }
		};

		public void ApplyStrategy(CharactersEnum characterId, CharacterRank characterRank, PlayerStats playerStats)
		{
			if (_characterStrategies.TryGetValue(characterId, out var characterStrategy))
			{
				characterStrategy.Apply(playerStats, characterRank);
			}
		}
	}
}