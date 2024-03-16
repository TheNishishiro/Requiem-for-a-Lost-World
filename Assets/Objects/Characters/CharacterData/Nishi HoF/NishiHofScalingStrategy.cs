using System.Linq;
using Data.Elements;
using Managers;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Nishi_HoF
{
    public class NishiHofScalingStrategy : CharacterScalingStrategyBase
    {
        public override float GetDamageIncreasePercentage()
        {
            if (!GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Flame_State))
                return base.GetDamageIncreasePercentage();
            return base.GetDamageIncreasePercentage() + ((GetMaxHealth() - GetHealth()) * 0.005f);
        }

        public override float GetFireDamageIncrease()
        {
            if (!GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Flame_State) || !GameData.IsCharacterRank(CharacterRank.E1))
                return base.GetFireDamageIncrease();
            
            var damageIncrease = WeaponManager.instance.GetUnlockedWeaponsAsInterface().Count(x => x.ElementField == Element.Fire) * 0.1f;
            return base.GetFireDamageIncrease() + 0.25f + damageIncrease;
        }

        public override float GetCosmicDamageIncrease()
        {
            if (!GameManager.IsCharacterState(PlayerCharacterState.Nishi_HoF_Void_State))
                return base.GetCosmicDamageIncrease();
            if (!GameData.IsCharacterRank(CharacterRank.E1))
                return base.GetCosmicDamageIncrease() + 0.25f;
            
            var damageIncrease = WeaponManager.instance.GetUnlockedWeaponsAsInterface().Count(x => x.ElementField == Element.Cosmic) * 0.30f;
            return base.GetCosmicDamageIncrease() + 0.25f + damageIncrease;
        }
    }
}