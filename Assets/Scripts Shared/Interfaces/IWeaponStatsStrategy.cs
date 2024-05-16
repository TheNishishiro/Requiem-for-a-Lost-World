using DefaultNamespace.Data.Weapons;

namespace Interfaces
{
    public interface IWeaponStatsStrategy
    {
        DamageResult GetDamageDealt(float damageIncrease = 0, float flatDamageIncrease = 0);
        float GetTotalCooldown();
        float GetDuplicateSpawnDelay();
        int GetAttackCount();
        float GetDamageCooldown();
        float GetDamageOverTime(float damageIncrease = 0);
        float GetDamageOverTimeFrequency();
        float GetDamageOverTimeDuration();
        float GetWeakness();
        float GetSpeed();
        float GetDamage();
        float GetScale();
        float GetTotalTimeToLive();
        float GetHealPerHit(bool allowCrit);
        double GetDetectionRange();
        int GetPassThroughCount();
        float GetLifeSteal();
    }
}