using System;
using Discord;
using UnityEngine;
using Discord = Discord.Discord;

namespace Managers
{
	public class DiscordManager : MonoBehaviour
	{
		private global::Discord.Discord _discord;
		private ActivityManager _activityManager;
		
		public void Awake()
		{
			var instances = FindObjectsOfType<AchievementManager>();
			if (instances.Length > 1)
			{
				Destroy(gameObject);
				return;
			}
			
			_discord = new global::Discord.Discord(1126131759722012674, (ulong)CreateFlags.NoRequireDiscord);
			_activityManager = _discord.GetActivityManager();
			DontDestroyOnLoad(gameObject);
		}

		private void Update()
		{
			_discord.RunCallbacks();
		}

		public void UpdateActivity(string details, string state, string image, string imageTitle)
		{
			return;
			
			var activity = new global::Discord.Activity()
			{
				State = state,
				Details = details,
				Assets =
				{
					LargeImage = "icon",
					LargeText = "RogueLite",
					SmallImage = image.ToLower(),
					SmallText = imageTitle
				},
				Timestamps =
				{
					Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				},
				Type = ActivityType.Playing
			};

			_activityManager.UpdateActivity(activity, (res) =>
			{
				if (res == global::Discord.Result.Ok)
				{
					Debug.Log("Discord activity updated!");
				}
				else
				{
					Debug.LogError($"Discord activity failed to update: {res}");
				}
			});
		}
	}
}