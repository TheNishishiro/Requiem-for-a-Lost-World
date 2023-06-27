using System;
using DefaultNamespace;
using UnityEngine;

namespace Objects.Environment
{
	public class Destructable : MonoBehaviour
	{
		private Damageable damageable;
		
		public void Awake()
		{
			damageable = GetComponent<Damageable>();
			damageable.SetHealth(1);
		}

		public void Update()
		{
			if (damageable.IsDestroyed())
			{
				GetComponent<DropOnDestroy>()?.CheckDrop();
				Destroy(gameObject);
			}
		}
	}
}