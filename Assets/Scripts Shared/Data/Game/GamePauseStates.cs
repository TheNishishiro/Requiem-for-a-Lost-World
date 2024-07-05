using System;

namespace DefaultNamespace.Data.Game
{
    [Flags]
    public enum GamePauseStates
    {
        None = 0,
        PauseWeaponAttacks = 1 << 0,
        PauseTimer = 1 << 1,
        PauseEnemySpawner = 1 << 2,
        PausePlayerMovement = 1 << 3,
        PauseEnemyMovement = 1 << 4,
    }
}