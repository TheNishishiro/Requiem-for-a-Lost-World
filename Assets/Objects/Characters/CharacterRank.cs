using DefaultNamespace.Attributes;

namespace Objects.Characters
{
	public enum CharacterRank
	{
		[StringValue("S")]
		E0 = 0,
		[StringValue("S<sub>I</sub>")]
		E1 = 1,
		[StringValue("SS")]
		E2 = 2,
		[StringValue("SS<sub>I</sub>")]
		E3 = 3,
		[StringValue("SS<sub>II</sub>")]
		E4 = 4,
		[StringValue("SSS")]
		E5 = 5
	}
}