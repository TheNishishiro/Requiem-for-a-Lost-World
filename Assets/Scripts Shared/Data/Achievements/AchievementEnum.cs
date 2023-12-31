﻿
namespace DefaultNamespace.Data.Achievements
{
	public enum AchievementEnum // 85
	{
		[AchievementValue("Jack of all trades", "Obtain 6 weapons in a single game")]
		Hold6Weapons = 0,
		[AchievementValue("Have a bunch of handy tools", "Obtain 6 items in a single game")]
		Hold6Items = 1,
		[AchievementValue("Let the bodies hit the floor", "Prove your worth by staining the ground with the blood of your enemies")]
		Kill100000Enemies = 2,
		[AchievementValue("All seeing", "Find a map hidden in the in the Capital outskirts")]
		CollectMap = 3,
		[AchievementValue("Master of shards", "Summon shards of fallen enemies")]
		CollectMagnet = 4,
		[AchievementValue("What's in the crate?", "Collect 100 pickups throughout your adventures")]
		Collect100Pickups = 5,
		[AchievementValue("Can't resist the shiny, can you?", "Collect 1000 pickups throughout your adventures")]
		Collect1000Pickups = 6,
		[AchievementValue("Summon a character from the past", "Obtain a character shard")]
		PerformGacha = 7,
		[AchievementValue("I'm not addicted", "Obtain 100 character shards")]
		PerformGacha100Times = 8,
		[AchievementValue("That's it?", "Kill a 1000 enemies in a single expedition")]
		Kill1000EnemiesInOneGame = 9,
		[AchievementValue("Lucky draw (I)", "Have your first weapon be of legendary rarity")]
		HaveFirst5StarWeapon = 10,
		[AchievementValue("Lucky draw (II)", "Have your first item be of legendary rarity")]
		HaveFirst5StarItem = 11,
		[AchievementValue("Who needs a lucky clover?", "Obtain 10 items of legendary rarity")]
		Obtain10HighRarityItems = 12,
		[AchievementValue("Sometimes death is just the beginning", "Fall during an expedition once")]
		DieOnce = 13,
		[AchievementValue("Be healthy on your adventures", "Heal a 1000 health in a single game")]
		Heal1000HealthInOneGame = 14,
		[AchievementValue("Sticks and stones can break my bones...", "Take a total of 1000 damage in a single game")]
		Take1000DamageInOneGame = 15,
		[AchievementValue("And he welcomed her as his old friend", "Have your expedition end prematurely 20 times")]
		Die20Times = 16,
		[AchievementValue("Blessed by the wind", "Find a wind orb in the Capital Outskirts")]
		UnlockWindShear = 17,
		[AchievementValue("Witness Octi's ugliness", "Survive 30 minutes with a light aligned character")]
		Survive30MinutesWithLightCharacter = 84,
		[AchievementValue("Witness Octi's brilliance", "Survive 30 minutes with a dark aligned character")]
		Survive30MinutesWithDarkCharacter = 85,
		
		
		#region Survive with characters

