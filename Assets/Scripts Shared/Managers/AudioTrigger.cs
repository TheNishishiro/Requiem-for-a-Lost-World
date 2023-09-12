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
			Debug.Log("AudioTrigger.OnDamageTaken");
			AudioManager.PlayHitAudio();
		}
		
		private void OnEnable()
		{
			Debug.Log("AudioTrigger.OnEnable");
			DamageTakenEvent.Register(this);
		}
    
		private void OnDisable()
		{
			Debug.Log("AudioTrigger.OnDisable");
			DamageTakenEvent.Unregister(this);
		}
	}
}