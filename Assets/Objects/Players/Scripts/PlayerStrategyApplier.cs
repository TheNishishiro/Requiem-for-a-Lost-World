using System;
using System.Collections.Generic;
using Interfaces;
using Objects.Characters;
using Objects.Characters.Adam;
using Objects.Characters.Alice;
using Objects.Characters.Amelia;
using Objects.Characters.Amelia_BoD;
using Objects.Characters.Arika;
using Objects.Characters.Chitose;
using Objects.Characters.Corina_Alter;
using Objects.Characters.David;
using Objects.Characters.Maid;
using Objects.Characters.Natalie;
using Objects.Characters.Nishi;
using Objects.Characters.Oana;
using Objects.Characters.Summer;

namespace Objects.Players.Scripts
{
	public class PlayerStrategyApplier
	{
		private readonly Dictionary<CharactersEnum, ICharacterStrategy> _characterStrategies = new()
		{
			{ CharactersEnum.Chitose, new ChitoseStrategy() },
			{ CharactersEnum.Amelia, new AmeliaStrategy() },
			{ CharactersEnum.Amelia_BoD, new AmeliaBoDStrategy() },
			{ CharactersEnum.Arika_BoV, new ArikaStrategy() },
			{ CharactersEnum.Corina_BoB, new CorinaStrategy() },
			{ CharactersEnum.David_BoF, new DavidStrategy() },
			{ CharactersEnum.Maid, new ElizaStrategy() },
			{ CharactersEnum.Nishi, new NishiStrategy() },
			{ CharactersEnum.Natalie_BoW, new NatalieStrategy() },
			{ CharactersEnum.Summer, new SummerStrategy() },
			{ CharactersEnum.Adam_OBoV, new AdamStrategy() },
			{ CharactersEnum.Oana_BoI, new OanaStrategy() },
			{ CharactersEnum.Alice_BoL, new AliceStrategy() },
		};

		public void ApplyRankStrategy(CharactersEnum characterId, CharacterRank characterRank, PlayerStats playerStats)
		{
			if (_characterStrategies.TryGetValue(characterId, out var characterStrategy))
			{
				characterStrategy.ApplyRank(playerStats, characterRank);
			}
		}

		public void ApplyLevelUpStrategy(CharactersEnum characterId, CharacterRank characterRank, int currentLevel, PlayerStatsComponent playerStatsComponent)
		{
			if (_characterStrategies.TryGetValue(characterId, out var characterStrategy))
			{
				characterStrategy.ApplyLevelUp(characterRank, currentLevel, playerStatsComponent);
			}
		}
	}
}