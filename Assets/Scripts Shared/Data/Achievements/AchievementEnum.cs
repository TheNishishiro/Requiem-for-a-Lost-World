
namespace DefaultNamespace.Data.Achievements
{
	public enum AchievementEnum
	{
		[AchievementValue("Jack of all trades", "Obtain 6 weapons in a single game")]
		Hold6Weapons,
		[AchievementValue("Have a bunch of handy tools", "Obtain 6 items in a single game")]
		Hold6Items,
		[AchievementValue("Let the bodies hit the floor", "Prove your worth by staining the ground with the blood of your enemies")]
		Kill100000Enemies,
		[AchievementValue("All seeing", "Find a map hidden in the in the Capital outskirts")]
		CollectMap,
		[AchievementValue("Master of shards", "Summon shards of fallen enemies")]
		CollectMagnet,
		[AchievementValue("Can't resist the shiny, can you?", "Collect 1000 pickups throughout your adventures")]
		Collect1000Pickups,
		[AchievementValue("Summon a character from the past", "Obtain a character shard")]
		PerformGacha,
		[AchievementValue("I'm not addicted", "Obtain 100 character shards")]
		PerformGacha100Times,
		[AchievementValue("That's it?", "Kill a 1000 enemies in a single expedition")]
		Kill1000EnemiesInOneGame,
		[AchievementValue("Lucky draw (I)", "Have your first weapon be of legendary rarity")]
		HaveFirst5StarWeapon,
		[AchievementValue("Lucky draw (II)", "Have your first item be of legendary rarity")]
		HaveFirst5StarItem,
		[AchievementValue("Who needs a lucky clover?", "Obtain 10 items of legendary rarity")]
		Obtain10HighRarityItems,
		[AchievementValue("Sometimes death is just the beginning", "Fall during an expedition once")]
		DieOnce,
		[AchievementValue("Be healthy on your adventures", "Heal a 1000 health in a single game")]
		Heal1000HealthInOneGame,
		[AchievementValue("Sticks and stones can break my bones...", "Take a total of 1000 damage in a single game")]
		Take1000DamageInOneGame,
		[AchievementValue("And he welcomed her as his old friend", "Have your expedition end prematurely 20 times")]
		Die20Times,
		
		#region Survive with characters

		[AchievementValue("Barely a warmup","Survive 15 minutes with Corina")]
		Survive15MinutesWithCorina,
		[AchievementValue("Taste the blood of strong", "Survive 30 minutes with Corina")]
		Survive30MinutesWithCorina,
		[AchievementValue("Corruption spreading", "Survive 15 minutes with Lucy")]
		Survive15MinutesWithLucy,
		[AchievementValue("I shall consume it all", "Survive 30 minutes with Lucy")]
		Survive30MinutesWithLucy,
		[AchievementValue("Beginning of my journey", "Survive 15 minutes with Amelia")]
		Survive15MinutesWithAmelia,
		[AchievementValue("Are you proud of me?", "Survive 30 minutes with Amelia")]
		Survive30MinutesWithAmelia,
		[AchievementValue("Relentless march", "Survive 15 minutes with David")]
		Survive15MinutesWithDavid,
		[AchievementValue("Scorch the earth", "Survive 30 minutes with David")]
		Survive30MinutesWithDavid,
		[AchievementValue("A walk in the park", "Survive 15 minutes with Oana")]
		Survive15MinutesWithOana,
		[AchievementValue("It's getting cold in here", "Survive 30 minutes with Oana")]
		Survive30MinutesWithOana,
		[AchievementValue("It's nothing", "Survive 15 minutes with Alice")]
		Survive15MinutesWithAlice,
		[AchievementValue("Just like old times", "Survive 30 minutes with Alice")]
		Survive30MinutesWithAlice,
		[AchievementValue("Not over yet", "Survive 15 minutes with Amelisana")]
		Survive15MinutesWithAmelisana,
		[AchievementValue("AHAHAHAHAHA", "Survive 30 minutes with Amelisana")]
		Survive30MinutesWithAmelisana,
		[AchievementValue("Puny mortals", "Survive 15 minutes with Chronastra")]
		Survive15MinutesWithChornastra,
		[AchievementValue("I am the end", "Survive 30 minutes with Chronastra")]
		Survive30MinutesWithChornastra,
		[AchievementValue("Is it over yet?", "Survive 15 minutes with Truzi")]
		Survive15MinutesWithTruzi,
		[AchievementValue("Time waits for no one", "Survive 30 minutes with Truzi")]
		Survive30MinutesWithTruzi,
		[AchievementValue("One step closer", "Survive 15 minutes with Arika")]
		Survive15MinutesWithArika,
		[AchievementValue("Transcendence", "Survive 30 minutes with Arika")]
		Survive30MinutesWithArika,
		[AchievementValue("The wind is my ally", "Survive 15 minutes with Natalie")]
		Survive15MinutesWithNatalie,
		[AchievementValue("Erode the land", "Survive 30 minutes with Natalie")]
		Survive30MinutesWithNatalie,
		[AchievementValue("First real expedition", "Survive 15 minutes with Chitose")]
		Survive15MinutesWithChitose,
		[AchievementValue("Can I be a knight yet?", "Survive 30 minutes with Chitose")]
		Survive30MinutesWithChitose,
		[AchievementValue("I'm not a maid!", "Survive 15 minutes with Eliza")]
		Survive15MinutesWithEliza,
		[AchievementValue("I'm still not a maid!", "Survive 30 minutes with Eliza")]
		Survive30MinutesWithEliza,
		[AchievementValue("When will they learn?", "Survive 15 minutes with Adam")]
		Survive15MinutesWithAdam,
		[AchievementValue("I am the end of all", "Survive 30 minutes with Adam")]
		Survive30MinutesWithAdam,
		[AchievementValue("Purge the evil", "Survive 15 minutes with Bearer of Dream")]
		Survive15MinutesWithAmelia_BoD,
		[AchievementValue("I will light the way!", "Survive 30 minutes with Bearer of Dream")]
		Survive30MinutesWithAmelia_BoD,
		[AchievementValue("Prove your worth", "Survive 15 minutes with Nishi")]
		Survive15MinutesWithNishi,
		[AchievementValue("I will not fail", "Survive 30 minutes with Nishi")]
		Survive30MinutesWithNishi,
		[AchievementValue("Arrow guide my way", "Survive 15 minutes with Summer")]
		Survive15MinutesWithSummer,
		[AchievementValue("I will protect you", "Survive 30 minutes with Summer")]
		Survive30MinutesWithSummer,

