using UnityEngine;
using Weapons;

namespace Objects.Characters
{
	public class CharacterSkillBase : StagableProjectile
	{
		[SerializeField] public float LifeTime;
		
		protected virtual void TickLifeTime()
		{
			LifeTime -= Time.deltaTime;
			if (LifeTime <= 0)
			{
				OnDestroy();
			}
		}

		protected virtual void OnDestroy()
		{
			Destroy(gameObject);
		}
	}
}