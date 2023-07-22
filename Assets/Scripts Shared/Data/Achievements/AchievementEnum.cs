
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
		[AchievementValue("Kill a 1000 enemies in one game")]
		Kill1000EnemiesInOneGame,
		[AchievementValue("Lucky draw (I)")]
		HaveFirst5StarWeapon,
		[AchievementValue("Lucky draw (II)")]
		HaveFirst5StarItem,
		[AchievementValue("Who needs a lucky clover?")]
		Obtain10HighRarityItems,
		[AchievementValue("Sometimes death is just the beginning")]
		DieOnce,
		[AchievementValue("Be healthy on your adventures")]
		Heal1000HealthInOneGame,
		[AchievementValue("Sticks and stones can break my bones...")]
		Take1000DamageInOneGame,
		[AchievementValue("And he welcomed her as his old friend")]
		Die20Times,
		
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
		[AchievementValue("Survive 15 minutes with Bearer of Dream")]
		Survive15MinutesWithAmelia_BoD,
		[AchievementValue("Survive 30 minutes with Bearer of Dream")]
		Survive30MinutesWithAmelia_BoD,

		#endregion
		
		#region Obtain characters
		
		[AchievementValue("Obtain Corina")]
        ObtainCorina_E0,
        [AchievementValue("Unlock final shard of Corina")]
        ObtainCorina_E5,
        [AchievementValue("Obtain Lucy")]
        ObtainLucy_E0,
        [AchievementValue("Unlock final shard of Lucy")]
        ObtainLucy_E5,
        [AchievementValue("Obtain Amelia")]
        ObtainAmelia_E0,
        [AchievementValue("Unlock final shard of Amelia")]
        ObtainAmelia_E5,
        [AchievementValue("Obtain David")]
        ObtainDavid_E0,
        [AchievementValue("Unlock final shard of David")]
        ObtainDavid_E5,
        [AchievementValue("Obtain Oana")]
        ObtainOana_E0,
        [AchievementValue("Unlock final shard of Oana")]
        ObtainOana_E5,
        [AchievementValue("Obtain Alice")]
        ObtainAlice_E0,
        [AchievementValue("Unlock final shard of Alice")]
        ObtainAlice_E5,
        [AchievementValue("Obtain Amelisana")]
        ObtainAmelisana_E0,
        [AchievementValue("Unlock final shard of Amelisana")]
        ObtainAmelisana_E5,
        [AchievementValue("Obtain Chornastra")]
        ObtainChornastra_E0,
        [AchievementValue("Unlock final shard of Chronastra")]
        ObtainChornastra_E5,
        [AchievementValue("Obtain Truzi")]
        ObtainTruzi_E0,
        [AchievementValue("Unlock final shard of Truzi")]
        ObtainTruzi_E5,
        [AchievementValue("Obtain Arika")]
        ObtainArika_E0,
        [AchievementValue("Unlock final shard of Arika")]
        ObtainArika_E5,
        [AchievementValue("Obtain Natalie")]
        ObtainNatalie_E0,
        [AchievementValue("Unlock final shard of Natalie")]
        ObtainNatalie_E5,
        [AchievementValue("Obtain Chitose")]
        ObtainChitose_E0,
        [AchievementValue("Unlock final shard of Chitose")]
        ObtainChitose_E5,
        [AchievementValue("Obtain Eliza")]
        ObtainEliza_E0,
        [AchievementValue("Unlock final shard of Eliza")]
        ObtainEliza_E5,
        [AchievementValue("Obtain Arthur")]
        ObtainArthur_E0,
        [AchievementValue("Unlock final shard of Arthur")]
        ObtainArthur_E5,
		
		#endregion

	}
}