using System;
using Events.Handlers;
using Events.Scripts;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UnityEngine;

namespace Objects.Players.Scripts
{
    public class PlayerPassiveComponent : MonoBehaviour, IDamageTakenHandler
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private PlayerStatsComponent playerStatsComponent;
        
        private void OnEnable()
        {
            DamageTakenEvent.Register(this);
        }

        private void OnDisable()
        {
            DamageTakenEvent.Unregister(this);
        }

        public void OnDamageTaken(float damage)
        {
            switch (GameData.GetPlayerCharacterId())
            {
                case CharactersEnum.David_BoF:
                    switch (damage)
                    {
                        case > 0:
                            playerStatsComponent.TemporaryStatBoost("david_hploss_critbuff", StatEnum.CritRate, 0.5f, 2);
                            playerStatsComponent.TemporaryStatBoost("david_hploss_damagebuff", StatEnum.DamagePercentageIncrease, 0.5f, 2);
                            break;
                        case <= -1:
                            playerStatsComponent.TemporaryStatBoost("david_hpgain_critbuff", StatEnum.CritDamage, 1.5f, 2);
                            playerStatsComponent.TemporaryStatBoost("david_hpgain_damagebuff", StatEnum.DamagePercentageIncrease, 0.5f, 2);
                            break;
                    }
                    
                    break;
            }
        }
    }
}