		[AchievementValue("Barely a warmup","Survive 15 minutes with Corina")]
		Survive15MinutesWithCorina = 18,
		[AchievementValue("Taste the blood of strong", "Survive 30 minutes with Corina")]
		Survive30MinutesWithCorina = 19,
		[AchievementValue("Corruption spreading", "Survive 15 minutes with Lucy")]
		Survive15MinutesWithLucy = 20,
		[AchievementValue("I shall consume it all", "Survive 30 minutes with Lucy")]
		Survive30MinutesWithLucy = 21,
		[AchievementValue("Beginning of my journey", "Survive 15 minutes with Amelia")]
		Survive15MinutesWithAmelia = 22,
		[AchievementValue("Are you proud of me?", "Survive 30 minutes with Amelia")]
		Survive30MinutesWithAmelia = 23,
		[AchievementValue("Relentless march", "Survive 15 minutes with David")]
		Survive15MinutesWithDavid = 24,
		[AchievementValue("Scorch the earth", "Survive 30 minutes with David")]
		Survive30MinutesWithDavid = 25,
		[AchievementValue("A walk in the park", "Survive 15 minutes with Oana")]
		Survive15MinutesWithOana = 26,
		[AchievementValue("It's getting cold in here", "Survive 30 minutes with Oana")]
		Survive30MinutesWithOana = 27,
		[AchievementValue("It's nothing", "Survive 15 minutes with Alice")]
		Survive15MinutesWithAlice = 28,
		[AchievementValue("Just like old times", "Survive 30 minutes with Alice")]
		Survive30MinutesWithAlice = 29,
		[AchievementValue("Not over yet", "Survive 15 minutes with Amelisana")]
		Survive15MinutesWithAmelisana = 30,
		[AchievementValue("AHAHAHAHAHA", "Survive 30 minutes with Amelisana")]
		Survive30MinutesWithAmelisana = 31,
		[AchievementValue("Puny mortals", "Survive 15 minutes with Chronastra")]
		Survive15MinutesWithChornastra = 32,
		[AchievementValue("I am the end", "Survive 30 minutes with Chronastra")]
		Survive30MinutesWithChornastra = 33,
		[AchievementValue("Is it over yet?", "Survive 15 minutes with Truzi")]
		Survive15MinutesWithTruzi = 34,
		[AchievementValue("Time waits for no one", "Survive 30 minutes with Truzi")]
		Survive30MinutesWithTruzi = 35,
		[AchievementValue("One step closer", "Survive 15 minutes with Arika")]
		Survive15MinutesWithArika = 36,
		[AchievementValue("Transcendence", "Survive 30 minutes with Arika")]
		Survive30MinutesWithArika = 37,
		[AchievementValue("The wind is my ally", "Survive 15 minutes with Natalie")]
		Survive15MinutesWithNatalie = 38,
		[AchievementValue("Erode the land", "Survive 30 minutes with Natalie")]
		Survive30MinutesWithNatalie = 39,
		[AchievementValue("First real expedition", "Survive 15 minutes with Chitose")]
		Survive15MinutesWithChitose = 40,
		[AchievementValue("Can I be a knight yet?", "Survive 30 minutes with Chitose")]
		Survive30MinutesWithChitose = 41,
		[AchievementValue("I'm not a maid!", "Survive 15 minutes with Eliza")]
		Survive15MinutesWithEliza = 42,
		[AchievementValue("I'm still not a maid!", "Survive 30 minutes with Eliza")]
		Survive30MinutesWithEliza = 43,
		[AchievementValue("When will they learn?", "Survive 15 minutes with Adam")]
		Survive15MinutesWithAdam = 44,
		[AchievementValue("I am the end of all", "Survive 30 minutes with Adam")]
		Survive30MinutesWithAdam = 45,
		[AchievementValue("Purge the evil", "Survive 15 minutes with Bearer of Dream")]
		Survive15MinutesWithAmelia_BoD = 46,
		[AchievementValue("I will light the way!", "Survive 30 minutes with Bearer of Dream")]
		Survive30MinutesWithAmelia_BoD = 47,
		[AchievementValue("Prove your worth", "Survive 15 minutes with Nishi")]
		Survive15MinutesWithNishi = 48,
		[AchievementValue("I will not fail", "Survive 30 minutes with Nishi")]
		Survive30MinutesWithNishi = 49,
		[AchievementValue("Arrow guide my way", "Survive 15 minutes with Summer")]
		Survive15MinutesWithSummer = 50,
		[AchievementValue("I will protect you", "Survive 30 minutes with Summer")]
		Survive30MinutesWithSummer = 51,

		#endregion
		
		#region Obtain characters
		