		#endregion
		
		#region Obtain characters
		
		[AchievementValue("Let the bloodshed begin", "Obtain Corina")]
        ObtainCorina_E0,
        [AchievementValue("The red moon rises once again", "Unlock final shard of Corina")]
        ObtainCorina_E5,
        [AchievementValue("Sleep tight little one", "Obtain Lucy")]
        ObtainLucy_E0,
        [AchievementValue("Decay escapes no one", "Unlock final shard of Lucy")]
        ObtainLucy_E5,
        [AchievementValue("Light of hope", "Obtain final shard of Amelia")]
        ObtainAmelia_E5,
        [AchievementValue("Soldiers! Onwards!", "Obtain David")]
        ObtainDavid_E0,
        [AchievementValue("Fire shall cleanse the land", "Unlock final shard of David")]
        ObtainDavid_E5,
        [AchievementValue("Glimpses of the past", "Obtain Oana")]
        ObtainOana_E0,
        [AchievementValue("Rest upon endless layers of ice", "Unlock final shard of Oana")]
        ObtainOana_E5,
        [AchievementValue("In need of a hero", "Obtain Alice")]
        ObtainAlice_E0,
        [AchievementValue("Thundering retribution", "Unlock final shard of Alice")]
        ObtainAlice_E5,
        [AchievementValue("Tainted dreams", "Obtain Amelisana")]
        ObtainAmelisana_E0,
        [AchievementValue("All shall suffer as I did", "Unlock final shard of Amelisana")]
        ObtainAmelisana_E5,
        [AchievementValue("The end is nigh", "Obtain Chronastra")]
        ObtainChornastra_E0,
        [AchievementValue("Requiem for a Lost World", "Unlock final shard of Chronastra")]
        ObtainChornastra_E5,
        [AchievementValue("Time waits for nobody", "Obtain Truzi")]
        ObtainTruzi_E0,
        [AchievementValue("Untapped potential", "Unlock final shard of Truzi")]
        ObtainTruzi_E5,
        [AchievementValue("Void consumes all", "Obtain Arika")]
        ObtainArika_E0,
        [AchievementValue("Promise of the end", "Unlock final shard of Arika")]
        ObtainArika_E5,
        [AchievementValue("Wind of peace", "Obtain Natalie")]
        ObtainNatalie_E0,
        [AchievementValue("The wind shall guide us", "Unlock final shard of Natalie")]
        ObtainNatalie_E5,
        [AchievementValue("Present!", "Obtain Chitose")]
        ObtainChitose_E0,
        [AchievementValue("The future is now", "Unlock final shard of Chitose")]
        ObtainChitose_E5,
        [AchievementValue("*Sigh* More work?", "Obtain Eliza")]
        ObtainEliza_E0,
        [AchievementValue("Vacation time!", "Unlock final shard of Eliza")]
        ObtainEliza_E5,
        [AchievementValue("Kneel before me!", "Obtain Adam")]
        ObtainAdam_E0,
        [AchievementValue("All shall know despair", "Unlock final shard of Adam")]
        ObtainAdam_E5,
        [AchievementValue("Looming darkness", "Obtain final shard of Nishi")]
        ObtainNishi_E5,
        [AchievementValue("I alone will forge my destiny", "Obtain Amelia")]
        ObtainAmelia_BoD_E0,
        [AchievementValue("Never falter, never surrender", "Unlock final shard of Amelia")]
        ObtainAmelia_BoD_E5,
        [AchievementValue("Arrow of novae", "Obtain Summer")]
        ObtainSummer_E0,
        [AchievementValue("The cutest of them all", "Unlock final shard of Summer")]
        ObtainSummer_E5,
		
		#endregion

	}
}