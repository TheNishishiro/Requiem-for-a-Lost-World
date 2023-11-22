using System;
using Events.Handlers;
using Events.Scripts;
using Objects.Players.Scripts;
using Objects.Stage;

namespace Objects.Characters.Adam.Skill
{
	public class AdamSpecial : CharacterSkillBase
	{
		private float previousStatsChange;
		private PlayerStatsComponent _playerStatsComponent;
		private PlayerStatsComponent PlayerStatsComponent
		{
			get
			{
				if (_playerStatsComponent == null)
				{
					_playerStatsComponent = FindFirstObjectByType<PlayerStatsComponent>();
				}

				return _playerStatsComponent;
			}
		}

		private void LateUpdate()
		{
			PlayerStatsComponent.IncreaseDamageTaken(-previousStatsChange);
			PlayerStatsComponent.IncreaseDamageIncreasePercentage(-previousStatsChange);

			previousStatsChange = EnemyManager.instance.currentEnemyCount * (GameData.IsCharacterRank(CharacterRank.E4) ? 0.005f : 0.001f);
			PlayerStatsComponent.IncreaseDamageTaken(previousStatsChange);
			PlayerStatsComponent.IncreaseDamageIncreasePercentage(previousStatsChange);
		}
	}
}