		[AchievementValue("Let the bloodshed begin", "Obtain Corina")]
		ObtainCorina_E0 = 52,
		[AchievementValue("The red moon rises once again", "Unlock final shard of Corina")]
		ObtainCorina_E5 = 53,
		[AchievementValue("Sleep tight little one", "Obtain Lucy")]
		ObtainLucy_E0 = 54,
		[AchievementValue("Decay escapes no one", "Unlock final shard of Lucy")]
		ObtainLucy_E5 = 55,
		[AchievementValue("Light of hope", "Obtain final shard of Amelia")]
		ObtainAmelia_E5 = 56,
		[AchievementValue("Soldiers! Onwards!", "Obtain David")]
		ObtainDavid_E0 = 57,
		[AchievementValue("Fire shall cleanse the land", "Unlock final shard of David")]
		ObtainDavid_E5 = 58,
		[AchievementValue("Glimpses of the past", "Obtain Oana")]
		ObtainOana_E0 = 59,
		[AchievementValue("Rest upon endless layers of ice", "Unlock final shard of Oana")]
		ObtainOana_E5 = 60,
		[AchievementValue("In need of a hero", "Obtain Alice")]
		ObtainAlice_E0 = 61,
		[AchievementValue("Thundering retribution", "Unlock final shard of Alice")]
		ObtainAlice_E5 = 62,
		[AchievementValue("Tainted dreams", "Obtain Amelisana")]
		ObtainAmelisana_E0 = 63,
		[AchievementValue("All shall suffer as I did", "Unlock final shard of Amelisana")]
		ObtainAmelisana_E5 = 64,
		[AchievementValue("The end is nigh", "Obtain Chronastra")]
		ObtainChornastra_E0 = 65,
		[AchievementValue("Requiem for a Lost World", "Unlock final shard of Chronastra")]
		ObtainChornastra_E5 = 66,
		[AchievementValue("Time waits for nobody", "Obtain Truzi")]
		ObtainTruzi_E0 = 67,
		[AchievementValue("Untapped potential", "Unlock final shard of Truzi")]
		ObtainTruzi_E5 = 68,
		[AchievementValue("Void consumes all", "Obtain Arika")]
		ObtainArika_E0 = 69,
		[AchievementValue("Promise of the end", "Unlock final shard of Arika")]
		ObtainArika_E5 = 70,
		[AchievementValue("Wind of peace", "Obtain Natalie")]
		ObtainNatalie_E0 = 71,
		[AchievementValue("The wind shall guide us", "Unlock final shard of Natalie")]
		ObtainNatalie_E5 = 72,
		[AchievementValue("Present!", "Obtain Chitose")]
		ObtainChitose_E0 = 73,
		[AchievementValue("The future is now", "Unlock final shard of Chitose")]
		ObtainChitose_E5 = 74,
		[AchievementValue("*Sigh* More work?", "Obtain Eliza")]
		ObtainEliza_E0 = 75,
		[AchievementValue("Vacation time!", "Unlock final shard of Eliza")]
		ObtainEliza_E5 = 76,
		[AchievementValue("Kneel before me!", "Obtain Adam")]
		ObtainAdam_E0 = 77,
		[AchievementValue("All shall know despair", "Unlock final shard of Adam")]
		ObtainAdam_E5 = 78,
		[AchievementValue("Looming darkness", "Obtain final shard of Nishi")]
		ObtainNishi_E5 = 79,
		[AchievementValue("I alone will forge my destiny", "Obtain Amelia")]
		ObtainAmelia_BoD_E0 = 80,
		[AchievementValue("Never falter, never surrender", "Unlock final shard of Amelia")]
		ObtainAmelia_BoD_E5 = 81,
		[AchievementValue("Arrow of novae", "Obtain Summer")]
		ObtainSummer_E0 = 82,
		[AchievementValue("The cutest of them all", "Unlock final shard of Summer")]
		ObtainSummer_E5 = 83,
		
		#endregion

	}
}