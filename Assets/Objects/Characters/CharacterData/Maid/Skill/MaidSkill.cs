using System;

namespace Objects.Characters.Maid.Skill
{
	public class MaidSkill : CharacterSkillBase
	{
		public void Update()
		{
			TickLifeTime();
		}
	}
}