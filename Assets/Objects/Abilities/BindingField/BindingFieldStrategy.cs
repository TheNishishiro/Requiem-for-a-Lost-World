using Data.Elements;
using Managers;
using Managers.StageEvents;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.BindingField
{
    public class BindingFieldStrategy : WeaponStatsStrategyBase
    {
        public BindingFieldStrategy(WeaponBase weapon) : base(weapon.weaponStats, weapon.ElementField)
        {
        }

        protected override float GetCooldownReductionPercentage()
        {
            if (GameData.IsCharacter(CharactersEnum.Amelisana_BoN))
            {
                var gameTime = GameManager.instance.GetGameTimeInMinutes();
                if (gameTime > 6)
                    return base.GetCooldownReductionPercentage();
                
                return base.GetCooldownReductionPercentage() - Mathf.Lerp(0.5f, 0f, gameTime/6f);
            }
            
            return base.GetCooldownReductionPercentage();
        }
    }
}