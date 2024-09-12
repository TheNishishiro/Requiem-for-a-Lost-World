using System.Collections.Generic;
using Interfaces;
using Objects.Characters;
using Objects.Characters.Adam;
using Objects.Characters.Amelia_Alter;
using Objects.Characters.David;
using Objects.Characters.Nishi_HoF;
using Objects.Characters.Truzi;
using Objects.Characters.Yami_no_Tokiya;
using Objects.Stage;

namespace Objects.Players.Scripts
{
    public static class PlayerStatsScaler
    {
        private static readonly Dictionary<CharactersEnum, CharacterScalingStrategyBase> _characterScalingStrategies = new()
        {
            { CharactersEnum.David_BoF, new DavidScalingStrategy() },
            { CharactersEnum.Truzi_BoT, new TruziScalingStrategy() },
            { CharactersEnum.Nishi_HoF, new NishiHofScalingStrategy() },
            { CharactersEnum.Amelisana_BoN, new AmelisanaScalingStrategy() },
            { CharactersEnum.Chornastra_BoR, new YamiScalingStrategy() },
            { CharactersEnum.Adam_OBoV, new AdamScalingStrategy() }
        };

        private static CharacterScalingStrategyBase _defaultScaler = new ();

        public static CharacterScalingStrategyBase GetScaler()
        {
            var currentCharacter = GameData.GetPlayerCharacterId();
            return _characterScalingStrategies.GetValueOrDefault(currentCharacter, _defaultScaler);
        }
    }
}