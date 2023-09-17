using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip buttonClick;
		[SerializeField] private List<AudioClip> hitClips;
		
		public void PlayButtonClick()
		{
			audioSource.PlayOneShot(buttonClick);
		}
		
		public void PlayHitAudio()
		{
			var audioClip = hitClips[UnityEngine.Random.Range(0, hitClips.Count)];
			audioSource.PlayOneShot(audioClip);
		}
	}
}