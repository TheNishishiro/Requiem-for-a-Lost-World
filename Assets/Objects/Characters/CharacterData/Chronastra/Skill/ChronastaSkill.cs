using System;
using System.Collections;
using UnityEngine;

namespace Objects.Characters.Chronastra.Skill
{
	public class ChronastaSkill : MonoBehaviour
	{
		[SerializeField] private Material dissolveMaterial;
		[SerializeField] private EnemyManager _enemyManager;

		public IEnumerator SetScreenShot()
		{
			yield return new WaitForEndOfFrame();
			var width = Screen.width;
			var height = Screen.height;

			var screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
			var rect = new Rect(0, 0, width, height);
			var pixels = screenshotTexture.GetPixels();
			screenshotTexture.SetPixels(pixels);
			screenshotTexture.ReadPixels(rect, 0, 0);
			screenshotTexture.Apply();
        
			dissolveMaterial.SetTexture("_MainTex", screenshotTexture);
			var dissolveAmount = 0f;
			var start = Time.realtimeSinceStartup;
			gameObject.SetActive(true);
			Time.timeScale = 0;
			while (dissolveAmount < 1f)
			{
				dissolveAmount += (Time.realtimeSinceStartup - start)/500;
				dissolveMaterial.SetFloat("_Progress", dissolveAmount);
				yield return null;
			}
			Time.timeScale = 1;
			gameObject.SetActive(false);
		}
	}
}