namespace Data.Elements
{
	public enum ElementalReaction
	{
		None,
		Melt, // Increase damage taken by 50% for 2 seconds
		Explosion, // Take damage equal to 35% of damage taken
		Swirl, // Shred resistance to swirled element by 10%
		Collapse, // Reduce all resistances by 10%
		Shock, // TODO: Paralyze enemy for 0.5s
		Erode // Increase damage taken by 10% for 1s and instantly deal 5% of current HP as damage
	}
}