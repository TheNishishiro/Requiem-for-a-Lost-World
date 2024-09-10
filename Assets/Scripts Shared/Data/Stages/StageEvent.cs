using DefaultNamespace.Attributes;

namespace DefaultNamespace.Data.Stages
{
    public enum StageEvent
    {
        [StringValue("Random spawn rate")]
        SpawnRate,
        [StringValue("Random enemy health multiplier")]
        Health,
        [StringValue("Random enemy damage multiplier")]
        Damage,
        [StringValue("Erase all enemies")]
        EraseEnemies,
        [StringValue("Random enemy count")]
        EnemyMinCount
    }
}