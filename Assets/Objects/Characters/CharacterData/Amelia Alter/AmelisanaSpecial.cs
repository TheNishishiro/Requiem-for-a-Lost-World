using System.Collections;
using Managers;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Amelia_Alter
{
    public class AmelisanaSpecial : CharacterSkillBase
    {
        private float _timeUntilBindConsume = 10f;
        
        public void Update()
        {
            _timeUntilBindConsume -= Time.deltaTime;
            if (_timeUntilBindConsume > 0 || GameManager.IsCharacterState(PlayerCharacterState.Amelisana_Ultimate)) return;

            var consumePercentage = GameData.IsCharacterRank(CharacterRank.E5) ? 0.15f : 0.1f;
            var specialConsumeAmount = PlayerStatsScaler.GetScaler().GetSpecialValue() * consumePercentage;
            PlayerStatsScaler.GetScaler().IncrementSpecial(-specialConsumeAmount);

            var buffDuration = GameData.IsCharacterRank(CharacterRank.E3) ? 7f : 5f;
            GameManager.instance.playerStatsComponent.TemporaryStatBoost(StatEnum.Damage, specialConsumeAmount*0.5f,  buffDuration);
            if (GameData.IsCharacterRank(CharacterRank.E5))
                StartCoroutine(RecoverSpecialWithDelay(buffDuration));
            
            GameManager.instance.playerVfxComponent.PlayConsumeBindingTransform();
            _timeUntilBindConsume = 10f;
        }

        private IEnumerator RecoverSpecialWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            var scaler = PlayerStatsScaler.GetScaler();
            scaler.IncrementSpecial(scaler.GetMaxHealth() * 0.1f);
        }
    }
}