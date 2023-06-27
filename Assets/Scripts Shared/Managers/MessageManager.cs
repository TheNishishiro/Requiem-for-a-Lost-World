using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
	public class MessageManager : MonoBehaviour
	{
		public static MessageManager instance;
		[SerializeField] private GameObject damageMessage;
		private List<TMPro.TextMeshPro> _messagePool;
		private int _objectCount = 30;
		private int _count;
		
		private void Start()
		{
			_messagePool = new List<TextMeshPro>();
			for (var i = 0; i < _objectCount; i++)
				Populate();
		}

		private void Awake()
		{
			instance = this;
		}

		public void Populate()
		{
			var go = Instantiate(damageMessage, transform);
			_messagePool.Add(go.GetComponent<TMPro.TextMeshPro>());
			go.SetActive(false);
		}

		public void PostMessage(string text, Vector3 worldPosition, Quaternion rotation)
		{
			_messagePool[_count].gameObject.SetActive(true);
			_messagePool[_count].transform.position = worldPosition;
			_messagePool[_count].transform.localRotation = rotation;
			_messagePool[_count].text = text;
			_count += 1;
			if (_count >= _objectCount)
				_count = 0;
		}
	}
}