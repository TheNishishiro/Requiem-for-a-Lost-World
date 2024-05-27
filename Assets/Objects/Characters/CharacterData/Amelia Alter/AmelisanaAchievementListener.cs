using System;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Weapons;

namespace Objects.Characters.Amelia_Alter
{
    public class AmelisanaAchievementListener : CharacterAchievementListener, IEnemyImmobileHandler, IWeaponUnlockedHandler, ISkillUsedHandler, IGameFinishedHandler
    {
        private float _bindTime;
        private bool _isLight;
        private bool _isCosmic;
        private bool _isAnyOtherElement;
        private int _weaponCount;
        private bool _isSkillUsed;
        
        private void OnEnable()
        {
            EnemyImmobileEvent.Register(this);
            WeaponUnlockedEvent.Register(this);
            SkillUsedEvent.Register(this);
            GameFinishedEvent.Register(this);
        }

        private void OnDisable()
        {
            EnemyImmobileEvent.Unregister(this);
            WeaponUnlockedEvent.Unregister(this);
            SkillUsedEvent.Unregister(this);
            GameFinishedEvent.Unregister(this);
        }

        public void OnEnemyStunned(float time)
        {
            _bindTime += time;
            if (_bindTime >= 60*60*60)
                SaveFile.Instance.UnlockAchievement(AchievementEnum.Amelisana_EnemyBindTime);
        }

        public void OnWeaponUnlocked(WeaponBase weapon, int rarity)
        {
            if (weapon.ElementField == Element.Light && !_isLight)
                _isLight = true;
            if (weapon.ElementField == Element.Cosmic && !_isCosmic)
                _isCosmic = true;
            if (weapon.ElementField != Element.Cosmic && weapon.ElementField != Element.Light && !_isAnyOtherElement)
                _isAnyOtherElement = true;
            _weaponCount++;
            
            if (_weaponCount >= 6 && !_isAnyOtherElement && _isLight && _isCosmic)
                SaveFile.Instance.UnlockAchievement(AchievementEnum.Amelisana_LightAndCosmicOnly);
        }

        public void OnSkillUsed()
        {
            _isSkillUsed = true;
        }

        public void OnGameFinished(bool isWin)
        {
            if (isWin && !_isSkillUsed)
                SaveFile.Instance.UnlockAchievement(AchievementEnum.Amelisana_FinishWithoutUsingUltimate);
        }
    }
}