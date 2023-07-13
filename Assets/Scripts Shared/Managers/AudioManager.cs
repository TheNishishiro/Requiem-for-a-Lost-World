using System;
using UnityEngine;

namespace Managers
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip buttonClick;
		
		public void PlayButtonClick()
		{
			audioSource.PlayOneShot(buttonClick);
		}
	}
}