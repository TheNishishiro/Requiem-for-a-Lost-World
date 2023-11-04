using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager instance;
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioSource bgmSource;
		[SerializeField] private AudioClip buttonClick;
		[SerializeField] private AudioClip buttonSimpleClick;
		[SerializeField] private AudioClip buttonConfirmClick;
		[SerializeField] private AudioClip buttonSkipClick;
		[SerializeField] private AudioClip buttonCymbalClick;
		[SerializeField] private List<AudioClip> hitClips;
		[SerializeField] private List<AudioClip> shardPickupClips;
		private static bool _playedPickupSoundThisFrame;

		public void Awake()
		{
			instance = this;
		}

		private void LateUpdate()
		{
			_playedPickupSoundThisFrame = false;
		}

		public void PlayBgmClick()
		{
			bgmSource.Play();
		}
		
		public void PlayButtonClick()
		{
			audioSource.PlayOneShot(buttonClick);
		}
		
		public void PlayButtonSimpleClick()
		{
			audioSource.PlayOneShot(buttonSimpleClick);
		}
		
		public void PlayButtonConfirmClick()
		{
			audioSource.PlayOneShot(buttonConfirmClick);
		}
		
		public void PlayButtonSkipClick()
		{
			audioSource.PlayOneShot(buttonSkipClick);
		}
		
		public void PlayButtonCymbalClick()
		{
			audioSource.PlayOneShot(buttonCymbalClick);
		}
		
		public void PlayPlayerHitAudio()
		{
			var audioClip = hitClips[UnityEngine.Random.Range(0, hitClips.Count)];
			audioSource.PlayOneShot(audioClip);
		}
		
		public void PlayExperiencePickup()
		{
			if (_playedPickupSoundThisFrame)
				return;
			
			var audioClip = shardPickupClips[UnityEngine.Random.Range(0, shardPickupClips.Count)];
			audioSource.PlayOneShot(audioClip, 0.5f);
		}
	}
}