using System;

namespace Objects.Stage
{
    public static class GameSettings
    {
        public static bool IsShortPlayTime { get; set; }
        public static float EnemyHealthModifier => IsShortPlayTime ? 1.25f : 1f;
        public static float EnemyDamageModifier => IsShortPlayTime ? 1.25f : 1f;
        public static float EnemySpawnRateModifier => IsShortPlayTime ? 0.85f : 1f;
        public static float ExpDropModifier => IsShortPlayTime ? 1.5f : 1f;
        public static bool IsRandomLevelUp { get; set; }
    }
}