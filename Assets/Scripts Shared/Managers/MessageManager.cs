using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
	public class MessageManager : MonoBehaviour
	{
		public static MessageManager instance;
		[SerializeField] private GameObject damageMessage;
		private ObjectPool<TextMeshPro> _objectPool;
		private string _postText;
		private Color _postColor;
		private Vector3 _postPosition;
		private Quaternion _postRotation;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			_objectPool = new ObjectPool<TextMeshPro>(
				() => Instantiate(damageMessage, transform).GetComponent<TextMeshPro>(), 
				message =>
				{
					message.gameObject.SetActive(true);
					message.color = _postColor;
					message.transform.position = Utilities.GetRandomInArea(_postPosition, 0.15f);
					message.transform.localRotation = _postRotation;
					message.text = _postText;
				}, 
				message =>
				{
					message.gameObject.SetActive(false);
				}, 
				expGem =>
				{
					Destroy(expGem.gameObject);
				}, false, 10000);
		}

		public void PostMessage(string text, Vector3 worldPosition, Quaternion rotation, Color color)
		{
			_postText = text;
			_postColor = color;
			_postPosition = worldPosition;
			_postRotation = rotation;
			_objectPool.Get();
		}
	}
}