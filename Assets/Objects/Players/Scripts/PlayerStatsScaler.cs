using System.Collections.Generic;
using Interfaces;
using Objects.Characters;
using Objects.Characters.David;
using Objects.Characters.Truzi;
using Objects.Stage;

namespace Objects.Players.Scripts
{
    public static class PlayerStatsScaler
    {
        private static readonly Dictionary<CharactersEnum, CharacterScalingStrategyBase> _characterScalingStrategies = new()
        {
            { CharactersEnum.David_BoF, new DavidScalingStrategy() },
            { CharactersEnum.Truzi_BoT, new TruziScalingStrategy() }
        };

        private static CharacterScalingStrategyBase _defaultScaler = new ();

        public static CharacterScalingStrategyBase GetScaler()
        {
            var currentCharacter = GameData.GetPlayerCharacterId();
            return _characterScalingStrategies.GetValueOrDefault(currentCharacter, _defaultScaler);
        }
    }
}