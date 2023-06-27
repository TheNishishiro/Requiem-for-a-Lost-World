using UnityEngine;

namespace Weapons
{
	public class ProjectileWithLimitedHitBoxBase : ProjectileBase
	{
		[SerializeField] public Collider collider;
		[SerializeField] public float colliderLifeTime = 0.1f;
		[SerializeField] public float colliderStartLifeTime = 0f;

		protected void UpdateCollider()
		{
			if (TimeAlive > colliderLifeTime)
				OnColliderEnd();
			else if (TimeAlive >= colliderStartLifeTime)
				OnColliderStart();
		}

		protected virtual void OnColliderStart()
		{
			collider.enabled = true;
		}

		protected virtual void OnColliderEnd()
		{
			collider.enabled = false;
		}
	}
}