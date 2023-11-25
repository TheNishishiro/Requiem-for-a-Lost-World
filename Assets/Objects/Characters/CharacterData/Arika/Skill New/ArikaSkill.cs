using System;
using System.Collections;
using Data.Elements;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Characters.Chronastra.Skill
{
	public class ArikaSkill : MonoBehaviour
	{
		[SerializeField] private Material dissolveMaterial;
		[SerializeField] private EnemyManager _enemyManager;
		[SerializeField] private Image image;
		[SerializeField] private GameObject skillPrefab;
		private float _duration;
		
		public void SetDuration(float duration)
		{
			_duration = duration;
		}
		
		public void OnEnable()
		{
			StartCoroutine(SetScreenShot());
		}

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

			var pivot = new Vector2(0.5f, 0.5f);  // Pivot in the middle
			var sprite = Sprite.Create(screenshotTexture, rect, pivot);

			image.sprite = sprite;
			var dissolveAmount = 0f;
			var start = Time.realtimeSinceStartup;
			gameObject.SetActive(true);
			EnemyManager.instance.SetTimeStop(true);
			while (dissolveAmount < 1f)
			{
				dissolveAmount += 0.01f;
				dissolveMaterial.SetFloat("_Progress", dissolveAmount);
				yield return new WaitForSeconds(0.01f);
			}
			EnemyManager.instance.SetTimeStop(false);
			gameObject.SetActive(false);
			var field = SpawnManager.instance.SpawnObject(GameManager.instance.playerComponent.transform.position, skillPrefab);
			field.GetComponent<ArikaExplosion>().SetDuration(_duration);
			EnemyManager.instance.GlobalDamage(100, new ElementalWeapon(Element.Cosmic));
		}
	}
}