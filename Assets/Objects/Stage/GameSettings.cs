using System;

namespace Objects.Stage
{
    public static class GameSettings
    {
        public static bool IsShortPlayTime { get; set; }
        public static float EnemyHealthModifier => IsShortPlayTime ? 1.4f : 1f;
        public static float EnemyDamageModifier => IsShortPlayTime ? 2f : 1f;
        public static float EnemySpawnRateModifier => IsShortPlayTime ? 0.75f : 1f;
        public static float ExpDropModifier => IsShortPlayTime ? 1.35f : 1f;
    }
}