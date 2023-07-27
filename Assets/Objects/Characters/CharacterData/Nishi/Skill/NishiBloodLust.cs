using System;
using Events.Handlers;
using Events.Scripts;
using Managers;
using UnityEngine;

namespace Objects.Characters.Nishi.Skill
{
	public class NishiBloodLust : CharacterSkillBase, IEnemyDiedHandler
	{
		private SpecialBarManager _specialBarManager;
		private SpecialBarManager SpecialBarManager
		{
			get
			{
				if (_specialBarManager == null)
					_specialBarManager = FindObjectOfType<SpecialBarManager>();
				return _specialBarManager;
			}
		}
		
		private void OnEnable()
		{
			EnemyDiedEvent.Register(this);
		}
    
		private void OnDisable()
		{
			EnemyDiedEvent.Unregister(this);
		}
		
		public void OnEnemyDied()
		{
			SpecialBarManager.Increment();
		}
	}
}