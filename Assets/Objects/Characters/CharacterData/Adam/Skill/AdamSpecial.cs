using System;
using Objects.Players.Scripts;

namespace Objects.Characters.Adam.Skill
{
	public class AdamSpecial : CharacterSkillBase
	{
		private float previousStatsChange;
		private PlayerStatsComponent _playerStatsComponent;
		private PlayerStatsComponent playerStatsComponent
		{
			get
			{
				if (_playerStatsComponent == null)
				{
					_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
				}

				return _playerStatsComponent;
			}
		}

		private void LateUpdate()
		{
			playerStatsComponent.IncreaseDamageTaken(-previousStatsChange);
			playerStatsComponent.IncreaseDamageIncreasePercentage(-previousStatsChange);

			previousStatsChange = EnemyManager.instance.currentEnemyCount * 0.005f;
			playerStatsComponent.IncreaseDamageTaken(previousStatsChange);
			playerStatsComponent.IncreaseDamageIncreasePercentage(previousStatsChange);
		}
	}
}