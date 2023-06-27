
namespace DefaultNamespace.Data.Achievements
{
	public enum AchievementEnum
	{
		[AchievementValue("Jack of all trades")]
		Hold6Weapons,
		[AchievementValue("Have a bunch of handy tools")]
		Hold6Items,
		[AchievementValue("Let the bodies hit the floor")]
		Kill100000Enemies,
		[AchievementValue("In the realm of green, march forward and reveal the coveted cartographic guide")]
		CollectMap,
		[AchievementValue("Summon shards of tormented memories")]
		CollectMagnet,
		[AchievementValue("Can't resist the shiny, can you?")]
		Collect1000Pickups,
		[AchievementValue("Summon a character from the past")]
		PerformGacha,
		[AchievementValue("Succumb to gambling addiction")]
		PerformGacha100Times,
		
		#region Survive with characters

		[AchievementValue("Survive 15 minutes with Corina")]
		Survive15MinutesWithCorina,
		[AchievementValue("Survive 30 minutes with Corina")]
		Survive30MinutesWithCorina,
		[AchievementValue("Survive 15 minutes with Lucy")]
		Survive15MinutesWithLucy,
		[AchievementValue("Survive 30 minutes with Lucy")]
		Survive30MinutesWithLucy,
		[AchievementValue("Survive 15 minutes with Amelia")]
		Survive15MinutesWithAmelia,
		[AchievementValue("Survive 30 minutes with Amelia")]
		Survive30MinutesWithAmelia,
		[AchievementValue("Survive 15 minutes with David")]
		Survive15MinutesWithDavid,
		[AchievementValue("Survive 30 minutes with David")]
		Survive30MinutesWithDavid,
		[AchievementValue("Survive 15 minutes with Oana")]
		Survive15MinutesWithOana,
		[AchievementValue("Survive 30 minutes with Oana")]
		Survive30MinutesWithOana,
		[AchievementValue("Survive 15 minutes with Alice")]
		Survive15MinutesWithAlice,
		[AchievementValue("Survive 30 minutes with Alice")]
		Survive30MinutesWithAlice,
		[AchievementValue("Survive 15 minutes with Amelisana")]
		Survive15MinutesWithAmelisana,
		[AchievementValue("Survive 30 minutes with Amelisana")]
		Survive30MinutesWithAmelisana,
		[AchievementValue("Survive 15 minutes with Chronastra")]
		Survive15MinutesWithChornastra,
		[AchievementValue("Survive 30 minutes with Chronastra")]
		Survive30MinutesWithChornastra,
		[AchievementValue("Survive 15 minutes with Truzi")]
		Survive15MinutesWithTruzi,
		[AchievementValue("Survive 30 minutes with Truzi")]
		Survive30MinutesWithTruzi,
		[AchievementValue("Survive 15 minutes with Arika")]
		Survive15MinutesWithArika,
		[AchievementValue("Survive 30 minutes with Arika")]
		Survive30MinutesWithArika,
		[AchievementValue("Survive 15 minutes with Natalie")]
		Survive15MinutesWithNatalie,
		[AchievementValue("Survive 30 minutes with Natalie")]
		Survive30MinutesWithNatalie,
		[AchievementValue("Survive 15 minutes with Chitose")]
		Survive15MinutesWithChitose,
		[AchievementValue("Survive 30 minutes with Chitose")]
		Survive30MinutesWithChitose,
		[AchievementValue("Survive 15 minutes with Eliza")]
		Survive15MinutesWithEliza,
		[AchievementValue("Survive 30 minutes with Eliza")]
		Survive30MinutesWithEliza,
		[AchievementValue("Survive 15 minutes with Arthur")]
		Survive15MinutesWithArthur,
		[AchievementValue("Survive 30 minutes with Arthur")]
		Survive30MinutesWithArthur,

		#endregion
		
		#region Obtain characters
		
		[AchievementValue("Obtain Corina")]
		ObtainCorina_S,
		[AchievementValue("Rank up Corina to SS")]
		ObtainCorina_SS,
		[AchievementValue("Rank up Corina to SSS")]
		ObtainCorina_SSS,
		[AchievementValue("Obtain Lucy")]
		ObtainLucy_S,
		[AchievementValue("Rank up Lucy to SS")]
		ObtainLucy_SS,
		[AchievementValue("Rank up Lucy to SSS")]
		ObtainLucy_SSS,
		[AchievementValue("Obtain Amelia")]
		ObtainAmelia_S,
		[AchievementValue("Rank up Amelia to SS")]
		ObtainAmelia_SS,
		[AchievementValue("Rank up Amelia to SSS")]
		ObtainAmelia_SSS,
		[AchievementValue("Obtain David")]
		ObtainDavid_S,
		[AchievementValue("Rank up David to SS")]
		ObtainDavid_SS,
		[AchievementValue("Rank up David to SSS")]
		ObtainDavid_SSS,
		[AchievementValue("Obtain Oana")]
		ObtainOana_S,
		[AchievementValue("Rank up Oana to SS")]
		ObtainOana_SS,
		[AchievementValue("Rank up Oana to SSS")]
		ObtainOana_SSS,
		[AchievementValue("Obtain Alice")]
		ObtainAlice_S,
		[AchievementValue("Rank up Alice to SS")]
		ObtainAlice_SS,
		[AchievementValue("Rank up Alice to SSS")]
		ObtainAlice_SSS,
		[AchievementValue("Obtain Amelisana")]
		ObtainAmelisana_S,
		[AchievementValue("Rank up Amelisana to SS")]
		ObtainAmelisana_SS,
		[AchievementValue("Rank up Amelisana to SSS")]
		ObtainAmelisana_SSS,
		[AchievementValue("Obtain Chornastra")]
		ObtainChornastra_S,
		[AchievementValue("Rank up Chronastra to SS")]
		ObtainChornastra_SS,
		[AchievementValue("Rank up Chronastra to SSS")]
		ObtainChornastra_SSS,
		[AchievementValue("Obtain Truzi")]
		ObtainTruzi_S,
		[AchievementValue("Rank up Truzi to SS")]
		ObtainTruzi_SS,
		[AchievementValue("Rank up Truzi to SSS")]
		ObtainTruzi_SSS,
		[AchievementValue("Obtain Arika")]
		ObtainArika_S,
		[AchievementValue("Rank up Arika to SS")]
		ObtainArika_SS,
		[AchievementValue("Rank up Arika to SSS")]
		ObtainArika_SSS,
		[AchievementValue("Obtain Natalie")]
		ObtainNatalie_S,
		[AchievementValue("Rank up Natalie to SS")]
		ObtainNatalie_SS,
		[AchievementValue("Rank up Natalie to SSS")]
		ObtainNatalie_SSS,
		[AchievementValue("Rank up Chitose to SS")]
		ObtainChitose_SS,
		[AchievementValue("Rank up Chitose to SSS")]
		ObtainChitose_SSS,
		[AchievementValue("Obtain Eliza")]
		ObtainEliza_S,
		[AchievementValue("Rank up Eliza to SS")]
		ObtainEliza_SS,
		[AchievementValue("Rank up Eliza to SSS")]
		ObtainEliza_SSS,
		[AchievementValue("Obtain Arthur")]
		ObtainArthur_S,
		[AchievementValue("Rank up Arthur to SS")]
		ObtainArthur_SS,
		[AchievementValue("Rank up Arthur to SSS")]
		ObtainArthur_SSS,
		
		#endregion

	}
}