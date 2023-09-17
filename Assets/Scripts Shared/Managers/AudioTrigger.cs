using System;
using Events.Handlers;
using Events.Scripts;
using UnityEngine;

namespace Managers
{
	public class AudioTrigger : MonoBehaviour, IDamageTakenHandler
	{
		private AudioManager AudioManager;

		private void Start()
		{
			AudioManager = GetComponent<AudioManager>();
		}

		public void OnDamageTaken(float damage)
		{
			AudioManager.PlayHitAudio();
		}
		
		private void OnEnable()
		{
			DamageTakenEvent.Register(this);
		}
    
		private void OnDisable()
		{
			DamageTakenEvent.Unregister(this);
		}
	}
}