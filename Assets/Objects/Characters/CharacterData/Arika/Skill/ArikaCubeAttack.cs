using System;
using System.Linq;
using DefaultNamespace;
using Objects.Enemies;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Arika.Skill
{
	public class ArikaCubeAttack : CharacterSkillBase
	{
		[SerializeField] GameObject blackHoleCenter;
		[SerializeField] GameObject starsContainer;
		[SerializeField] GameObject particleContainer;
		private float _attackTimer = 0.5f;
		private bool _isAttacked = false;
		private bool _isAreParticlesActive = false;

		private void Update()
		{
			if (_attackTimer > 0)
			{
				_attackTimer -= Time.deltaTime;

				if (_attackTimer <= 0)
				{
					blackHoleCenter.SetActive(true);
					var arikaRank = GameData.GetPlayerCharacterRank();
					
					foreach (var enemy in EnemyManager.instance.GetActiveEnemies())
					{
						if (arikaRank < CharacterRank.E5)
						{
							var distance = Vector3.Distance(transform.position, enemy.transform.position);
							if (distance > 5f)
								continue;
						}

						enemy?.GetChaseComponent()?.SetTemporaryTarget(blackHoleCenter, 7f, 2f);
						enemy?.SetNoCollisions(2f);
					}
				}
			}
			else
			{
				TickLifeTime();
			}
		}
		
		protected override void OnDestroy()
		{
			if (_isAttacked && !_isAreParticlesActive)
			{
				LifeTime = 1.5f;
				particleContainer.SetActive(true);
				_isAreParticlesActive = true;
				return;
			}
			
			if (_isAreParticlesActive)
			{
				base.OnDestroy();
				return;
			}

			LifeTime = 0.4f;
			_isAttacked = true;
			starsContainer.SetActive(true);
		}
	}
}