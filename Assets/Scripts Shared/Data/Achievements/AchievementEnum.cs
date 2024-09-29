
using System.Collections.Generic;
using DefaultNamespace.Steam.Attributes;
using Objects.Characters;

namespace DefaultNamespace.Data.Achievements
{
	public enum AchievementEnum // 130
	{
		[SteamAchievement("NEW_ACHIEVEMENT_HOLD6WEAPONS")]
		[AchievementValue("Jack of all trades", "Obtain 6 weapons in a single game", AchievementSection.Misc, Rarity.Common)]
		Hold6Weapons = 0,
		[AchievementValue("Have a bunch of handy tools", "Obtain 6 items in a single game", AchievementSection.Misc, Rarity.Common)]
		Hold6Items = 1,
		[SteamAchievement("NEW_ACHIEVEMENT_KILLENEMIES")]
		[AchievementValue("Let the bodies hit the floor", "Prove your worth by staining the ground with the blood of your enemies", AchievementSection.Combat, Rarity.Common, RequirementType.EnemyKills, 100000)]
		Kill100000Enemies = 2,
		[AchievementValue("All seeing", "Find a map hidden in the in the Capital outskirts", AchievementSection.Misc, Rarity.Rare)]
		CollectMap = 3,
		[SteamAchievement("NEW_ACHIEVEMENT_COLLECTMAGNET")]
		[AchievementValue("Master of shards", "Summon shards of fallen enemies", AchievementSection.Environment, Rarity.Common)]
		CollectMagnet = 4,
		[SteamAchievement("NEW_ACHIEVEMENT_COLLECT100PICKUPS")]
		[AchievementValue("What's in the crate?", "Collect 100 pickups throughout your adventures", AchievementSection.Environment, Rarity.Rare, RequirementType.Pickups, 100)]
		Collect100Pickups = 5,
		[AchievementValue("Can't resist the shiny, can you?", "Collect 1000 pickups throughout your adventures", AchievementSection.Environment, Rarity.Legendary, RequirementType.Pickups, 1000)]
		Collect1000Pickups = 6,
		[AchievementValue("Summon a character from the past", "Obtain a character shard", AchievementSection.Collection, Rarity.Common)]
		PerformGacha = 7,
		[AchievementValue("I'm not addicted", "Obtain 50 character shards", AchievementSection.Collection, Rarity.Legendary, RewardType.Gems, 1000, RequirementType.Shards, 50)]
		PerformGacha100Times = 8,
		[SteamAchievement("NEW_ACHIEVEMENT_KILLENEMIES2")]
		[AchievementValue("That's it?", "Kill a 1000 enemies in a single expedition", AchievementSection.Combat, Rarity.Common, RequirementType.EnemyKillInSingleGame, 1000)]
		Kill1000EnemiesInOneGame = 9,
		[AchievementValue("Lucky draw (I)", "Have your first weapon be of legendary rarity", AchievementSection.Misc, Rarity.Rare)]
		HaveFirst5StarWeapon = 10,
		[SteamAchievement("NEW_ACHIEVEMENT_HAVEFIRST5STARITEM")]
		[AchievementValue("Lucky draw (II)", "Have your first item be of legendary rarity", AchievementSection.Misc, Rarity.Rare)]
		HaveFirst5StarItem = 11,
		[AchievementValue("Who needs a lucky clover?", "Obtain 20 items of legendary rarity", AchievementSection.Misc, Rarity.Rare, RequirementType.LegendaryItems, 20)]
		Obtain10HighRarityItems = 12,
		[SteamAchievement("NEW_ACHIEVEMENT_DIEONCE")]
		[AchievementValue("Sometimes death is just the beginning", "Fall during an expedition once", AchievementSection.Combat, Rarity.Common)]
		DieOnce = 13,
		[SteamAchievement("NEW_ACHIEVEMENT_HEALINONEGAME")]
		[AchievementValue("Be healthy on your adventures", "Heal a 1000 health in a single game", AchievementSection.Combat, Rarity.Common, RequirementType.RegenerationInSingleGame, 1000)]
		Heal1000HealthInOneGame = 14,
		[SteamAchievement("NEW_ACHIEVEMENT_TAKEDAMAGE")]
		[AchievementValue("Sticks and stones can break my bones...", "Take a total of 1000 damage in a single game", AchievementSection.Combat, Rarity.Common, RequirementType.DamageTakenInSingleGame, 1000)]
		Take1000DamageInOneGame = 15,
		[AchievementValue("And he welcomed her as his old friend", "Have your expedition end prematurely 20 times", AchievementSection.Combat, Rarity.Common, RequirementType.Deaths, 20)]
		Die20Times = 16,
		[SteamAchievement("NEW_ACHIEVEMENT_UNLOCKWINDSHEAR")]
		[AchievementValue("Blessed by the wind", "Find a wind orb in the Capital Outskirts", AchievementSection.Environment, Rarity.Legendary)]
		UnlockWindShear = 17,
		[SteamAchievement("NEW_ACHIEVEMENT_LIGHTCHAR")]
		[AchievementValue("Witness Octi's ugliness", "Survive 30 minutes with a light aligned character", AchievementSection.Combat, Rarity.Rare, RequirementType.TimeSpendWithLightCharacter, 30)]
		Survive30MinutesWithLightCharacter = 84,
		[SteamAchievement("NEW_ACHIEVEMENT_DARKCHAR")]
		[AchievementValue("Witness Octi's brilliance", "Survive 30 minutes with a dark aligned character", AchievementSection.Combat, Rarity.Rare, RequirementType.TimeSpendWithDarkCharacter, 30)]
		Survive30MinutesWithDarkCharacter = 85,
		[SteamAchievement("NEW_ACHIEVEMENT_PLAYFOR5HOURS")]
		[AchievementValue("Long journey", "Play for a total of 5 hours", AchievementSection.Misc, Rarity.Common, RequirementType.PlayTime, 5*60)]
		PlayFor5Hours = 86,		
		[SteamAchievement("NEW_ACHIEVEMENT_KILLBOSSENEMIES")]
		[AchievementValue("David and Goliath", "Defeat 500 boss enemies", AchievementSection.Combat, Rarity.Rare, RequirementType.EnemyBossKills, 500)]
		Kill500BossEnemies = 87,
		[SteamAchievement("NEW_ACHIEVEMENT_HEALANDTAKE")]
		[AchievementValue("The Path of Pain", "Take and heal 1500 damage in a single game", AchievementSection.Combat, Rarity.Common, RequirementType.HealAndDamageInSingleGame, 1500)]
		HealAndTake6000Damage = 88,
		[SteamAchievement("NEW_ACHIEVEMENT_REACHHIGHCDR")]
		[AchievementValue("Haste", "Have over 55% cooldown reduction upon leveling up", AchievementSection.Combat, Rarity.Rare)]
		Reach55PercentCdr = 89,
		[SteamAchievement("NEW_ACHIEVEMENT_WALKATHOUSANDSTEPS")]
		[AchievementValue("Path of Thousand Steps", "Walk for 10 kilometers", AchievementSection.Misc, Rarity.Rare, RequirementType.DistanceTraveled, 10000)]
		WalkAThousandSteps = 94,
		[SteamAchievement("NEW_ACHIEVEMENT_DECIDEALREADY")]
		[AchievementValue("Decide already", "Be indecisive for too long", AchievementSection.Misc, Rarity.Legendary)]
		ScrollCharactersTooManyTimes = 95,
		[SteamAchievement("NEW_ACHIEVEMENT_VISITSTONEHENGE")]
		[AchievementValue("Is this Stonehenge?", "Visit Stonehenge in the Capital Outskirts", AchievementSection.Environment, Rarity.Legendary)]
		VisitStonehenge = 100,
		[SteamAchievement("NEW_ACHIEVEMENT_HHRARITYEWEAPONS")]
		[AchievementValue("Earth Bender", "Obtain at least 2 earth type weapons of rare quality or higher in a single game", AchievementSection.Misc, Rarity.Legendary)]
		HoldHighRarityEarthWeapons = 101,
		[SteamAchievement("NEW_ACHIEVEMENT_VISIT100SHRINES")]
		[AchievementValue("Secret Arcana", "Visit 100 shrines in your journey", AchievementSection.Environment, Rarity.Rare, RequirementType.Shrines, 100)]
		Visit100Shrines = 102,
		[SteamAchievement("NEW_ACHIEVEMENT_FINDSECRETROOM")]
		[AchievementValue("Chamber of Secrets", "Find a hidden room in the Capital Undergrounds", AchievementSection.Environment, Rarity.Mythic)]
		FindSecretRoom = 105,
		[SteamAchievement("NEW_ACHIEVEMENT_LEAPOFFAITH")]
		[AchievementValue("Leap of Faith", "Survive an unfortunate fall", AchievementSection.Environment, Rarity.Rare)]
		PerformLeapOfFaith = 106,
		[SteamAchievement("NEW_ACHIEVEMENT_BANISHEVIL")]
		[AchievementValue("Banish Evil", "Defeat The Watcher in the Capital Undergrounds", AchievementSection.Environment, Rarity.Legendary)]
		BanishEvil = 107,
		[AchievementValue("Dead End", "Enter the dark corridor with no way forward", AchievementSection.Environment, Rarity.Common)]
		DeadEnd = 108,
		[AchievementValue("Enter the Dungeon", "Visit Capital Undergrounds for the first time", AchievementSection.Environment, Rarity.Common)]
		EnterTheDungeon = 109,
		[AchievementValue("Is this the way out?", "Find a potential exist from the undergrounds", AchievementSection.Environment, Rarity.Rare)]
		IsThisTheWayOut = 110,
		[SteamAchievement("NEW_ACHIEVEMENT_PORTALPICKUP")]
		[AchievementValue("Not-so-hidden treasure", "Loot a chest in the Capital Undergrounds", AchievementSection.Environment, Rarity.Common)]
		PortalPickUp = 111,
		[SteamAchievement("NEW_ACHIEVEMENT_OWN3FOLLOWUPWEAPONS")]
		[AchievementValue("Master of strategy", "In a single game obtain 3 follow-up type weapons", AchievementSection.Misc, Rarity.Rare)]
		Own3FollowUpWeapons = 112,
		[AchievementValue("Ever-frost seed", "Keep a seed from defrosting", AchievementSection.Combat, Rarity.Mythic)]
		KeepFrostSeedUpFor30Seconds = 113,
		[SteamAchievement("NEW_ACHIEVEMENT_ANOSMIA")]
		[AchievementValue("Anosmia", "Expose yourself to smell for 5 minutes", AchievementSection.Combat, Rarity.Legendary)]
		StayInSewerFor5Minutes = 114,
		[SteamAchievement("NEW_ACHIEVEMENT_BTSCARF")]
		[AchievementValue("My precious gift", "Retrieve the scarf belonging to Sev's friend", AchievementSection.Environment, Rarity.Mythic)]
		UnlockBloodThornScarf = 115,
		[SteamAchievement("NEW_ACHIEVEMENT_BRUTETALISMAN")]
		[AchievementValue("Brute Force", "Find a talisman belonging to a captured criminal", AchievementSection.Combat, Rarity.Legendary)]
		UnlockBruteTalisman = 116,
		[SteamAchievement("NEW_ACHIEVEMENT_UNLOCKLOWPRESSURE")]
		[AchievementValue("Finally, a quiet place", "Read inscription left by a scholar", AchievementSection.Environment, Rarity.Legendary)]
		UnlockLowPressure = 117,
		[SteamAchievement("NEW_ACHIEVEMENT_PEAKOFPEAKS")]
		[AchievementValue("I see no god up here", "Climb the peak of Capital Outskirts", AchievementSection.Environment, Rarity.Rare)]
		PeakofPeaks = 118,
		[SteamAchievement("NEW_ACHIEVEMENT_MASTEROFELEMENTS")]
		[AchievementValue("Master of Elements", "Trigger a total of 10000 reactions", AchievementSection.Environment, Rarity.Rare, RequirementType.Reactions, 10000)]
		MasterOfElements = 119,
		[SteamAchievement("NEW_ACHIEVEMENT_ICEMASTERY")]
		[AchievementValue("Ice Mastery", "Own at least 4 ice weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		IceMastery = 120,
		[SteamAchievement("NEW_ACHIEVEMENT_FIREMASTERY")]
		[AchievementValue("Fire Mastery", "Own at least 4 fire weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		FireMastery = 121,
		[SteamAchievement("NEW_ACHIEVEMENT_EARTHMASTERY")]
		[AchievementValue("Earth Mastery", "Own at least 4 earth weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		EarthMastery = 122,
		[SteamAchievement("NEW_ACHIEVEMENT_LIGHTNINGMASTERY")]
		[AchievementValue("Lightning Mastery", "Own at least 4 lightning weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		LightningMastery = 123,
		[SteamAchievement("NEW_ACHIEVEMENT_WINDMASTERY")]
		[AchievementValue("Wind Mastery", "Own at least 4 wind weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		WindMastery = 124,
		[SteamAchievement("NEW_ACHIEVEMENT_COSMICMASTERY")]
		[AchievementValue("Cosmic Mastery", "Own at least 4 cosmic weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		CosmicMastery = 125,
		[SteamAchievement("NEW_ACHIEVEMENT_LIGHTMASTERY")]
		[AchievementValue("Light Mastery", "Own at least 4 light weapons in a single game", AchievementSection.Misc, Rarity.Rare)]
		LightMastery = 126,
		[SteamAchievement("NEW_ACHIEVEMENT_FOLLOWTHEPHANTOM")]
		[AchievementValue("Illuminate my way", "Follow the light", AchievementSection.Environment, Rarity.Legendary)]
		FollowThePhantom = 127,
		[SteamAchievement("NEW_ACHIEVEMENT_FILLUPRUNEPAGE")]
		[AchievementValue("Full Page", "Fill up one character rune page", AchievementSection.Misc, Rarity.Common)]
		FillUpRunePage = 128,
		[SteamAchievement("NEW_ACHIEVEMENT_OWNALLCHARACTERS")]
		[AchievementValue("Catch'em all...?", "Obtain every character", AchievementSection.Collection, Rarity.Legendary, RequirementType.CharactersOwned, 1)] // 1 is a filler
		OwnAllCharacters = 129,
		[SteamAchievement("NEW_ACHIEVEMENT_OBTAINSANDSTORM")]
		[AchievementValue("Will of the desert", "Find sand orb within Capital Undergrounds", AchievementSection.Environment, Rarity.Rare)] // 1 is a filler
		ObtainSandStorm = 130,
		
		#region Character specific achievements
		[AchievementValue("Bound by fate", "Bind enemies for 1 hour in total in a single game using Amelisana", AchievementSection.Character, Rarity.Common, CharactersEnum.Amelisana_BoN, RequirementType.None, 1)]
		Amelisana_EnemyBindTime = 96,
		[AchievementValue("Power of Nightmare", "Increase damage by at least 30 upon consuming <color=red>Bind</color>", AchievementSection.Character, Rarity.Legendary, CharactersEnum.Amelisana_BoN, RequirementType.None, 1)]
		Amelisana_ConsumeBinding = 97,
		[AchievementValue("Defy reason", "Finish a game without using your ultimate as Amelisana", AchievementSection.Character, Rarity.Mythic, CharactersEnum.Amelisana_BoN, RequirementType.None, 1)]
		Amelisana_FinishWithoutUsingUltimate = 98,
		[AchievementValue("Inner struggle", "As Amelisana gather inventory of conflicting forces", AchievementSection.Character, Rarity.Rare, CharactersEnum.Amelisana_BoN, RequirementType.None, 1)]
		Amelisana_LightAndCosmicOnly = 99,
		[AchievementValue("Transcendence", "As Yami-no-Tokiya deal a total of 20 million damage in a single game", AchievementSection.Character, Rarity.Rare, CharactersEnum.Chornastra_BoR, RequirementType.None, 1)]
		Chronastra_DamageDealt = 103,
		[AchievementValue("Field of paper flowers", "As Yami-no-Tokiya pick up a 1000 Abyss Flowers", AchievementSection.Character, Rarity.Rare, CharactersEnum.Chornastra_BoR, RequirementType.AbyssFlowers, 1000)]
		Chronastra_FlowersPickup = 104,
		#endregion
		
		#region Survive with characters

		[SteamAchievement("NEW_ACHIEVEMENT_CORINA15")]
		[AchievementValue("Barely a warmup","Survive 15 minutes with Corina", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Corina_BoB, RequirementType.PlayTime, 15)]
		Survive15MinutesWithCorina = 18,
		[SteamAchievement("NEW_ACHIEVEMENT_CORINA30")]
		[AchievementValue("Taste the blood of strong", "Survive 30 minutes with Corina", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Corina_BoB, RequirementType.PlayTime, 30)]
		Survive30MinutesWithCorina = 19,
		[SteamAchievement("NEW_ACHIEVEMENT_LUCY15")]
		[AchievementValue("Corruption spreading", "Survive 15 minutes with Lucy", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Lucy_BoC, RequirementType.PlayTime, 15)]
		Survive15MinutesWithLucy = 20,
		[SteamAchievement("NEW_ACHIEVEMENT_LUCY30")]
		[AchievementValue("I shall consume it all", "Survive 30 minutes with Lucy", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Lucy_BoC, RequirementType.PlayTime, 30)]
		Survive30MinutesWithLucy = 21,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIA15")]
		[AchievementValue("Beginning of my journey", "Survive 15 minutes with Amelia", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Amelia, RequirementType.PlayTime, 15)]
		Survive15MinutesWithAmelia = 22,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIA30")]
		[AchievementValue("Are you proud of me?", "Survive 30 minutes with Amelia", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Amelia, RequirementType.PlayTime, 30)]
		Survive30MinutesWithAmelia = 23,
		[SteamAchievement("NEW_ACHIEVEMENT_DAVID15")]
		[AchievementValue("Relentless march", "Survive 15 minutes with David", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.David_BoF, RequirementType.PlayTime, 15)]
		Survive15MinutesWithDavid = 24,
		[SteamAchievement("NEW_ACHIEVEMENT_DAVID30")]
		[AchievementValue("Scorch the earth", "Survive 30 minutes with David", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.David_BoF, RequirementType.PlayTime, 30)]
		Survive30MinutesWithDavid = 25,
		[SteamAchievement("NEW_ACHIEVEMENT_OANA15")]
		[AchievementValue("A walk in the park", "Survive 15 minutes with Oana", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Oana_BoI, RequirementType.PlayTime, 15)]
		Survive15MinutesWithOana = 26,
		[SteamAchievement("NEW_ACHIEVEMENT_OANA30")]
		[AchievementValue("It's getting cold in here", "Survive 30 minutes with Oana", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Oana_BoI, RequirementType.PlayTime, 30)]
		Survive30MinutesWithOana = 27,
		[SteamAchievement("NEW_ACHIEVEMENT_ALICE15")]
		[AchievementValue("It's nothing", "Survive 15 minutes with Alice", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Alice_BoL, RequirementType.PlayTime, 15)]
		Survive15MinutesWithAlice = 28,
		[SteamAchievement("NEW_ACHIEVEMENT_ALICE30")]
		[AchievementValue("Just like old times", "Survive 30 minutes with Alice", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Alice_BoL, RequirementType.PlayTime, 30)]
		Survive30MinutesWithAlice = 29,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELISANA15")]
		[AchievementValue("Not over yet", "Survive 15 minutes with Amelisana", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Amelisana_BoN, RequirementType.PlayTime, 15)]
		Survive15MinutesWithAmelisana = 30,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELISANA30")]
		[AchievementValue("AHAHAHAHAHA", "Survive 30 minutes with Amelisana", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Amelisana_BoN, RequirementType.PlayTime, 30)]
		Survive30MinutesWithAmelisana = 31,
		[SteamAchievement("NEW_ACHIEVEMENT_YAMI15")]
		[AchievementValue("Puny mortals", "Survive 15 minutes with Yami-no-Tokiya", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Chornastra_BoR, RequirementType.PlayTime, 15)]
		Survive15MinutesWithChornastra = 32,
		[SteamAchievement("NEW_ACHIEVEMENT_YAMI30")]
		[AchievementValue("I am the end", "Survive 30 minutes with Yami-no-Tokiya", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Chornastra_BoR, RequirementType.PlayTime, 30)]
		Survive30MinutesWithChornastra = 33,
		[SteamAchievement("NEW_ACHIEVEMENT_TRUZI15")]
		[AchievementValue("Is it over yet?", "Survive 15 minutes with Truzi", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Truzi_BoT, RequirementType.PlayTime, 15)]
		Survive15MinutesWithTruzi = 34,
		[SteamAchievement("NEW_ACHIEVEMENT_TRUZI30")]
		[AchievementValue("Time waits for no one", "Survive 30 minutes with Truzi", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Truzi_BoT, RequirementType.PlayTime, 30)]
		Survive30MinutesWithTruzi = 35,
		[SteamAchievement("NEW_ACHIEVEMENT_ARIKA15")]
		[AchievementValue("One step closer", "Survive 15 minutes with Arika", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Arika_BoV, RequirementType.PlayTime, 15)]
		Survive15MinutesWithArika = 36,
		[SteamAchievement("NEW_ACHIEVEMENT_ARIKA30")]
		[AchievementValue("Transcendence", "Survive 30 minutes with Arika", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Arika_BoV, RequirementType.PlayTime, 30)]
		Survive30MinutesWithArika = 37,
		[SteamAchievement("NEW_ACHIEVEMENT_NATALIE15")]
		[AchievementValue("The wind is my ally", "Survive 15 minutes with Natalie", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Natalie_BoW, RequirementType.PlayTime, 15)]
		Survive15MinutesWithNatalie = 38,
		[SteamAchievement("NEW_ACHIEVEMENT_NATALIE30")]
		[AchievementValue("Erode the land", "Survive 30 minutes with Natalie", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Natalie_BoW, RequirementType.PlayTime, 30)]
		Survive30MinutesWithNatalie = 39,
		[SteamAchievement("NEW_ACHIEVEMENT_CHITOSE15")]
		[AchievementValue("First real expedition", "Survive 15 minutes with Chitose", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Chitose, RequirementType.PlayTime, 15)]
		Survive15MinutesWithChitose = 40,
		[SteamAchievement("NEW_ACHIEVEMENT_CHITOSE30")]
		[AchievementValue("Can I be a knight yet?", "Survive 30 minutes with Chitose", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Chitose, RequirementType.PlayTime, 30)]
		Survive30MinutesWithChitose = 41,
		[SteamAchievement("NEW_ACHIEVEMENT_ELIZA15")]
		[AchievementValue("I'm not a maid!", "Survive 15 minutes with Eliza", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Maid, RequirementType.PlayTime, 15)]
		Survive15MinutesWithEliza = 42,
		[SteamAchievement("NEW_ACHIEVEMENT_ELIZA30")]
		[AchievementValue("I'm still not a maid!", "Survive 30 minutes with Eliza", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Maid, RequirementType.PlayTime, 30)]
		Survive30MinutesWithEliza = 43,
		[SteamAchievement("NEW_ACHIEVEMENT_ADAM15")]
		[AchievementValue("When will they learn?", "Survive 15 minutes with Adam", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Adam_OBoV, RequirementType.PlayTime, 15)]
		Survive15MinutesWithAdam = 44,
		[SteamAchievement("NEW_ACHIEVEMENT_ADAM30")]
		[AchievementValue("I am the end of all", "Survive 30 minutes with Adam", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Adam_OBoV, RequirementType.PlayTime, 30)]
		Survive30MinutesWithAdam = 45,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIABOD15")]
		[AchievementValue("Purge the evil", "Survive 15 minutes with Bearer of Dream", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Amelia_BoD, RequirementType.PlayTime, 15)]
		Survive15MinutesWithAmelia_BoD = 46,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIABOD30")]
		[AchievementValue("I will light the way!", "Survive 30 minutes with Bearer of Dream", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Amelia_BoD, RequirementType.PlayTime, 30)]
		Survive30MinutesWithAmelia_BoD = 47,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHI15")]
		[AchievementValue("Prove your worth", "Survive 15 minutes with Nishi", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Nishi, RequirementType.PlayTime, 15)]
		Survive15MinutesWithNishi = 48,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHI30")]
		[AchievementValue("I will not fail", "Survive 30 minutes with Nishi", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Nishi, RequirementType.PlayTime, 30)]
		Survive30MinutesWithNishi = 49,
		[SteamAchievement("NEW_ACHIEVEMENT_SUMMER15")]
		[AchievementValue("Arrow guide my way", "Survive 15 minutes with Summer", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Summer, RequirementType.PlayTime, 15)]
		Survive15MinutesWithSummer = 50,
		[SteamAchievement("NEW_ACHIEVEMENT_SUMMER30")]
		[AchievementValue("I will protect you", "Survive 30 minutes with Summer", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Summer, RequirementType.PlayTime, 30)]
		Survive30MinutesWithSummer = 51,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHIHOF15")]
		[AchievementValue("Embrace the evil", "Survive 15 minutes with Vanquisher of Faith", AchievementSection.CharacterSurvival, Rarity.Common, CharactersEnum.Nishi_HoF, RequirementType.PlayTime, 15)]
		Survive15MinutesWithNishi_HoF = 90,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHIHOF30")]
		[AchievementValue("I will extinguish all light", "Survive 30 minutes with Vanquisher of Faith", AchievementSection.CharacterSurvival, Rarity.Rare, CharactersEnum.Nishi_HoF, RequirementType.PlayTime, 30)]
		Survive30MinutesWithNishi_HoF = 91,

		#endregion
		
		#region Obtain characters
		
		[AchievementValue("Let the bloodshed begin", "Obtain Corina", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Corina_BoB, RequirementType.Shards, 0)]
		ObtainCorina_E0 = 52,
		[SteamAchievement("NEW_ACHIEVEMENT_CORINA_S5")]
		[AchievementValue("The red moon rises once again", "Unlock final shard of Corina", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Corina_BoB, RequirementType.Shards, 5)]
		ObtainCorina_E5 = 53,
		[AchievementValue("Sleep tight little one", "Obtain Lucy", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Lucy_BoC, RequirementType.Shards, 0)]
		ObtainLucy_E0 = 54,
		[SteamAchievement("NEW_ACHIEVEMENT_LUCY_S5")]
		[AchievementValue("Decay escapes no one", "Unlock final shard of Lucy", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Lucy_BoC, RequirementType.Shards, 5)]
		ObtainLucy_E5 = 55,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIA_S5")]
		[AchievementValue("Light of hope", "Obtain final shard of Amelia", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Amelia, RequirementType.Shards, 5)]
		ObtainAmelia_E5 = 56,
		[AchievementValue("Soldiers! Onwards!", "Obtain David", AchievementSection.Collection, Rarity.Rare, CharactersEnum.David_BoF, RequirementType.Shards, 0)]
		ObtainDavid_E0 = 57,
		[SteamAchievement("NEW_ACHIEVEMENT_DAVID_S5")]
		[AchievementValue("Fire shall cleanse the land", "Unlock final shard of David", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.David_BoF, RequirementType.Shards, 5)]
		ObtainDavid_E5 = 58,
		[AchievementValue("Glimpses of the past", "Obtain Oana", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Oana_BoI, RequirementType.Shards, 0)]
		ObtainOana_E0 = 59,
		[SteamAchievement("NEW_ACHIEVEMENT_OANA_S5")]
		[AchievementValue("Rest upon endless layers of ice", "Unlock final shard of Oana", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Oana_BoI, RequirementType.Shards, 5)]
		ObtainOana_E5 = 60,
		[AchievementValue("In need of a hero", "Obtain Alice", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Alice_BoL, RequirementType.Shards, 0)]
		ObtainAlice_E0 = 61,
		[SteamAchievement("NEW_ACHIEVEMENT_ALICE_S5")]
		[AchievementValue("Thundering retribution", "Unlock final shard of Alice", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Alice_BoL, RequirementType.Shards, 5)]
		ObtainAlice_E5 = 62,
		[AchievementValue("Tainted dreams", "Obtain Amelisana", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Amelisana_BoN, RequirementType.Shards, 0)]
		ObtainAmelisana_E0 = 63,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELISANA_S5")]
		[AchievementValue("All shall suffer as I did", "Unlock final shard of Amelisana", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Amelisana_BoN, RequirementType.Shards, 5)]
		ObtainAmelisana_E5 = 64,
		[AchievementValue("The end is nigh", "Obtain Yami-no-Tokiya", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Chornastra_BoR, RequirementType.Shards, 0)]
		ObtainChornastra_E0 = 65,
		[SteamAchievement("NEW_ACHIEVEMENT_YAMI_S5")]
		[AchievementValue("Requiem for a Lost World", "Unlock final shard of Yami-no-Tokiya", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Chornastra_BoR, RequirementType.Shards, 5)]
		ObtainChornastra_E5 = 66,
		[AchievementValue("Time waits for nobody", "Obtain Truzi", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Truzi_BoT, RequirementType.Shards, 0)]
		ObtainTruzi_E0 = 67,
		[SteamAchievement("NEW_ACHIEVEMENT_TRUZI_S5")]
		[AchievementValue("Untapped potential", "Unlock final shard of Truzi", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Truzi_BoT, RequirementType.Shards, 5)]
		ObtainTruzi_E5 = 68,
		[AchievementValue("Void consumes all", "Obtain Arika", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Arika_BoV, RequirementType.Shards, 0)]
		ObtainArika_E0 = 69,
		[SteamAchievement("NEW_ACHIEVEMENT_ARIKA_S5")]
		[AchievementValue("Promise of the end", "Unlock final shard of Arika", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Arika_BoV, RequirementType.Shards, 5)]
		ObtainArika_E5 = 70,
		[AchievementValue("Wind of peace", "Obtain Natalie", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Natalie_BoW, RequirementType.Shards, 0)]
		ObtainNatalie_E0 = 71,
		[SteamAchievement("NEW_ACHIEVEMENT_NATALIE_S5")]
		[AchievementValue("The wind shall guide us", "Unlock final shard of Natalie", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Natalie_BoW, RequirementType.Shards, 5)]
		ObtainNatalie_E5 = 72,
		[AchievementValue("Present!", "Obtain Chitose", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Chitose, RequirementType.Shards, 0)]
		ObtainChitose_E0 = 73,
		[SteamAchievement("NEW_ACHIEVEMENT_CHITOSE_S5")]
		[AchievementValue("The future is now", "Unlock final shard of Chitose", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Chitose, RequirementType.Shards, 5)]
		ObtainChitose_E5 = 74,
		[AchievementValue("*Sigh* More work?", "Obtain Eliza", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Maid, RequirementType.Shards, 0)]
		ObtainEliza_E0 = 75,
		[SteamAchievement("NEW_ACHIEVEMENT_ELIZA_S5")]
		[AchievementValue("Vacation time!", "Unlock final shard of Eliza", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Maid, RequirementType.Shards, 5)]
		ObtainEliza_E5 = 76,
		[AchievementValue("Kneel before me!", "Obtain Adam", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Adam_OBoV, RequirementType.Shards, 0)]
		ObtainAdam_E0 = 77,
		[SteamAchievement("NEW_ACHIEVEMENT_ADAM_S5")]
		[AchievementValue("All shall know despair", "Unlock final shard of Adam", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Adam_OBoV, RequirementType.Shards, 5)]
		ObtainAdam_E5 = 78,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHI_S5")]
		[AchievementValue("Looming darkness", "Obtain final shard of Nishi", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Nishi, RequirementType.Shards, 5)]
		ObtainNishi_E5 = 79,
		[AchievementValue("I alone will forge my destiny", "Obtain Amelia", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Amelia_BoD, RequirementType.Shards, 0)]
		ObtainAmelia_BoD_E0 = 80,
		[SteamAchievement("NEW_ACHIEVEMENT_AMELIABOD_S5")]
		[AchievementValue("Never falter, never surrender", "Unlock final shard of Amelia", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Amelia_BoD, RequirementType.Shards, 5)]
		ObtainAmelia_BoD_E5 = 81,
		[AchievementValue("Arrow of novae", "Obtain Summer", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Summer, RequirementType.Shards, 0)]
		ObtainSummer_E0 = 82,
		[SteamAchievement("NEW_ACHIEVEMENT_SUMMER_S5")]
		[AchievementValue("The cutest of them all", "Unlock final shard of Summer", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Summer, RequirementType.Shards, 5)]
		ObtainSummer_E5 = 83,
		[AchievementValue("Time of Reckoning", "Obtain Nishi the Vanquisher of Faith", AchievementSection.Collection, Rarity.Rare, CharactersEnum.Nishi_HoF, RequirementType.Shards, 0)]
		ObtainNishi_HoF_E0 = 92,
		[SteamAchievement("NEW_ACHIEVEMENT_NISHIHOF_S5")]
		[AchievementValue("For you, I will reverse all creation", "Unlock final shard of the Vanquisher of Faith", AchievementSection.Collection, Rarity.Legendary, CharactersEnum.Nishi_HoF, RequirementType.Shards, 5)]
		ObtainNishi_HoF_E5 = 93,
		
		#endregion

	}
}