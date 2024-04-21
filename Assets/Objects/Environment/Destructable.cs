using System;
using DefaultNamespace;
using UnityEngine;

namespace Objects.Environment
{
	public class Destructable : MonoBehaviour
	{
		private Damageable damageable;
		[SerializeField] private DropOnDestroy dropOnDestroy;
		private bool _isDropping;
		
		public void Awake()
		{
			damageable = GetComponent<Damageable>();
			damageable.SetHealth(1);
			_isDropping = false;
		}

		public void Update()
		{
			if (damageable.IsDestroyed() && !_isDropping)
			{
				_isDropping = true;
				dropOnDestroy.CheckDrop();
				Destroy(gameObject);
			}
		}
	}
}