namespace Objects.Players.PermUpgrades
{
    public enum StatEnum
    {
        Health,
        [Stat(StatCategory.Defensive, "HP", "Health")]
        HealthMax,
        [Stat(StatCategory.Utility, "SP", "Special")]
        SpecialMax,
        [Stat(StatCategory.Utility, "SPG", "Special gain")]
        SpecialIncrease,
        [Stat(StatCategory.Utility, "MAG", "Magnet")]
        MagnetSize,
        [Stat(StatCategory.Utility, "CDR", "Cooldown reduction")]
        CooldownReduction,
        [Stat(StatCategory.Utility, "CDR", "Cooldown reduction")]
        [StatType(true)]
        CooldownReductionPercentage,
        [Stat(StatCategory.Utility, "AC", "Attack count")]
        AttackCount,
        [Stat(StatCategory.Offensive, "ATK", "Attack")]
        Damage,
        [Stat(StatCategory.Utility, "SC", "Scale")]
        [StatType(true)]
        Scale,
        [Stat(StatCategory.Utility, "PS", "Projectile speed")]
        [StatType(true)]
        Speed,
        [Stat(StatCategory.Utility, "PLT", "Attack time")]
        TimeToLive,
        [Stat(StatCategory.Utility, "DTR", "Detection range")]
        DetectionRange,
        [Stat(StatCategory.Offensive, "ATK", "Attack")]
        [StatType(true)]
        DamagePercentageIncrease,
        [Stat(StatCategory.Utility, "EXP", "Experience gain")]
        [StatType(true)]
        ExperienceIncreasePercentage,
        [Stat(StatCategory.Utility, "MS", "Movement speed")]
        MovementSpeed,
        [Stat(StatCategory.Utility, "SCDR", "Skill cooldown reduction")]
        [StatType(true)]
        SkillCooldownReductionPercentage,
        [Stat(StatCategory.Defensive, "HPR", "Health regen")]
        HealthRegen,
        [Stat(StatCategory.Offensive, "CR", "Crit rate")]
        [StatType(true)]
        CritRate,
        [Stat(StatCategory.Offensive, "CD", "Crit damage")]
        [StatType(true)]
        CritDamage,
        [Stat(StatCategory.Offensive, "HC", "Attack pierce count")]
        PassThroughCount,
        [Stat(StatCategory.Defensive, "ARM", "Armor")]
        [StatType(true)]
        Armor,
        [Stat(StatCategory.Utility, "ESP", "Enemy speed")]
        [StatType(true)]
        EnemySpeedIncreasePercentage,
        [Stat(StatCategory.Utility, "EDMG", "Enemy spawn rate")]
        [StatType(true)]
        EnemySpawnRateIncreasePercentage,
        [Stat(StatCategory.Utility, "EHP", "Enemy health")]
        [StatType(true)]
        EnemyHealthIncreasePercentage,
        [Stat(StatCategory.Utility, "EC", "Enemy count")]
        [StatType(true)]
        EnemyMaxCountIncreasePercentage,
        [Stat(StatCategory.Utility, "GRD", "Item rewards")]
        [StatType(true)]
        ItemRewardIncrease,
        [Stat(StatCategory.Defensive, "REV", "Revive")]
        Revives,
        [Stat(StatCategory.Utility, "PLT", "Attack time")]
        [StatType(true)]
        ProjectileLifeTimeIncreasePercentage,
        [Stat(StatCategory.Defensive, "DOG", "Dodge chance")]
        [StatType(true)]
        DodgeChance,
        [Stat(StatCategory.Utility, "DMT", "Damage taken")]
        [StatType(true)]
        DamageTakenIncreasePercentage,
        [Stat(StatCategory.Defensive, "HLI", "Healing")]
        [StatType(true)]
        HealingIncreasePercentage,
        [Stat(StatCategory.Utility, "LK", "Luck")]
        [StatType(true)]
        Luck,
        [Stat(StatCategory.Offensive, "DOT", "DoT")]
        DamageOverTime,
        [Stat(StatCategory.Utility, "RL", "Re-roll")]
        Rerolls,
        [Stat(StatCategory.Utility, "SKP", "Skip")]
        Skips,
        [Stat(StatCategory.Offensive, "DOT", "DoT frequency")]
        [StatType(true)]
        DamageOverTimeFrequencyReduction,
        [Stat(StatCategory.Offensive, "DOTD", "DoT duration")]
        [StatType(true)]
        DamageOverTimeDurationIncrease,
        [Stat(StatCategory.Defensive, "LS", "Life steal")]
        [StatType(true)]
        LifeSteal,
        [Stat(StatCategory.Offensive, "FDMG", "Fire damage")]
        [StatType(true)]
        FireDamageIncrease,
        [Stat(StatCategory.Offensive, "LDMG", "Lightning damage")]
        [StatType(true)]
        LightningDamageIncrease,
        [Stat(StatCategory.Offensive, "IDMG", "Ice damage")]
        [StatType(true)]
        IceDamageIncrease,
        [Stat(StatCategory.Offensive, "PDMG", "Physical damage")]
        [StatType(true)]
        PhysicalDamageIncrease,
        [Stat(StatCategory.Offensive, "WDMG", "Wind damage")]
        [StatType(true)]
        WindDamageIncrease,
        [Stat(StatCategory.Offensive, "LDMG", "Light damage")]
        [StatType(true)]
        LightDamageIncrease,
        [Stat(StatCategory.Offensive, "CDMG", "Cosmic damage")]
        [StatType(true)]
        CosmicDamageIncrease,
        [Stat(StatCategory.Offensive, "EDMG", "Earth damage")]
        [StatType(true)]
        EarthDamageIncrease,
        [Stat(StatCategory.Offensive, "STM", "Stamina")]
        StaminaIncrease
    }
}