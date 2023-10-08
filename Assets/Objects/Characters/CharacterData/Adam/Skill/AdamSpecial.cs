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
		private EnemyManager _enemyManager;
		private EnemyManager enemyManager
		{
			get
			{
				if (_enemyManager == null)
				{
					_enemyManager = FindObjectOfType<EnemyManager>();
				}

				return _enemyManager;
			}
		}

		private void LateUpdate()
		{
			playerStatsComponent.IncreaseDamageTaken(-previousStatsChange);
			playerStatsComponent.IncreaseDamageIncreasePercentage(-previousStatsChange);

			previousStatsChange = enemyManager.currentEnemyCount * 0.005f;
			playerStatsComponent.IncreaseDamageTaken(previousStatsChange);
			playerStatsComponent.IncreaseDamageIncreasePercentage(previousStatsChange);
		}
	}
}