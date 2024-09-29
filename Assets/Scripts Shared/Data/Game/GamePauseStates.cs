using System;

namespace DefaultNamespace.Data.Game
{
    [Flags]
    public enum GamePauseStates
    {
        PauseWeaponAttacks,
        PauseTimer,
        PauseEnemySpawner,
        PausePlayerMovement,
        PauseEnemyMovement,
    }
}