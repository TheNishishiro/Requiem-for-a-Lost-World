using UnityEngine;

namespace Objects.Characters
{
	public class CharacterSkillBase : MonoBehaviour
	{
		[SerializeField] public float LifeTime;

		protected void TickLifeTime()
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