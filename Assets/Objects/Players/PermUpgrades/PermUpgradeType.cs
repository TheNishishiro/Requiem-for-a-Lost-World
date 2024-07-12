namespace Objects.Players.PermUpgrades
{
	public enum PermUpgradeType
	{
		[StatType(true)]
		Magnet = 1,
		MovementSpeed = 2,
		AttackCount = 3,
		[StatType(true)]
		HealthRegen = 4,
		[StatType(true)]
		DamageIncrease = 5,
		Armor = 6,
		[StatType(true)]
		CooldownReduction = 7,
		Health = 8,
		[StatType(true)]
		Curse = 9,
		Revival = 10,
		[StatType(true)]
		CritRate = 11,
		[StatType(true)]
		CritDamage = 12,
		[StatType(true)]
		AreaIncrease = 13,
		[StatType(true)]
		ProjectileSpeed = 14,
		[StatType(true)]
		EffectDuration = 15,
		[StatType(true)]
		ExpIncrease = 16,
		[StatType(true)]
		Greed = 17,
		[StatType(true)]
		Luck = 18,
		Reroll = 19,
		Skip = 20,
		[StatType(true)]
		ElementalReactionEffectIncreasePercentage = 21,
		[StatType(true)]
		FollowUpDamageIncrease
	}
}